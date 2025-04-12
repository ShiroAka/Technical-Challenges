using BasicAPI.Models;

namespace BasicAPI.Helpers
{     
    public static class ConfigHelper
    {
        public static string GetDevicesApiBaseURL(IConfiguration config)
        {
            return config.GetValue<string>(ConfigSections.DevicesApiBaseURL);
        }

        public static string GetRandomUsersApiBaseURL(IConfiguration config)
        {
            return config.GetValue<string>(ConfigSections.RandomUsersApiBaseURL);
        }

        public static string GetWebHookApiNewIdURL(IConfiguration config)
        {
            return config.GetValue<string>(ConfigSections.WebHookApiNewIdURL);
        }

        public static string GetWebHookApiGetObjectURL(IConfiguration config)
        {
            return config.GetValue<string>(ConfigSections.WebHookApiGetObjectURL);
        }

        public static string GetWebHookApiCreateObjectURL(IConfiguration config)
        {
            return config.GetValue<string>(ConfigSections.WebHookApiCreateObjectURL);
        }

        public static string GetCustomWebHookApiHeaderName(IConfiguration config)
        {
            return config.GetValue<string>(ConfigSections.WebHookApiCustomHeaderName);
        }

        public static string GetCustomWebHookApiHeaderValue(IConfiguration config)
        {
            return config.GetValue<string>(ConfigSections.WebHookApiCustomHeaderValue);
        }

        public static int GetApiRequestsMaxTimeout(IConfiguration config)
        {
            return config.GetValue<int>(ConfigSections.ApiRequestsMaxTimeout);
        }

        public static int GetApiRequestsMaxRetries(IConfiguration config)
        {
            return config.GetValue<int>(ConfigSections.ApiRequestsMaxRetries);
        }
    }
}
