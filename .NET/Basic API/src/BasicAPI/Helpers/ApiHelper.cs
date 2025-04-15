using BasicAPI.Models;
using System.Diagnostics;
using System.Text.Json;

namespace BasicAPI.Helpers
{
    public static class ApiHelper
    {
        public static async Task<OperationResult<T>> ApiRequest<T>(ILogger logger, CancellationToken cancellationToken, Task<HttpResponseMessage> httpRequest,
            string apiName, string callingMethod, string httpRequestFailMessage, bool isCreatingWebHookApiObject = false)
        {
            var result = new OperationResult<T>();

            try
            {
                Stopwatch time = Stopwatch.StartNew();
                HttpResponseMessage response = await httpRequest;
                time.Stop();

                logger.LogDebug($"[{callingMethod}] Finished call to the {apiName} - it took {time.Elapsed.TotalMilliseconds} ms");

                if (!response.IsSuccessStatusCode)
                {
                    result.IsSuccess = false;
                    result.StatusCode = (int)response.StatusCode;
                    result.ErrorMessage = response.ReasonPhrase;

                    logger.LogError($"[{callingMethod}] {httpRequestFailMessage} - Status Code: {response.StatusCode} - Message: \"{response.ReasonPhrase}\"");
                    return result;
                }

                using (Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken))
                {
                    //Without this the deserializer may throw errors
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    //When posting something to the WebHook API, the response is a string that says "OK", so I will not set the "result.Data" here
                    if (!isCreatingWebHookApiObject)
                    {                        
                        result.Data = await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);                        
                    }

                    result.IsSuccess = true;
                    result.StatusCode = (int)response.StatusCode;
                }
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                logger.LogDebug(ex, $"[{callingMethod}] The request to the {apiName} has timed out");
                result.StatusCode = StatusCodes.Status504GatewayTimeout;
                result.ErrorMessage = "Request timed out";
                result.IsSuccess = false;
            }
            catch (OperationCanceledException ex) //CancellationToken expired
            {
                logger.LogDebug(ex, $"[{callingMethod}] Request was cancelled by the client");
                result.StatusCode = StatusCodes.Status499ClientClosedRequest;
                result.ErrorMessage = "Request was cancelled by the client";
                result.IsSuccess = false;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
