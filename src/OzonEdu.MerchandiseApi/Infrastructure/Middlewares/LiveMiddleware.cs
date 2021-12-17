using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchandiseApi.Infrastructure.Middlewares
{
    public class LiveMiddleware
    {
        public LiveMiddleware(RequestDelegate next){}

        public async Task InvokeAsync(HttpContext context) 
            => await context.Response.WriteAsync("200 Ok");
    }
}