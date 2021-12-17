using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Constants;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Maps;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Queries;
using OzonEdu.MerchandiseApi.Infrastructure.Trace;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;
using MerchDelivery = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDelivery;
using MerchDeliveryStatus = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDeliveryStatus;
using MerchPackType = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchPackType;
using MerchType = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchType;

#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation
{
    public class MerchDeliveryRepository : IMerchDeliveryRepository
    {
        private const string ClassName = nameof(MerchDeliveryRepository);
        
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly ICustomTracer _tracer;

        public MerchDeliveryRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker,
            ICustomTracer tracer)
        {
            _tracer = tracer;
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<MerchDelivery?> CreateAsync(MerchDelivery itemToCreate, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(CreateAsync));
            
            var parameters = new
            {
                MerchDeliveryStatusId = itemToCreate.Status.Id,
                MerchPackTypeId = itemToCreate.MerchPackType.Id,
                StatusChangeDate = itemToCreate.StatusChangeDate?.Value 
                    ?? DateTime.Now,
                SkuIds = itemToCreate
                    .SkuCollection
                    .Select(s => s.Value)
                    .ToArray()
            };

            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.Insert,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            var id = await connection.ExecuteScalarAsync<int>(commandDefinition);
            _changeTracker.Track(itemToCreate);
            
            return new MerchDelivery(id,
                itemToCreate.MerchPackType,
                itemToCreate.SkuCollection,
                itemToCreate.Status,
                itemToCreate.StatusChangeDate);
        }

        public async Task<MerchDelivery?> UpdateAsync(MerchDelivery itemToUpdate, CancellationToken cancellationToken = default)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(UpdateAsync));
            
            var parameters = new
            {
                MerchDeliveryId = itemToUpdate.Id,
                MerchDeliveryStatusId = itemToUpdate.Status.Id,
                MerchPackTypeId = itemToUpdate.MerchPackType.Id,
                StatusChangeDate = itemToUpdate.StatusChangeDate?.Value
                    ?? DateTime.Now,
                SkuIds = itemToUpdate
                    .SkuCollection
                    .Select(s => s.Value)
                    .ToArray()
            };
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.Update,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }

        public async Task<IEnumerable<MerchDelivery>?> GetAsync(int employeeId, 
            CancellationToken token = default)
        {
            using var span = _tracer.GetSpan(ClassName, 
                nameof(GetAsync),
                ("filter", "employee_id"));

            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.FilterByEmployeeId,
                new { EmployeeId = employeeId },
                commandTimeout: Connection.Timeout,
                cancellationToken: token);

            // TODO Нужен один запрос вместо двух
            var merchTypes = await GetMerchTypes(token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            
            return await connection
                .QueryAsync<Models.MerchDelivery, Models.MerchPackType, Models.MerchDeliveryStatus, MerchDelivery>(
                    commandDefinition,
                (delivery, type, status) => 
                        MerchDeliveryMap.CreateMerchDelivery(delivery, type, status, merchTypes));
        }

        public async Task<MerchPackType?> FindMerchPackType(int typeId, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(FindMerchPackType));
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.FindMerchPackType,
                new { MerchPackTypeId = typeId },
                commandTimeout: Connection.Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            
            var dbResult = await connection
                .QueryFirstOrDefaultAsync<Models.MerchPackType>(commandDefinition);
            
            return dbResult is null
                ? null
                : new MerchPackType(dbResult.Id, dbResult.Name);
        }

        public async Task<MerchDeliveryStatus?> FindStatus(int employeeId, int merchPackTypeId, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(FindStatus));
            
            var parameters = new
            {
                EmployeeId = employeeId,
                MerchPackTypeId = merchPackTypeId
            };
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.FindMerchDeliveryStatusByEmployeeIdAndMerchPackTypeId,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            
            var dbResult = await connection
                .QueryFirstOrDefaultAsync<Models.MerchDeliveryStatus>(commandDefinition);
            
            return dbResult is null
                ? null
                : new MerchDeliveryStatus(dbResult.Id.Value, dbResult.Name);
        }

        public async Task<IEnumerable<MerchDelivery>> GetAsync(int employeeId,
            int statusId,
            IEnumerable<long> skuCollection,
            CancellationToken token)
        {
            using var span = _tracer.GetSpan(ClassName, 
                nameof(GetAsync),
                ("filter", "sku_collection"));

            var parameters = new
            {
                EmployeeId = employeeId,
                MerchDeliveryStatusId = statusId,
                SkuCollection = skuCollection.ToArray()
            };
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.FilterByEmployeeIdAndStatusIdAndSkuCollection,
                parameters,
                commandTimeout: Connection.Timeout,
                cancellationToken: token);

            // TODO Нужен один запрос вместо двух
            var merchTypes = await GetMerchTypes(token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            
            return await connection
                .QueryAsync<Models.MerchDelivery, Models.MerchPackType, Models.MerchDeliveryStatus, MerchDelivery>(
                    commandDefinition,
                    (delivery, type, status) => 
                        MerchDeliveryMap.CreateMerchDelivery(delivery, type, status, merchTypes));

        }

        private async Task<Dictionary<int, MerchType>> GetMerchTypes(CancellationToken token)
        {
            using var span = _tracer.GetSpan(ClassName, nameof(GetMerchTypes));
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.GetMerchTypes,
                commandTimeout: Connection.Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            var types = await connection.QueryAsync<Models.MerchType>(commandDefinition);
            return types
                .Select(t => new MerchType(t.Id.Value, t.Name))
                .ToDictionary(k => k.Id, v => v);
        }
    }
}