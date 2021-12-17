using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Constants;
using OzonEdu.MerchandiseApi.Infrastructure.Extensions;

namespace OzonEdu.MerchandiseApi.Infrastructure.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (!(path is null || path.StartsWith(RouteConstant.Route)))
            {
                await _next(context);
                return;
            }
               
            
            var originalBody = context.Response.Body;
            await using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            try
            {
                await _next(context);
            }
            finally
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Response logged");
                stringBuilder.AppendLine($"Route: {context.Request.GetRoute()}");
                stringBuilder.AppendLine("Headers:");
                stringBuilder.AppendLine(context.Response.Headers.AsString());
                
                newBody.Seek(0, SeekOrigin.Begin);
                var bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                stringBuilder.Append($"Body: {bodyText}");
                
                _logger.LogInformation(stringBuilder.ToString());
                
                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
            }
        }
    }
}