using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchandiseApi.Infrastructure.Interceptors
{
    public class ExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception exception)
            {
                var httpContext = context.GetHttpContext();
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
             
                throw new RpcException(new Status(StatusCode.Internal, exception.ToString()));
            }
        }
    }
}