using FluentValidation;
using NZWalks.API.Models.DTO.User;

namespace NZWalks.API.Repository.Validator
{
    public class LoginRequestValidator:AbstractValidator<LogInRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(p => p.LoginName).NotEmpty().WithErrorCode("Please enter login");
            RuleFor(p => p.Password).NotEmpty().WithErrorCode("Please enter password");
        }
    }
}
