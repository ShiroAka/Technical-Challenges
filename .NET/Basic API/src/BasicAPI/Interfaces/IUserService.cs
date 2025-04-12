using Microsoft.AspNetCore.Mvc;
using BasicAPI.Models;

namespace BasicAPI.Interfaces
{
    public interface IUserService
    {
        public Task<OperationResult<User>> GetRandomUser(CancellationToken cancellationToken);
        public Task<OperationResult<string>> CreateUser(User user, CancellationToken cancellationToken);
    }
}
