using System;
using FlowersPlanner.Localization;
using FluentValidation;
namespace FlowersPlanner.Presentation.Registration
{
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(name => name).NotNull().WithMessage(AppResources.EmptyEntryError)
                                 .NotEmpty().WithMessage(AppResources.EmptyEntryError)
                                 .MinimumLength(6).WithMessage(AppResources.PasswordRequirementsError)
                                 .Matches(@"^(?=.*\d).{6,}$").WithMessage(AppResources.PasswordRequirementsError);
        }

        protected override void EnsureInstanceNotNull(object instanceToValidate)
        {

        }
    }
}
