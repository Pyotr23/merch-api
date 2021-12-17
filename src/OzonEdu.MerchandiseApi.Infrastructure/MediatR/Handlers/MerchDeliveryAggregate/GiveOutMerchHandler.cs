using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Exceptions;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.MessageBroker;
using OzonEdu.MerchandiseApi.Infrastructure.Trace;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.MerchDeliveryAggregate
{
    public class GiveOutMerchHandler : IRequestHandler<GiveOutMerchCommand>
    {
        private const string HandlerName = nameof(GiveOutMerchHandler);
        
        private readonly IMerchService _merchService;
        private readonly IEmployeeService _employeeService;
        private readonly IStockService _stockService;
        private readonly ICustomTracer _tracer;
        private readonly KafkaManager _kafka;

        public GiveOutMerchHandler(IMerchService merchService,
            IEmployeeService employeeService,
            IStockService stockService,
            ICustomTracer tracer,
            KafkaManager kafka)
        {
            _kafka = kafka;
            _tracer = tracer;
            _merchService = merchService;
            _employeeService = employeeService;
            _stockService = stockService;
            
        }
        
        public async Task<Unit> Handle(GiveOutMerchCommand request, CancellationToken token)
        {
            using var span = _tracer.GetSpan(HandlerName, nameof(Handle));
            
            var employee = await _employeeService.FindAsync(request.EmployeeId, token);
            if (employee is null)
                throw new NotExistsException($"Employee with id={request.EmployeeId} does not exists");

            var merchPackType = await _merchService.FindMerchPackType(request.MerchPackTypeId, token);
            if (merchPackType is null)
            {
                throw new NotExistsException(
                    $"Merch pack type with id={request.MerchPackTypeId} does not exists");
            }

            var merchDelivery = employee
                                    .MerchDeliveries
                                    .FirstOrDefault(d => d.MerchPackType.Id == merchPackType.Id);

            if (merchDelivery is null)
            {
                merchDelivery = await _merchService.CreateMerchDeliveryAsync(merchPackType, 
                    employee.ClothingSize, 
                    token);
                
                await _employeeService.AddMerchDelivery(employee.Id, merchDelivery.Id, token);
            }
            
            if (merchDelivery.Status.Equals(MerchDeliveryStatus.Done))
                return Unit.Value;

            var newStatus = MerchDeliveryStatus.Done;

            if (await _stockService.IsReadyToGiveOut(merchDelivery, token))
            {
                var isReserved = await _stockService.TryGiveOutItems(merchDelivery, token);
                if (isReserved)
                {
                    await SendMessageToBroker(employee, merchDelivery, token);
                }
                else
                {
                    newStatus = request.IsManual
                        ? MerchDeliveryStatus.EmployeeCame
                        : MerchDeliveryStatus.Notify;
                }
            }
            else
            {
                if (request.IsManual)
                    newStatus = MerchDeliveryStatus.EmployeeCame;
                else
                {
                    // TODO Здесь будет отправка сообщения HR, что закончился мерч с таким-то SKU (для автоматической выдачи)
                    // TODO Но как это сделать? Не нашёл подходящего в описании.
                    newStatus = MerchDeliveryStatus.Notify;
                }
            }
            
            merchDelivery.SetStatus(newStatus);
            await _merchService.UpdateAsync(merchDelivery, token);
            return Unit.Value;
        }
        
        // TODO Нужен сервис для работы с Kafka. Метод ниже используется в двух местах.
        private async Task SendMessageToBroker(Employee employee, 
            MerchDelivery delivery,
            CancellationToken token)
        {
            var topic = _kafka
                .Configuration
                .EmployeeNotificationEventTopic;
                
            var key = delivery
                .Id
                .ToString();
                
            var notificationEvent = new NotificationEvent
            {
                EmployeeEmail = employee.EmailAddress?.Value ?? string.Empty,
                EmployeeName = employee.Name.Value,
                EventType = EmployeeEventType.MerchDelivery,
                Payload = new
                {
                    MerchType = delivery
                        .MerchPackType
                        .Id
                }
            };
                
            await _kafka.ProduceAsync(topic, key, notificationEvent, token);
        }
    }
}