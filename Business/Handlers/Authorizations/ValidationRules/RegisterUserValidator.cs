using Business.Constants;
using Business.Handlers.Authorizations.Commands;
using FluentValidation;

namespace Business.Handlers.Authorizations.ValidationRules
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(p => p.Account).Account();
            RuleFor(p => p.CitizenId.ToString()).CitizenId();
            RuleFor(p => p.Password).Password();
            RuleFor(p => p.Email).EmailAddress().WithMessage(Messages.EmailNotValid);
            RuleFor(p => p.Name).Name();
            RuleFor(p => p.Surname).Name();
        }
    }
}