using Domain.Aggregates.Customer.Events;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using System;

namespace Domain.Aggregates.Customer
{
    public class Customer : AggregateRootBaseEntity
    {
        // Empty constructor for EF
        protected Customer() { }

        private Customer(Guid id, string name, string email, DateTime birthDate)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.BirthDate = birthDate;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }

        public static Customer Create(string name, string email, DateTime birthDate, ICustomerCreationPrerequisiteStrategy preconditionOfCreation)
        {
            preconditionOfCreation.IsAllPrerequisitesSupplied(email, name, birthDate);

            var newCust = new Customer(Guid.NewGuid(), name, email, birthDate);

            newCust.AddDomainEvent(new CustomerRegisteredEvent(newCust));

            return newCust;
        }

        public void Update(string name, string email, DateTime birthDate, ICustomerModificationPrerequisiteStrategy preconditionOfModification)
        {
            preconditionOfModification.IsAllPrerequisitesSupplied(email, name, birthDate);

            this.Name = name;
            this.Email = email;
            this.BirthDate = birthDate;

            AddDomainEvent(new CustomerUpdatedEvent(this));
        }
    }
}
