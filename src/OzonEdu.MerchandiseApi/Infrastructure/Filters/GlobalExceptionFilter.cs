using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OzonEdu.MerchandiseApi.Domain.Services.Exceptions;

namespace OzonEdu.MerchandiseApi.Infrastructure.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var resultObject = new
            {
                ExceptionMessage = context.Exception.Message
            };

            var statusCode = StatusCodes.Status500InternalServerError;

            var exception = context.Exception;
            if (exception is NotExistsException)
                statusCode = StatusCodes.Status400BadRequest;

            var jsonResult = new JsonResult(resultObject)
            {
                StatusCode = statusCode
            };
            
            context.Result = jsonResult;
        }
    }
}