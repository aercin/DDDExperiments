using Domain.Common;

namespace Domain.Aggregates.Customer.Events
{
    public class CustomerRegisteredEvent : DomainEvent
    {
        //Mock instance için mecburi empty constructor.
        public CustomerRegisteredEvent()
        { 
        }

        public CustomerRegisteredEvent(Customer item) : base(item.Id)
        {
            this.Item = item;
        }
        public Customer Item { get; }
    }
}
