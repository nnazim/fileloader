using Amazon;
using Amazon.RDS.Util;
using MySql.Data.MySqlClient;

namespace FileLoader.Helpers
{
    public static class RdsHelper
    {
        public static string GetConnectionString(int affiliateId)
        {
            try
            {
                //Console.WriteLine($">>>Loading database details");

                ConfigHelper.LoadDbDetails(affiliateId);

                Console.WriteLine($">>>Database details loaded. HOST: {ConfigHelper.RDSPROXY_COREDB_HOST}, PORT: {ConfigHelper.RDSPROXY_COREDB_PORT}, USER: {ConfigHelper.CORE_DB_USER}");
//#if DEBUG
                //var host = "localhost";
                //var user = "admin";
                //var _token = @"ra0Y>2w,wjB\*@V>XR:2=_s'>SMpMN>$";
                //var port = Convert.ToUInt32(9003);
//#else
                var host = ConfigHelper.RDSPROXY_COREDB_HOST;
                var user = ConfigHelper.CORE_DB_USER;
                var port = Convert.ToUInt32(ConfigHelper.RDSPROXY_COREDB_PORT);
                var _token = RDSAuthTokenGenerator.GenerateAuthToken(RegionEndpoint.GetBySystemName(ConfigHelper.AWS_REGION), ConfigHelper.RDSPROXY_COREDB_HOST, Convert.ToInt32(ConfigHelper.RDSPROXY_COREDB_PORT), ConfigHelper.CORE_DB_USER);
//#endif
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
                {
                    Server = host,
                    Port = port,
                    UserID = user,
                    Password = _token,
                    Database = ConfigHelper.CORE_DB_NAME,
                    ConnectionTimeout = 300,
                    SslMode = MySqlSslMode.Required
                };

                return builder.ConnectionString;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
