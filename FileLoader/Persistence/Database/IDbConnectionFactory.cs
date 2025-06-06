using System.Data;

namespace FileLoader.Persistence.Database
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(int affiliateId);
    }
}
