using System;

using Xamarin.Forms;
using FluentValidation;
using FlowersPlanner.Localization;

namespace FlowersPlanner.Presentation.Registration
{
    public class NameValidator : AbstractValidator<string>
    {
        public NameValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(name => name).NotNull().WithMessage(AppResources.EmptyEntryError)
                                 .NotEmpty().WithMessage(AppResources.EmptyEntryError)           
                                 .MinimumLength(2).WithMessage(AppResources.NameLengthError);
        }

        protected override void EnsureInstanceNotNull(object instanceToValidate)
        {
            
        }
    }
}

