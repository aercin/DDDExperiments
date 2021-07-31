using Application.Customer.Commands.DeleteCustomer;
using FluentValidation;
using System;

namespace Infrastructure.FluentValidation
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(c => c.Id)
             .NotEqual(Guid.Empty); 
        }
    }
}
