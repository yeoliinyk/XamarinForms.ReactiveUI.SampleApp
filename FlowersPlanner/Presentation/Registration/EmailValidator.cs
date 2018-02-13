using System;
using FlowersPlanner.Localization;
using FluentValidation;

namespace FlowersPlanner.Presentation.Registration
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(name => name).NotNull().WithMessage(AppResources.EmptyEntryError)
                                 .EmailAddress().WithMessage(AppResources.EmailFormatError);
        }

        protected override void EnsureInstanceNotNull(object instanceToValidate)
        {
            
        }
    }
}
