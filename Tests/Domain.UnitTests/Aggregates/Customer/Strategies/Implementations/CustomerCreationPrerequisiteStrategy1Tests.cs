using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Strategies.Implementations;
using Domain.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.UnitTests.Aggregates.Customer.Strategies.Implementations
{
    [TestFixture]
    public class CustomerCreationPrerequisiteStrategy1Tests
    {
        Mock<IUnitOfWork> mockUow;
        Mock<ICustomerRepository> mockCustomerRepo;

        [SetUp]
        public void Setup()
        {
            mockUow = new Mock<IUnitOfWork>();
            mockCustomerRepo = new Mock<ICustomerRepository>(); 
        }

        [Test]
        public void IsAllPrerequisitesSupplied_WhenEmailIsEmpty_ThrowArgumentException()
        {
            //Arrange 
            var customerCreationPrerequisiteStrategy1Obj = new CustomerCreationPrerequisiteStrategy1(mockUow.Object);

            //Act & Assert
            Assert.Throws<ArgumentException>(() => customerCreationPrerequisiteStrategy1Obj.IsAllPrerequisitesSupplied(string.Empty, "blabla", DateTime.Now.AddYears(-20)));
        }

        [Test]
        public void IsAllPrerequisitesSupplied_WhenEmailIsAlreadyExist_ThrowArgumentException()
        {
            //Arrange
            var stubOfCustomerRepoFindMethod = new List<Domain.Aggregates.Customer.Customer> { Mock.Of<Domain.Aggregates.Customer.Customer>() };
            mockCustomerRepo.Setup(m => m.Find(It.IsAny<Expression<Func<Domain.Aggregates.Customer.Customer, bool>>>(), true)).Returns(stubOfCustomerRepoFindMethod.AsQueryable());
            mockUow.Setup(x => x.Customers).Returns(mockCustomerRepo.Object);

            var customerCreationPrerequisiteStrategy1Obj = new CustomerCreationPrerequisiteStrategy1(mockUow.Object);

            //Act & Assert
            Assert.Throws<ArgumentException>(() => customerCreationPrerequisiteStrategy1Obj.IsAllPrerequisitesSupplied("blabla", "blabla", DateTime.Now.AddYears(-20)));
        }
    }
}
