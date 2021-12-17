using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseApi.Domain.Contracts
{
    /// <summary>
    /// Базовый интерфейс репозитория
    /// </summary>
    /// <typeparam name="TAggregationRoot">Объект сущности для управления</typeparam>
    public interface IRepository<TAggregationRoot> where TAggregationRoot : class
    {
        /// <summary>
        /// Создать новую сущность
        /// </summary>
        /// <param name="itemToCreate">Объект для создания</param>
        /// <param name="token">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Созданная сущность</returns>
        Task<TAggregationRoot?> CreateAsync(TAggregationRoot itemToCreate, CancellationToken token = default);

        /// <summary>
        /// Обновить существующую сущность
        /// </summary>
        /// <param name="itemToUpdate">Объект для создания</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Обновленная сущность сущность</returns>
        Task<TAggregationRoot?> UpdateAsync(TAggregationRoot itemToUpdate, CancellationToken cancellationToken = default);
    }
}