using System.Collections.Generic;

namespace Domain.Common
{
    public abstract class AggregateRootBaseEntity : IAggregateRoot
    {
        private List<DomainEvent> _domainEvents;
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();//Expression bodied Member C# 6

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
