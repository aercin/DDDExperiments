using Domain.Common;

namespace Domain.Aggregates.Customer.Events
{
    public class CustomerRemovedEvent : DomainEvent
    {
        //Mock instance için mecburi empty constructor.
        public CustomerRemovedEvent()
        {

        }

        public CustomerRemovedEvent(Customer item) : base(item.Id)
        {
            this.Item = item;
        }
        public Customer Item { get; }
    }
}
