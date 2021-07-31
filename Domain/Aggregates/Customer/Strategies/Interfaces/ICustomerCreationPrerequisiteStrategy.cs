using System;

namespace Domain.Aggregates.Customer.Strategies.Interfaces
{
    public interface ICustomerCreationPrerequisiteStrategy
    {
        public void IsAllPrerequisitesSupplied(string email, string name, DateTime birthDate);
    }
}
