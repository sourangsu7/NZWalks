using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO.User;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandler _tokenHandler;

        public AuthController(IUserRepository userRepository,ITokenHandler tokenHandler)
        {
            
            _userRepository = userRepository;
            _tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AuthenticateUser(LogInRequest logInRequest)
        {
           var authenticatedUser = await _userRepository.AuthenticateUserAsync(logInRequest);

            if(authenticatedUser != null)
            {
               var securityToken =  await _tokenHandler.CreateTokenAsync(authenticatedUser);

                return Ok(securityToken);
            }
            return BadRequest("Please enter a valid combination of user login and password");
        }
    }
}
