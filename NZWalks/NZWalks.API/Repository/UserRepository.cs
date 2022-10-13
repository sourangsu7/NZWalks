using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.User;
using NZWalks.API.Repository.Validator;

namespace NZWalks.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>();
        public UserRepository()
        {
            _users.Add(new User()
            {
                FirstName = "Christopher",
                LastName = "NKunku",
                LoginName = "c.nkunku",
                Password = "leipzig-2018",
                Roles = new List<string>() { "reader" },
                Email = "chris.nkunku@nzwalk.com"
            });
            _users.Add(new User()
            {
                FirstName = "Justin",
                LastName = "Fofana",
                LoginName = "j.fofana",
                Password = "Leicester-2020",
                Roles = new List<string>() { "reader","writer" },
                Email = "j.fofana@nzwalk.com"
            });
        }
        public async Task<User> AuthenticateUserAsync(LogInRequest logInRequest)
        {
            var loginValidator = new LoginRequestValidator().Validate(logInRequest);
            if(loginValidator.IsValid)
            {
               var registeredUser = _users.Find(u => u.LoginName.Equals(logInRequest.LoginName,StringComparison.InvariantCultureIgnoreCase) && u.Password == logInRequest.Password);

                return registeredUser;
            }
            return null;
        }
    }
}
