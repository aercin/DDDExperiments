using Application.Common.Models;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Result<Guid>>
    { 
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICustomerCreationPrerequisiteStrategy _customerCreationPrerequisiteStrategy;

        public CreateCustomerCommandHandler(IUnitOfWork uow, ICustomerCreationPrerequisiteStrategy customerCreationPrerequisiteStrategy)
        {
            this._uow = uow;
            this._customerCreationPrerequisiteStrategy = customerCreationPrerequisiteStrategy;
        }

        public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = Domain.Aggregates.Customer.Customer.Create(request.Name, request.Email, request.BirthDate, this._customerCreationPrerequisiteStrategy);

            this._uow.Customers.Add(entity);

            await this._uow.CompleteAsync(cancellationToken);

            var result = Result<Guid>.Success(entity.Id);

            return result;
        }
    }
}
