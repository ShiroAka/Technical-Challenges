using BasicAPI.Helpers;
using BasicAPI.Interfaces;
using BasicAPI.Models;

namespace BasicAPI.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly ILogger<DeviceService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private const string API_NAME = "Devices API";

        public DeviceService(ILogger<DeviceService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; //Saving it in case it is needed later

            /*
             * TODO - Add retry policy --> https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
             */
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(ConfigHelper.GetApiRequestsMaxTimeout(configuration));

            //_httpClient.BaseAddress = new Uri(ConfigHelper.GetDevicesApiBaseURL(configuration)); --> This adds a slash at the end of the URL, which does not work with this specific API when trying to fetch all the devices
        }

        public async Task<OperationResult<List<Device>>> GetAll(CancellationToken cancellationToken)
        {
            string methodName = $"DeviceService.GetAll";
            string endpoint = ConfigHelper.GetDevicesApiBaseURL(_configuration);
            string httpRequestFailMessage = "Error while getting devices";            
            var httpRequest = _httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            return await ApiHelper.ApiRequest<List<Device>>(_logger, cancellationToken, httpRequest, API_NAME, methodName, httpRequestFailMessage);
        }

        public async Task<OperationResult<Device>> GetByID(int id, CancellationToken cancellationToken)
        {
            string methodName = $"DeviceService.GetByID";
            string baseURL = ConfigHelper.GetDevicesApiBaseURL(_configuration);
            string endpoint = $"{baseURL}/{id}";
            string httpRequestFailMessage = "Error while getting a device";
            var httpRequest = _httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            return await ApiHelper.ApiRequest<Device>(_logger, cancellationToken, httpRequest, API_NAME, methodName, httpRequestFailMessage);            
        }

        public async Task<OperationResult<Device>> CreateDevice(Device device, CancellationToken cancellationToken)
        {
            string methodName = $"DeviceService.CreateDevice";
            string endpoint = ConfigHelper.GetDevicesApiBaseURL(_configuration);
            string httpRequestFailMessage = "Error while creating a device";
            var httpRequest = _httpClient.PostAsJsonAsync(endpoint, device, cancellationToken);

            return await ApiHelper.ApiRequest<Device>(_logger, cancellationToken, httpRequest, API_NAME, methodName, httpRequestFailMessage);
        }

        public async Task<OperationResult<Device>> UpdateDevice(int id, Device device, CancellationToken cancellationToken)
        {
            string methodName = $"DeviceService.UpdateDevice";
            string baseURL = ConfigHelper.GetDevicesApiBaseURL(_configuration);
            string endpoint = $"{baseURL}/{id}";
            string httpRequestFailMessage = "Error while updating a device";
            var httpRequest = _httpClient.PutAsJsonAsync(endpoint, device, cancellationToken);

            return await ApiHelper.ApiRequest<Device>(_logger, cancellationToken, httpRequest, API_NAME, methodName, httpRequestFailMessage);
        }
    }
}
