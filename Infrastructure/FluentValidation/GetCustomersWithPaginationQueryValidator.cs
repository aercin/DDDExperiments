using Application.Customer.Queries.GetCustomersWithPagination;
using FluentValidation;

namespace Infrastructure.FluentValidation
{
    public class GetCustomersWithPaginationQueryValidator : AbstractValidator<GetCustomersWithPaginationQuery>
    {
        public GetCustomersWithPaginationQueryValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equel to 1.");
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equak to 1.");
        }
    }
}
