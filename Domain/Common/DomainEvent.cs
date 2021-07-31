using System;

namespace Domain.Common
{
    public abstract class DomainEvent
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }
        public DateTime Timestamp { get; protected set; }

        public DomainEvent() { }
        public DomainEvent(Guid aggregateId)
        {
            this.AggregateId = aggregateId;
            this.Timestamp = DateTime.Now;
            this.MessageType = GetType().Name;
        } 
    }
}
