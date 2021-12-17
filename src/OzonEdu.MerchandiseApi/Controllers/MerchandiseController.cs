using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseApi.Constants;
using OzonEdu.MerchandiseApi.HttpModels;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Queries;

namespace OzonEdu.MerchandiseApi.Controllers
{
    [ApiController]
    [Route(RouteConstant.Route)]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchandiseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        ///     Запросить мерч.
        /// </summary>
        /// <param name="request">  </param>
        /// <param name="token">  </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GiveOutMerch([FromBody] GiveOutMerchRequest request, 
            CancellationToken token)
        {
            if (request.EmployeeId is null)
                return BadRequest("You must specify the employee ID for issue a merch");
            
            if (request.MerchPackTypeId is null)
                return BadRequest("You must specify the merch pack type ID");
            
            var command = new GiveOutMerchCommand
            {
                EmployeeId = request.EmployeeId.Value,
                MerchPackTypeId = request.MerchPackTypeId.Value,
                IsManual = true
            };
            
            await _mediator.Send(command, token);
            return Ok();
        }
        
        [HttpGet("status")]
        public async Task<ActionResult<string?>> GetMerchDeliveryStatus(
            [FromQuery] GetMerchDeliveryStatusRequest request, 
            CancellationToken token)
        {
            if (request.EmployeeId is null)
                return BadRequest("You must specify the employee ID for issue a merch");
            
            if (request.MerchPackTypeId is null)
                return BadRequest("You must specify the merch pack type ID");
            
            var query = new GetMerchDeliveryStatusQuery
            {
                EmployeeId = request.EmployeeId.Value,
                MerchPackTypeId = request.MerchPackTypeId.Value
            };
            
            var statusName = await _mediator.Send(query, token);

            return Ok(statusName);
        }
    }
}