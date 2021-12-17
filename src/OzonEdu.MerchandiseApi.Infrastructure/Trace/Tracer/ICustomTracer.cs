using OpenTracing;

namespace OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer
{
    public interface ICustomTracer
    {
        IScope? GetSpan(string className, string method);
        IScope? GetSpan(string className, string method, (string key, string value) tag);
    }
}