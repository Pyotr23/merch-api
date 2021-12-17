using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;

namespace OzonEdu.MerchandiseApi.Infrastructure.Trace.Services
{
    public interface ITraceable
    {
        ICustomTracer Tracer { get; }
    }
}