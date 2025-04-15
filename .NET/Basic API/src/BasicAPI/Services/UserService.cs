using BasicAPI.Helpers;
using BasicAPI.Interfaces;
using BasicAPI.Models;
using System.Text.Json;

namespace BasicAPI.Services
{
    public class UserService : IUserService
    {
        public readonly HttpClient _httpClient;
        public readonly ILogger<UserService> _logger;
        public readonly IConfiguration _configuration;

        public UserService(ILogger<UserService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; //Saving it in case it is needed later

            /*
             * TODO - Add retry policy --> https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
             */
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(ConfigHelper.GetApiRequestsMaxTimeout(configuration));
        }        

        public async Task<OperationResult<User>> GetRandomUser(CancellationToken cancellationToken)
        {
            string methodName = $"UserService.GetRandomUser";
            string endpoint = ConfigHelper.GetRandomUsersApiBaseURL(_configuration);
            string httpRequestFailMessage = "Error while getting a random user";
            string apiName = "Random Users API";
            var httpRequest = _httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            OperationResult<RandomUserApiResponse> response = await ApiHelper.ApiRequest<RandomUserApiResponse>(_logger, cancellationToken, httpRequest, apiName, methodName, httpRequestFailMessage);
            
            return new OperationResult<User>
            {
                IsSuccess = response.IsSuccess,
                StatusCode = response.StatusCode,
                ErrorMessage = response.ErrorMessage,
                Data = response.Data?.Results?.FirstOrDefault() //The API always returns only one user in a list
            };
        }

        public async Task<OperationResult<string>> CreateUser(User user, CancellationToken cancellationToken)
        {
            string methodName = $"UserService.CreateUser";
            string webHookApiNewIdEndpoint = ConfigHelper.GetWebHookApiNewIdURL(_configuration);            
            string httpRequestFailMessage = "Error while creating a new webHook ID";
            string apiName = "WebHook API";
            string customHeaderName = ConfigHelper.GetCustomWebHookApiHeaderName(_configuration);
            string customHeaderValue = ConfigHelper.GetCustomWebHookApiHeaderValue(_configuration);

            //var httpRequest = _httpClient.GetAsync(webHookApiNewIdEndpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken); //This does not let you add custom headers

            //NOTE --> I could add the custom header to the HttpClient, but I do not want to use the custom header for all the requests in this service

            //GetAsync with custom header - Start
            Task<HttpResponseMessage> httpRequest;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, webHookApiNewIdEndpoint))
            {
                requestMessage.Headers.Add(customHeaderName, customHeaderValue);
                httpRequest = _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            //GetAsync with custom header - End

            OperationResult<WebHookApiNewIdResponse> newWebHookIdResult = await ApiHelper.ApiRequest<WebHookApiNewIdResponse>(_logger, cancellationToken, httpRequest, apiName, methodName, httpRequestFailMessage);

            if(newWebHookIdResult.IsSuccess)
            {
                OperationResult<string> result = new OperationResult<string>();

                string webHookIdUrl = newWebHookIdResult.Data?.Routes?.WebHook;

                if(webHookIdUrl == null)
                {
                    result.IsSuccess = false;
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    result.ErrorMessage = "Error while creating a new webHook ID - the obtained ID is null";

                    return result;
                }

                httpRequestFailMessage = "Error while creating a new user";
                //httpRequest = _httpClient.PostAsJsonAsync(webHookIdUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken); //This does not let you add custom headers

                //PostAsJsonAsync with custom header - Start
                string userAsJson = JsonSerializer.Serialize(user, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                StringContent content = new StringContent(userAsJson, System.Text.Encoding.UTF8, "application/json");

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, webHookIdUrl))
                {
                    requestMessage.Headers.Add(customHeaderName, customHeaderValue);
                    requestMessage.Content = content;
                    httpRequest = _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                }
                //PostAsJsonAsync with custom header - End
                
                OperationResult<string> result2 = await ApiHelper.ApiRequest<string>(_logger, cancellationToken, httpRequest, apiName, methodName, httpRequestFailMessage, true);

                if(result2.IsSuccess)
                {
                    result2.Data = newWebHookIdResult.Data?.Routes?.Inspect?.Html;
                }

                return result2;
            }
            else
            {
                return new OperationResult<string>
                {
                    IsSuccess = false,
                    StatusCode = newWebHookIdResult.StatusCode,
                    ErrorMessage = newWebHookIdResult.ErrorMessage
                };
            }
        }
    }
}
