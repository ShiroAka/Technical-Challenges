namespace BasicAPI.Models
{
    public class WebHookApiNewIdResponse
    {
        public required WebHookApiRoutes Routes { get; set; }
    }

    public class WebHookApiRoutes
    {
        public required WebHookApiInspectRoutes Inspect { get; set; }
        public required string WebHook { get; set; }
    }

    public class WebHookApiInspectRoutes
    {
        public required string Api { get; set; }
        public required string Html { get; set; }
    }
}
