using Amazon;
using Amazon.Runtime;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace FileLoader.Helpers
{
    public static class ParameterStoreHelper
    {
        public static string GetParameterFromParameterStore(string parameterName)
        {
            try
            {
                var getParameterRequest = new GetParameterRequest()
                {
                    Name = parameterName,
                    WithDecryption = true
                };

                GetParameterResponse response;

                using (AmazonSimpleSystemsManagementClient _parameterStoreClient = GetSimpleSystemsManagementClient())
                {
                    response = _parameterStoreClient.GetParameterAsync(getParameterRequest).Result;
                }

                return response.Parameter.Value;
            }
            catch (Exception ex)
            {
                //LambdaLogger.Log($">>>Error in GetParameterFromParameterStore. {ex.Message}");
                throw;
            }
        }

        public static async Task<Dictionary<string, string>> GetParametersFromParameterStoreAsync(List<string> parameterNames)
        {
            Dictionary<string, string> dicParameters = new Dictionary<string, string>();

            try
            {
                using (AmazonSimpleSystemsManagementClient _parameterStoreClient = GetSimpleSystemsManagementClient())
                {
                    var getParametersRequest = new GetParametersRequest()
                    {
                        Names = parameterNames,
                        WithDecryption = true
                    };

                    GetParametersResponse response = _parameterStoreClient.GetParametersAsync(getParametersRequest).Result;

                    response.Parameters.ForEach(p => dicParameters.Add(p.Name, p.Value));
                }

                return dicParameters;
            }
            catch (Exception ex)
            {
                //LambdaLogger.Log($">>>Error in GetParametersFromParameterStoreAsync. {ex.Message}");
                throw;
            }
        }

        private static AmazonSimpleSystemsManagementClient GetSimpleSystemsManagementClient()
        {
            try
            {
                var maxErrorRetry = ConfigHelper.AWS_MAX_ERROR_RETRY;

                var parameterStoreConfig = new AmazonSimpleSystemsManagementConfig()
                {
                    MaxErrorRetry = int.Parse(maxErrorRetry),
                    Timeout = TimeSpan.FromSeconds(60),
                    RetryMode = RequestRetryMode.Adaptive,
                    RegionEndpoint = RegionEndpoint.GetBySystemName(ConfigHelper.AWS_REGION)
                };

                return new AmazonSimpleSystemsManagementClient(parameterStoreConfig);
            }
            catch (Exception ex)
            {
                //LambdaLogger.Log($">>>Error in GetSimpleSystemsManagementClient. {ex.Message}");
                throw;
            }
        }
    }
}
