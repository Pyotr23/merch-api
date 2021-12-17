using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Npgsql;
using OzonEdu.MerchandiseApi.Infrastructure.Configuration;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure
{
    public class NpgsqlConnectionFactory : IDbConnectionFactory<NpgsqlConnection>
    {
        private readonly DatabaseConnectionOptions _options;
        private  NpgsqlConnection? _connection;
        public NpgsqlConnectionFactory(IOptions<DatabaseConnectionOptions> options)
        {
            _options = options.Value;
        }


        public async Task<NpgsqlConnection> CreateConnection(CancellationToken token)
        {
            if (_connection is not null)
                return _connection;

            _connection = new NpgsqlConnection(_options.ConnectionString);
            await _connection.OpenAsync(token);
            _connection.StateChange += (_, e) =>
            {
                if (e.CurrentState == ConnectionState.Closed)
                    _connection = null;
            };
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}