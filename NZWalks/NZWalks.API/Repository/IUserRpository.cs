using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.User;

namespace NZWalks.API.Repository
{
    public interface IUserRepository
    {
        Task<User> AuthenticateUserAsync(LogInRequest logInRequest);
    }
}
