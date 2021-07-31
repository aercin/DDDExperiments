using Domain.Aggregates.Customer.Strategies.Interfaces;
using System;
using System.Text;

namespace Domain.Aggregates.Customer.Strategies.Implementations
{
    public class CustomerModificationPrerequisiteStrategy1 : ICustomerModificationPrerequisiteStrategy
    {
        public CustomerModificationPrerequisiteStrategy1()
        {
        }

        public void IsAllPrerequisitesSupplied(string email, string name, DateTime birthDate)
        {
            var strBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(email))
            {
                strBuilder.AppendLine("Email adresi boş olamaz");
            }
            if (string.IsNullOrEmpty(name))
            {
                strBuilder.AppendLine("Müşteri ismi boş olamaz");
            }
            if (birthDate != DateTime.MinValue && DateTime.Now.Subtract(birthDate).TotalDays / 360 < 18)
            {
                strBuilder.AppendLine("Müşteri 18 yaşından büyük olmalıdır");
            }
            if (!string.IsNullOrEmpty(strBuilder.ToString()))
            {
                throw new ArgumentException(strBuilder.ToString());
            }
            //TOD: Gönderilen bilgide yer alan mail adresini kullanan başka bir müşteri bilgisi var mı kontrolü eklenebilir.
        }
    }
}
