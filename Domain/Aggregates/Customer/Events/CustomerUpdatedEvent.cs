using Domain.Common;

namespace Domain.Aggregates.Customer.Events
{
    public class CustomerUpdatedEvent : DomainEvent
    {
        //Mock instance için mecburi empty constructor.
        public CustomerUpdatedEvent()
        {

        }

        public CustomerUpdatedEvent(Customer item) : base(item.Id)
        {
            this.Item = item;
        }
        public Customer Item { get; }
    }
}
