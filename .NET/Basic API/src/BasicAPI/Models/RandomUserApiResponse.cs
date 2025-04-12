namespace BasicAPI.Models
{
    public class RandomUserApiResponse
    {
        public required List<User> Results { get; set; }
        public required RandomUserApiResponseInfo Info { get; set; }
    }

    public class RandomUserApiResponseInfo
    {
        public required string Seed { get; set; }
        public required int Results { get; set; }
        public required int Page { get; set; }
        public required string Version { get; set; }
    }
}
