namespace BasicAPI.Models
{
    public static class ConfigSections
    {
        //Endpoints
        public static string DevicesApiBaseURL = "Endpoints:Devices:BaseURL";
        public static string RandomUsersApiBaseURL = "Endpoints:RandomUsers:BaseURL";
        public static string WebHookApiNewIdURL = "Endpoints:WebHook:NewId";
        public static string WebHookApiGetObjectURL = "Endpoints:WebHook:GetObject";
        public static string WebHookApiCreateObjectURL = "Endpoints:WebHook:CreateObject";

        //Custom WebHook API Headers
        public static string WebHookApiCustomHeaderName = "CustomWebHookApiHeader:Name";
        public static string WebHookApiCustomHeaderValue = "CustomWebHookApiHeader:Value";

        //Api Requests Handling
        public static string ApiRequestsMaxTimeout = "ApiRequests:MaxTimeoutSeconds";
        public static string ApiRequestsMaxRetries = "ApiRequests:MaxRetries";
    }
}
