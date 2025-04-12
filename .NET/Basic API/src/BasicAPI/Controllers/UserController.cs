using Microsoft.AspNetCore.Mvc;
using BasicAPI.Interfaces;
using BasicAPI.Models;
using BasicAPI.Services;
using System.Text.Json;

namespace BasicAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("users/random")]        
        public async Task<IActionResult> GetRandomUser(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"[UserController.GetRandomUser] A request was made to get a random user");

            OperationResult<User> result = await _userService.GetRandomUser(cancellationToken);

            if (result.IsSuccess)
            {
                string responseJson = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                _logger.LogDebug($"[UserController.GetRandomUser] The request was completed successfully - Data: {responseJson}");

                return Ok(responseJson);
            }
            else
            {
                _logger.LogDebug($"[UserController.GetRandomUser] Request finished with errors - StatusCode: {result.StatusCode} - ErrorMessage: {result.ErrorMessage}");

                //If there are any exceptions, they are handled and logged in the service layer
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(User user, CancellationToken cancellationToken)
        {
            string inputDataJson = JsonSerializer.Serialize(user, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _logger.LogDebug($"[UserController.CreateUser] A request was made to add a new user - Data: {inputDataJson}");

            OperationResult<string> result = await _userService.CreateUser(user, cancellationToken);

            if (result.IsSuccess)
            {
                string responseJson = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                _logger.LogDebug($"[UserController.CreateUser] The request was completed successfully - Data: {responseJson}");

                return Ok(responseJson);
            }
            else
            {
                _logger.LogDebug($"[UserController.CreateUser] Request finished with errors - StatusCode: {result.StatusCode} - ErrorMessage: {result.ErrorMessage}");
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}
