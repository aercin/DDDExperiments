using Domain.Aggregates.Customer.Strategies.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTests.Aggregates.Customer
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void Create_IsANewCustomerSuccessfullyCreated_ReturnsTrue()
        {
            //Arrange
            var customerCreationPrerequisiteStrategyObj = new Mock<ICustomerCreationPrerequisiteStrategy>();

            //Act
            var newCustomer = Domain.Aggregates.Customer.Customer.Create("dummyname", "dummy@email", DateTime.Today.AddYears(-20), customerCreationPrerequisiteStrategyObj.Object);

            //Assert
            Assert.IsTrue(newCustomer.Id != Guid.Empty);
            Assert.IsTrue(newCustomer.Name.Equals("dummyname"));
            Assert.IsTrue(newCustomer.Email.Equals("dummy@email"));
            Assert.IsTrue(newCustomer.BirthDate.Equals(DateTime.Today.AddYears(-20)));
        }

        [Test]
        public void Update_IsCustomerSuccessfullyModified_ReturnsTrue()
        {
            //Arrange
            var customerModificationPrerequisiteStrategyObj = new Mock<ICustomerModificationPrerequisiteStrategy>();

            var customerCreationPrerequisiteStrategyObj = new Mock<ICustomerCreationPrerequisiteStrategy>();
            var stubOfCustomer = Domain.Aggregates.Customer.Customer.Create("dummyname", "dummy@email", DateTime.Today.AddYears(-20), customerCreationPrerequisiteStrategyObj.Object);


            //Act
            stubOfCustomer.Update("dummyname1", "dummy@email1", DateTime.Today.AddYears(-25), customerModificationPrerequisiteStrategyObj.Object);

            //Assert 
            Assert.IsTrue(stubOfCustomer.Name.Equals("dummyname1"));
            Assert.IsTrue(stubOfCustomer.Email.Equals("dummy@email1"));
            Assert.IsTrue(stubOfCustomer.BirthDate.Equals(DateTime.Today.AddYears(-25)));
        }
    }
}
