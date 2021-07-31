using Domain.Common;
using System;

namespace Domain.Aggregates.StoredEvent
{
    public class StoredEvent : DomainEvent, IAggregateRoot
    {
        // EF Constructor
        protected StoredEvent() { }

        public StoredEvent(DomainEvent theEvent, string data)
        {
            Id = Guid.NewGuid();
            AggregateId = theEvent.AggregateId;
            MessageType = theEvent.MessageType;
            Timestamp = DateTime.Now;
            Data = data;
        }

        public Guid Id { get; private set; }
        public string Data { get; private set; }
    }
}
