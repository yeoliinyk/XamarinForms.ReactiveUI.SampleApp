using System;
using FlowersPlanner.Localization;
using FluentValidation;
namespace FlowersPlanner.Presentation.Registration
{
    public class ConfirmPasswordValidator : AbstractValidator<ConfirmPasswordValidatorArgs>
    {
        public ConfirmPasswordValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(args => args.ConfirmPassword)
                .NotNull().WithMessage(AppResources.EmptyEntryError)                                          
                .NotEmpty().WithMessage(AppResources.EmptyEntryError)
                .Equal(args => args.Password).WithMessage(AppResources.PasswordsDoNotMatch);
        }
    }

    public class ConfirmPasswordValidatorArgs
    {
        public ConfirmPasswordValidatorArgs(string password, string confirmPassword)
        {
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        public string Password { get; }
        public string ConfirmPassword { get; }
    }
}
