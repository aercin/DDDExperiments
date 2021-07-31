using Domain.Common;
using MediatR;

namespace Application.Common.Models
{
    public class DomainEventNotification<T> : INotification where T : DomainEvent
    {
        public DomainEventNotification(T domainEvent)
        {
            this.DomainEvent = domainEvent;
        }

        public T DomainEvent { get; }
    }
}
