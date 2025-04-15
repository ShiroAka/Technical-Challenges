using BasicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BasicAPI.Interfaces;
using System.Text.Json;

namespace BasicAPI.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }
                
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"[DeviceController.GetAll] A request was made to get ALL the devices");

            OperationResult<List<Device>> result = await _deviceService.GetAll(cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogDebug($"[DeviceController.GetAll] The request was completed successfully");
                return Ok(result.Data);
            }
            else
            {
                _logger.LogDebug($"[DeviceController.GetAll] Request finished with errors - StatusCode: {result.StatusCode} - ErrorMessage: {result.ErrorMessage}");

                //If there are any exceptions, they are handled and logged in the service layer
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"[DeviceController.GetByID] A request was made to get a device with ID {id}");

            OperationResult<Device> result = await _deviceService.GetByID(id, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogDebug($"[DeviceController.GetByID] The request was completed successfully");
                return Ok(result.Data);
            }
            else
            {
                _logger.LogDebug($"[DeviceController.GetByID] Request finished with errors - StatusCode: {result.StatusCode} - ErrorMessage: {result.ErrorMessage}");
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDevice(Device device, CancellationToken cancellationToken)
        {
            string inputDataJson = JsonSerializer.Serialize(device, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _logger.LogDebug($"[DeviceController.CreateDevice] A request was made to add a new device - Data: {inputDataJson}");

            OperationResult<Device> result = await _deviceService.CreateDevice(device, cancellationToken);

            if (result.IsSuccess)
            { 
                _logger.LogDebug($"[DeviceController.CreateDevice] The request was completed successfully");
                return Ok(result.Data);
            }
            else
            {
                _logger.LogDebug($"[DeviceController.CreateDevice] Request finished with errors - StatusCode: {result.StatusCode} - ErrorMessage: {result.ErrorMessage}");
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, Device device, CancellationToken cancellationToken)
        {
            string inputDataJson = JsonSerializer.Serialize(device, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _logger.LogDebug($"[DeviceController.UpdateDevice] A request was made to update a device with ID {id} - Data: {inputDataJson}");

            OperationResult<Device> result = await _deviceService.UpdateDevice(id, device, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogDebug($"[DeviceController.UpdateDevice] The request was completed successfully");
                return Ok(result.Data);
            }
            else
            {
                _logger.LogDebug($"[DeviceController.UpdateDevice] Request finished with errors - StatusCode: {result.StatusCode} - ErrorMessage: {result.ErrorMessage}");
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}
