using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Domain.Models;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.Trace;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Services;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.DomainEvent
{
    public class EmployeeNotificationDomainEventHandler : INotificationHandler<EmployeeNotificationDomainEvent>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMerchService _merchService;
        private readonly IMediator _mediator;
        private readonly ICustomTracer _tracer;

        public EmployeeNotificationDomainEventHandler(IEmployeeService employeeService, 
            IMerchService merchService,
            IMediator mediator,
            ICustomTracer tracer)
        {
            _tracer = tracer;
            _employeeService = employeeService;
            _merchService = merchService;
            _mediator = mediator;
        }
        
        public async Task Handle(EmployeeNotificationDomainEvent notification, CancellationToken token)
        {
            using var span = _tracer.GetSpan(nameof(EmployeeNotificationDomainEventHandler), nameof(Handle));
            var notificationEvent = notification.NotificationEvent;
            var eventPayload = notificationEvent.Payload;
            if (eventPayload is not MerchDeliveryEventPayload merchDeliveryEventPayload)
                throw new Exception("Notification event payload isn't merch delivery");

            var merchType = merchDeliveryEventPayload.MerchType;
            if (!IsTypeForReaction(merchType))
                throw new Exception("Notification event without reaction");

            var employee = await _employeeService.FindAsync(notificationEvent.EmployeeEmail, token) 
                           ?? await _employeeService.CreateAsync(notificationEvent.EmployeeName, 
                               notificationEvent.EmployeeEmail, 
                               token);

            var merchDelivery = employee
                .MerchDeliveries
                .FirstOrDefault(d => d.MerchPackType.Id == (int)merchType);

            if (merchDelivery is null)
            {
                var merchPackType = Enumeration
                    .GetAll<MerchPackType>()
                    .First(t => t.Id == (int)merchType);
                
                merchDelivery = await _merchService.CreateMerchDeliveryAsync(merchPackType, 
                    employee.ClothingSize, 
                    token);
            }
            
            var command = new GiveOutMerchCommand
            {
                EmployeeId = employee.Id,
                MerchPackTypeId = merchDelivery.MerchPackType.Id,
                IsManual = false
            };
            
            await _mediator.Send(command, token);
        }

        private static bool IsTypeForReaction(MerchType merchType)
        {
            return merchType is MerchType.WelcomePack 
                or MerchType.ConferenceListenerPack 
                or MerchType.ConferenceSpeakerPack;
        }
    }
}