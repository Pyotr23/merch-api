using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.MessageBroker;

namespace OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors
{
    public class ManualAppealProcessor : IAppealProcessor
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ManualAppealProcessor> _logger;
        private readonly IStockService _stockService;
        private readonly KafkaManager _kafka;

        public MerchDeliveryStatus MerchDeliveryStatus { get; } = 
            MerchDeliveryStatus.EmployeeCame;

        public ManualAppealProcessor(IEmployeeService employeeService, 
            ILogger<ManualAppealProcessor> logger,
            IStockService stockService,
            KafkaManager kafka)
        {
            _kafka = kafka;
            _stockService = stockService;
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task Do(IEnumerable<long> skuCollection, CancellationToken token)
        {
            var employeesForNotify = await _employeeService
                .GetAsync(MerchDeliveryStatus, skuCollection, token);

            foreach (var employee in employeesForNotify)
            {
                await TryCompleteDeliveries(employee, token);
            }
        }

        public async Task TryCompleteDeliveries(Employee employee, CancellationToken token)
        {
            foreach (var delivery in employee.MerchDeliveries)
            {
                if (employee.EmailAddress is null)
                {
                    _logger.LogWarning("The email of employee {name} is not specified", 
                        employee.Name.Value);
                    continue;
                }

                if (!await _stockService.IsReadyToGiveOut(delivery, token))
                    continue;

                await SendMessageToBroker(employee, delivery, token);
            }
        }

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