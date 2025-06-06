using Newtonsoft.Json;

namespace FileLoader.Helpers
{
    public static class ConfigHelper
    {
        public static Dictionary<string, string> StoredParameters = new Dictionary<string, string>();
        public static string AWS_ACCESS_KEY_ID { get; set; } = "";
        public static string AWS_SECRET_ACCESS_KEY { get; set; } = "";
        public static string AWS_SESSION_TOKEN { get; set; } = "";
        public static string AWS_REGION { get; set; } = "eu-central-1";
        public static string COUNTRY_CODE_ISO3166 { get; set; } = "";
        public static string CORE_DB_USER { get; set; } = "";
        public static string CORE_DB_NAME { get; set; } = "";
        public static string RDSPROXY_COREDB_HOST_PSN { get; set; } = "";
        public static string RDSPROXY_COREDB_PORT_PSN { get; set; } = "";
        public static string SITEID_2_COUNTRY_CODE_ISO3166_PSN { get; set; } = "";
        public static string AWS_MAX_ERROR_RETRY { get; set; } = "";
        public static string RDSPROXY_COREDB_HOST { get; set; } = "";
        public static string RDSPROXY_COREDB_PORT { get; set; } = "";
        public static string SITEID_2_COUNTRY_CODE_ISO3166 { get; set; } = "";
        public static string SNS_FILE_LOADER_ARN { get; set; } = "";
        public static string SNS_FILE_LOADER_ARN_PSN { get; set; } = "";
        public static string RDSPROXY_COREDB_HOST_PREFIX_PSN { get; private set; } = "";
        public static string RDSPROXY_COREDB_PORT_PREFIX_PSN { get; private set; } = "";
        public static string PSN_AFFILIATE_ID_2_TIME_ZONE { get; set; } = "";
        public static string AFFILIATE_ID_2_TIME_ZONE { get; set; } = "";

        public static void LoadEnvironmentVariablesAndParameters()
        {
            try
            {
                AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? "";
                AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? "";
                AWS_SESSION_TOKEN = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN") ?? "";
                CORE_DB_USER = Environment.GetEnvironmentVariable("CORE_DB_USER") ?? "";
                CORE_DB_NAME = Environment.GetEnvironmentVariable("CORE_DB_NAME") ?? "coredb";
                RDSPROXY_COREDB_HOST_PSN = Environment.GetEnvironmentVariable("RDSPROXY_COREDB_HOST_PSN") ?? "";
                RDSPROXY_COREDB_PORT_PSN = Environment.GetEnvironmentVariable("RDSPROXY_COREDB_PORT_PSN") ?? "";
                AWS_MAX_ERROR_RETRY = Environment.GetEnvironmentVariable("AWS_MAX_ERROR_RETRY") ?? "2";
                SITEID_2_COUNTRY_CODE_ISO3166_PSN = Environment.GetEnvironmentVariable("SITEID_2_COUNTRY_CODE_ISO3166_PSN") ?? "";
                RDSPROXY_COREDB_HOST_PREFIX_PSN = Environment.GetEnvironmentVariable("RDSPROXY_COREDB_HOST_PREFIX_PSN") ?? "";
                RDSPROXY_COREDB_PORT_PREFIX_PSN = Environment.GetEnvironmentVariable("RDSPROXY_COREDB_PORT_PREFIX_PSN") ?? "";

                SNS_FILE_LOADER_ARN_PSN = Environment.GetEnvironmentVariable("SNS_FILE_LOADER_ARN_PSN") ?? "";
                PSN_AFFILIATE_ID_2_TIME_ZONE = Environment.GetEnvironmentVariable("PSN_AFFILIATE_ID_2_TIME_ZONE") ?? "";

                var parameterNames = new List<string>()
                {
                    SNS_FILE_LOADER_ARN_PSN,
                    SITEID_2_COUNTRY_CODE_ISO3166_PSN,
                    PSN_AFFILIATE_ID_2_TIME_ZONE
                };

                StoredParameters = ParameterStoreHelper.GetParametersFromParameterStoreAsync(parameterNames).Result;
                SNS_FILE_LOADER_ARN = StoredParameters[SNS_FILE_LOADER_ARN_PSN];
                SITEID_2_COUNTRY_CODE_ISO3166 = StoredParameters[SITEID_2_COUNTRY_CODE_ISO3166_PSN];
                AFFILIATE_ID_2_TIME_ZONE = StoredParameters[PSN_AFFILIATE_ID_2_TIME_ZONE];
            }
            catch (Exception) { throw; }
        }

        public static void LoadDbDetails(int affiliateId)
        {
            try
            {
                string strAffiliateId = Convert.ToString(affiliateId);

                COUNTRY_CODE_ISO3166 = GetCountryCode(strAffiliateId);

                RDSPROXY_COREDB_HOST_PSN = RDSPROXY_COREDB_HOST_PREFIX_PSN + $"{COUNTRY_CODE_ISO3166}";
                RDSPROXY_COREDB_PORT_PSN = RDSPROXY_COREDB_PORT_PREFIX_PSN + $"{COUNTRY_CODE_ISO3166}";

                var dbParameterNames = new List<string>()
                {
                    RDSPROXY_COREDB_HOST_PSN,
                    RDSPROXY_COREDB_PORT_PSN
                };

                StoredParameters = ParameterStoreHelper.GetParametersFromParameterStoreAsync(dbParameterNames).Result;

                RDSPROXY_COREDB_HOST = StoredParameters[RDSPROXY_COREDB_HOST_PSN];
                RDSPROXY_COREDB_PORT = StoredParameters[RDSPROXY_COREDB_PORT_PSN];
            }
            catch (Exception) { throw; }
        }

        public static string GetCountryCode(string affiliateId)
        {
            try
            {
                affiliateId = affiliateId.Substring(affiliateId.Length - 2, 2);
                Dictionary<string, string> SiteIdToCc = JsonConvert.DeserializeObject<Dictionary<string, string>>(SITEID_2_COUNTRY_CODE_ISO3166);
                return SiteIdToCc.TryGetValue(affiliateId, out string countryCode) ? countryCode : string.Empty;
            }
            catch (Exception) { throw; }
        }
    }
}
