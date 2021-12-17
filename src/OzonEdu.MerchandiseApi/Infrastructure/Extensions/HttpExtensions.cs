using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OzonEdu.MerchandiseApi.Constants;

namespace OzonEdu.MerchandiseApi.Infrastructure.Extensions
{
    internal static class HttpExtensions
    {
        internal static string AsString(this IHeaderDictionary headers)
        {
            const int headerNameSpace = -30;
            var headersString = headers
                .Select(h => $"\t{h.Key, headerNameSpace}{h.Value}");
            return string.Join("\n", headersString);    
        }

        internal static string GetRoute(this HttpRequest request)
        {
            var path = request.Path.Value;

            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var query = request.QueryString.Value 
                        ?? string.Empty;

            return path + query;
        }

        internal static async Task<string> BodyToString(this HttpRequest request)
        {
            var length = request.ContentLength;
            if (length is null or 0)
                return string.Empty;
            
            request.EnableBuffering();

            var buffer = new byte[length.Value];

            await request
                .Body
                .ReadAsync(buffer.AsMemory(0, buffer.Length));

            request.Body.Position = 0;
            
            return Encoding.UTF8.GetString(buffer);
        }
    }
}