using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Constants;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Maps;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Queries;
using OzonEdu.MerchandiseApi.Infrastructure.Trace;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;

#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const string ClassName = nameof(EmployeeRepository);
        
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly ICustomTracer _tracer;

        public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker,
            ICustomTracer tracer)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
            _tracer = tracer;
        }

        public async Task<Employee?> CreateAsync(Employee itemToCreate, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(CreateAsync));
            
            var parameters = new
            {
                Name = itemToCreate.Name.Value,
                ClothingSizeId = itemToCreate.ClothingSize?.Id,
                EmailAddress = itemToCreate.EmailAddress?.Value,
                ManagerEmailAddress = itemToCreate.ManagerEmailAddress?.Value
            };

            var commandDefinition = new CommandDefinition(
                EmployeeQuery.Insert,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            var id =  await connection.ExecuteScalarAsync<int>(commandDefinition);
            _changeTracker.Track(itemToCreate);
            return new Employee(id,
                itemToCreate.Name,
                itemToCreate.EmailAddress,
                itemToCreate.ManagerEmailAddress,
                itemToCreate.ClothingSize);
        }

        public async Task<Employee?> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(UpdateAsync));
            
            var parameters = new
            {
                EmployeeId = itemToUpdate.Id,
                Name = itemToUpdate.Name.Value,
                ClothingSizeId = itemToUpdate.ClothingSize?.Id,
                EmailAddress = itemToUpdate.EmailAddress?.Value,
                ManagerEmailAddress = itemToUpdate.ManagerEmailAddress?.Value
            };
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.Update,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }

        public async Task AddMerchDelivery(int employeeId, int merchDeliveryId, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(AddMerchDelivery));

            var parameters = new
            {
                EmployeeId = employeeId,
                MerchDeliveryId = merchDeliveryId
            };
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.AddMerchDelivery,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            await connection.ExecuteAsync(commandDefinition);
        }

        public async Task<Employee?> FindAsync(int id, CancellationToken token = default)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(FindAsync), ("filter", "id"));

            var commandDefinition = new CommandDefinition(
                EmployeeQuery.FilterById,
                new { Id = id },
                commandTimeout: Connection.Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection
                .QueryAsync<Models.Employee, Models.ClothingSize, Employee>(commandDefinition, 
                    EmployeeMap.CreateEmployee);

            return employees.FirstOrDefault();
        }

        public async Task<Employee?> FindAsync(string email, CancellationToken token = default)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(FindAsync), ("filter", "email"));
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.FilterByEmail,
                new { Email = email },
                commandTimeout: Connection.Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection
                .QueryAsync<Models.Employee, Models.ClothingSize, Employee>(commandDefinition, 
                    EmployeeMap.CreateEmployee);

            return employees.FirstOrDefault();
        }

        public async Task<IEnumerable<Employee>> GetByMerchDeliveryStatusAndSkuCollection(int statusId, 
            IEnumerable<long> skuIdCollection,
            CancellationToken token = default)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(GetByMerchDeliveryStatusAndSkuCollection));
            
            var parameters = new
            {
                StatusId = statusId,
                SkuIds = skuIdCollection.ToArray()
            };
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.FilterByMerchDeliveryStatusAndSkuCollection,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection
                .QueryAsync<Models.Employee, Models.ClothingSize, Employee>(commandDefinition, 
                    EmployeeMap.CreateEmployee);

            return employees;
        }
    }
}