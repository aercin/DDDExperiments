using Application.Common.Exceptions;
using Application.Common.Models;
using Domain.Aggregates.Customer.Events;
using Domain.Common;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customer.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<Result>
    {
        public virtual Guid Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public DeleteCustomerCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = this._uow.Customers.Find(x => x.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Aggregates.Customer.Customer), request.Id);
            }

            entity.AddDomainEvent(new CustomerRemovedEvent(entity));

            this._uow.Customers.Remove(entity);

            await this._uow.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
