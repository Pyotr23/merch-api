using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Commands;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Queries.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Grpc;

namespace OzonEdu.MerchandiseApi.GrpcServices
{
    public class MerchandiseApiGrpcService : MerchandiseApiGrpc.MerchandiseApiGrpcBase
    {
        private readonly IMediator _mediator;

        public MerchandiseApiGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Empty> GiveOutMerch(GiveOutMerchRequest request, ServerCallContext context)
        {
            if (request.EmployeeId == default)
                throw new ArgumentException("You must specify the employee ID for issue a merch");
            
            if (request.MerchPackTypeId == default)
                throw new ArgumentException("You must specify the merch pack type ID");
            
            var command = new GiveOutMerchCommand
            {
                EmployeeId = request.EmployeeId,
                MerchPackTypeId = request.MerchPackTypeId,
                IsManual = true
            };
            
            await _mediator.Send(command, context.CancellationToken);
            return new Empty();
        }

        public override async Task<GetMerchDeliveryStatusResponse> GetMerchDeliveryStatus(
            GetMerchDeliveryStatusRequest request, ServerCallContext context)
        {
            if (request.EmployeeId == default)
                throw new ArgumentException("You must specify the employee ID for issue a merch");
            
            if (request.MerchPackTypeId == default)
                throw new ArgumentException("You must specify the merch pack type ID");
            
            var query = new GetMerchDeliveryStatusQuery
            {
                EmployeeId = request.EmployeeId,
                MerchPackTypeId = request.MerchPackTypeId
            };
            
            var result = await _mediator.Send(query, context.CancellationToken);

            if (result is null)
                throw new Exception("merch delivery status not found");
            
            return new GetMerchDeliveryStatusResponse
            {
                Name = result
            };
        }
    }
}