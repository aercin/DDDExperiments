using System;

namespace Domain.Aggregates.Customer.Strategies.Interfaces
{
    public interface ICustomerModificationPrerequisiteStrategy
    {
        public void IsAllPrerequisitesSupplied(string email, string name, DateTime birthDate);
    }
}
