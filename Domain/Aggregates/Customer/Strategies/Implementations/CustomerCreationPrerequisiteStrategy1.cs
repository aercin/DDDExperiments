using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using System;
using System.Text;
using System.Linq;

namespace Domain.Aggregates.Customer.Strategies.Implementations
{
    public class CustomerCreationPrerequisiteStrategy1 : ICustomerCreationPrerequisiteStrategy
    {
        private readonly IUnitOfWork _uow;
        public CustomerCreationPrerequisiteStrategy1(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        protected CustomerCreationPrerequisiteStrategy1()
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
            if (birthDate != DateTime.MinValue )
            {
                if(DateTime.Now.Subtract(birthDate).TotalDays / 360 < 18)
                {
                    strBuilder.AppendLine("Müşteri 18 yaşından büyük olmalıdır");
                } 
            }
            else
            {
                strBuilder.AppendLine("Müşteri doğum tarihi boş olamaz");
            }
            if(!string.IsNullOrEmpty(strBuilder.ToString()))
            {
                throw new ArgumentException(strBuilder.ToString());
            }

            if(this._uow.Customers.Find(x => x.Email.ToLower().Equals(email.ToLower()),isNoTracking:true).Any())
            {
                throw new ArgumentException("Mail adresi sistemde kullanımdadır");
            } 
        }
    }
}
