using Application.Customer.Commands.UpdateCustomer;
using FluentValidation;
using System;

namespace Infrastructure.FluentValidation
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(c => c.Id)
             .NotEqual(Guid.Empty);

            RuleFor(c => c.Name)
                   .NotEmpty().WithMessage("Please ensure you have entered the Name")
                   .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");

            RuleFor(c => c.BirthDate)
              .NotEmpty()
              .Must(HaveMinimumAge)
              .WithMessage("The customer must have 18 years or more");

            RuleFor(c => c.Email)
               .NotEmpty()
               .EmailAddress();
        }

        protected static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-18);
        }
    }
}
