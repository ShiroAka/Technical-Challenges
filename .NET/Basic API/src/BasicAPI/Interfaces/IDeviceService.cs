using BasicAPI.Models;

namespace BasicAPI.Interfaces
{
    public interface IDeviceService
    {
        public Task<OperationResult<List<Device>>> GetAll(CancellationToken cancellationToken);
        public Task<OperationResult<Device>> GetByID(int id, CancellationToken cancellationToken);
        public Task<OperationResult<Device>> CreateDevice(Device device, CancellationToken cancellationToken);
        public Task<OperationResult<Device>> UpdateDevice(int id, Device device, CancellationToken cancellationToken);
    }
}
