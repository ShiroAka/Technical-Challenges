
namespace BasicAPI.Models
{
    public class Device
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public DeviceData? Data { get; set; }
    }

    public class DeviceData
    {
        public string? Color { get; set; }
        public string? Description { get; set; }
    }
}
