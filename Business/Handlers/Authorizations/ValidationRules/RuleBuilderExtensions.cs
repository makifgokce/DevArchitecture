using Business.Constants;
using FluentValidation;

namespace Business.Handlers.Authorizations.ValidationRules
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 8)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage(Messages.PasswordEmpty)
                .MinimumLength(minimumLength).WithMessage(Messages.PasswordLength)
                .Matches("[A-Z]").WithMessage(Messages.PasswordUppercaseLetter)
                .Matches("[a-z]").WithMessage(Messages.PasswordLowercaseLetter)
                .Matches("[0-9]").WithMessage(Messages.PasswordDigit)
                .Matches("[^a-zA-Z0-9]").WithMessage(Messages.PasswordSpecialCharacter);
            return options;
        }
        public static IRuleBuilder<T, string> Account<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 3, int maximumLength = 30)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage(Messages.AccountEmpty)
                .MinimumLength(minimumLength).WithMessage(Messages.AccountMinLength)
                .MaximumLength(maximumLength).WithMessage(Messages.AccountMaxLength)
                .Matches(@"^[\w](?!.*?\.{2})[\w.]{1,18}[\w]$").WithMessage(Messages.AccountPattern);
            return options;
        }
        public static IRuleBuilder<T, string> Name<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 2, int maximumLength = 50)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage(Messages.NotEmpty)
                .NotNull().WithMessage(Messages.NotEmpty)
                .MinimumLength(minimumLength).WithMessage(Messages.NameMinLength)
                .MaximumLength(maximumLength).WithMessage(Messages.NameMaxLength)
                .Matches(@"^(?=.{2,50}$)[A-Za-zÇçÖöÜüİıŞş]+(?:\s[A-Za-zÇçÖöÜüİıŞşĞğ]+)*$").WithMessage(Messages.NameNotValid);
            return options;
        }
        public static IRuleBuilder<T, string> CitizenId<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder.MinimumLength(11).MaximumLength(11).WithMessage(Messages.WrongCitizenId);

            return options;
        }
    }
}