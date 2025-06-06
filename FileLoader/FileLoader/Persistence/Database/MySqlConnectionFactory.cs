using FileLoader.Helpers;
using MySql.Data.MySqlClient;
using System.Data;

namespace FileLoader.Persistence.Database
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        public async Task<IDbConnection> CreateConnectionAsync(int affiliateId)
        {
            var connectionString = RdsHelper.GetConnectionString(affiliateId);
            var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            //return new SerilogLoggingDbConnection(connection); // Add Serilog logging to track query being executed. Need to create adapter.
            return connection;
        }
    }
}
