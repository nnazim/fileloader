using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;
using Amazon;

namespace FileLoader.Helpers
{
    public static class SnsHelper
    {
        public static async Task<bool> PushToSNS(string serializedMessage, Dictionary<string, MessageAttributeValue> attributes = null)
        {
            try
            {
                var maxErrorRetry = ConfigHelper.AWS_MAX_ERROR_RETRY;
                var topicARN = ConfigHelper.SNS_FILE_LOADER_ARN;

                var snsConfig = new AmazonSimpleNotificationServiceConfig()
                {
                    MaxErrorRetry = int.Parse(maxErrorRetry),
                    Timeout = TimeSpan.FromSeconds(10),
                    RetryMode = Amazon.Runtime.RequestRetryMode.Adaptive,
                    RegionEndpoint = RegionEndpoint.GetBySystemName(ConfigHelper.AWS_REGION)
                };

                bool messageSent = false;

                using (var snsClient = new AmazonSimpleNotificationServiceClient(snsConfig))
                {
#if DEBUG
                    messageSent = true;
#else
                    messageSent = await PublishToTopicAsync(snsClient, topicARN, serializedMessage, attributes);
#endif
                }

                return messageSent;
            }
            catch
            {
                return false;
            }
        }

        private static async Task<bool> PublishToTopicAsync(IAmazonSimpleNotificationService snsClient, string topicArn, string messageText, Dictionary<string, MessageAttributeValue> attributes)
        {
            var request = new PublishRequest
            {
                TopicArn = topicArn,
                Message = messageText,
            };

            string LogAndEventType = "PublishToTopicAsync";

            if (attributes != null)
            {
                request.MessageAttributes = attributes;

                if (attributes.TryGetValue("EventType", out var objEventSource))
                {
                    LogAndEventType += $" {objEventSource.StringValue}";
                }
            }

            try
            {
                var response = await snsClient.PublishAsync(request);

                if (string.IsNullOrWhiteSpace(response.MessageId))
                {
                    //LambdaLogger.Log($">>>{LogAndEventType} Failed: {messageText}");
                    return false;
                }
                else
                {
                    //LambdaLogger.Log($">>>{LogAndEventType} MessageId: {response.MessageId} {messageText}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                //LambdaLogger.Log($">>>{LogAndEventType} Failed: {messageText}");
                //LambdaLogger.Log($">>>{LogAndEventType} Exception: {ex.Message}");
                return false;
            }
        }

    }
}
