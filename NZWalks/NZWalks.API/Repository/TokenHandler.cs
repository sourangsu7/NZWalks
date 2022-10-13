using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NZWalks.API.Repository
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration configuration;

        public TokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(User user)
        {
            var claims = new List<Claim>();
            claims.AddRange(new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, user.LoginName),
                new Claim(ClaimTypes.Name , user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            });
            user.Roles.ForEach((role)=> claims.Add(new Claim(ClaimTypes.Role, role)));

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var securityKey = configuration["Jwt:Key"];

            var signingCredential = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256) ;

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: signingCredential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
