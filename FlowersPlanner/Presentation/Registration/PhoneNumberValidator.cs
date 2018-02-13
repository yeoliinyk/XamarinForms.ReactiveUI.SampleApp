using System;
using FlowersPlanner.Localization;
using FluentValidation;
namespace FlowersPlanner.Presentation.Registration
{
    public class PhoneNumberValidator : AbstractValidator<string>
    {
        public PhoneNumberValidator()
        {
            RuleFor(name => name).NotNull().WithMessage(AppResources.EmptyEntryError)                                 
                                 .Length(19).WithMessage(AppResources.PhoneNumberLengthError);
        }

        protected override void EnsureInstanceNotNull(object instanceToValidate)
        {
            
        }
    }
}
