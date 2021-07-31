using Application.Common.Exceptions;
using Application.Common.Models;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customer.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<Result>
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime BirthDate { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result>
    {
        private readonly ICustomerModificationPrerequisiteStrategy _customerModificationPrerequisiteStrategy;
        private readonly IUnitOfWork _uow;

        public UpdateCustomerCommandHandler(IUnitOfWork uow, ICustomerModificationPrerequisiteStrategy customerModificationPrerequisiteStrategy)
        {
            this._uow = uow;
            this._customerModificationPrerequisiteStrategy = customerModificationPrerequisiteStrategy;
        }

        public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = this._uow.Customers.Find(x => x.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Aggregates.Customer.Customer), request.Id);
            }

            entity.Update(request.Name, request.Email, request.BirthDate, this._customerModificationPrerequisiteStrategy);

            await this._uow.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
