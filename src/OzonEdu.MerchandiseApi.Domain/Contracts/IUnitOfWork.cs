using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseApi.Domain.Contracts
{
    public interface IUnitOfWork
    {
        ValueTask StartTransaction(CancellationToken token);
        
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}