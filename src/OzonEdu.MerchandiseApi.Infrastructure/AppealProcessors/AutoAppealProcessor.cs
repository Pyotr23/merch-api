using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;

namespace OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors
{
    public class AutoAppealProcessor : IAppealProcessor
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<AutoAppealProcessor> _logger;
        private readonly IMediator _mediator;

        public MerchDeliveryStatus MerchDeliveryStatus { get; } = MerchDeliveryStatus.Notify;

        public AutoAppealProcessor(IEmployeeService employeeService,
            ILogger<AutoAppealProcessor> logger,
            IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
            _employeeService = employeeService;
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
                var command = new GiveOutMerchCommand
                {
                    EmployeeId = employee.Id,
                    MerchPackTypeId = delivery.MerchPackType.Id,
                    IsManual = false
                };
                await _mediator.Send(command, token);
            }
        }
    }
}