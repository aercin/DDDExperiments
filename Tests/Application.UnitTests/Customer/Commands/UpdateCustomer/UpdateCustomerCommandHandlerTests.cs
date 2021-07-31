using Application.Common.Exceptions;
using Application.Customer.Commands.UpdateCustomer;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Customer.Commands.UpdateCustomer
{
    [TestFixture]
    public class UpdateCustomerCommandHandlerTests
    {
        [Test]
        public void Handle_RequestedCustomerNotExist_ThrowNotFoundEx()
        {
            //Arrange 
            var mockCustomerModificationPrerequisiteStrategy = new Mock<ICustomerModificationPrerequisiteStrategy>();

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(m => m.Find(It.IsAny<Expression<Func<Domain.Aggregates.Customer.Customer, bool>>>(), false)).Returns(new List<Domain.Aggregates.Customer.Customer>().AsQueryable());

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.Customers).Returns(mockCustomerRepo.Object);

            var updateCustomerCommandHandlerObj = new UpdateCustomerCommandHandler(mockUow.Object, mockCustomerModificationPrerequisiteStrategy.Object);

            var mockUpdateCustomerCommand = new Mock<UpdateCustomerCommand>();
            mockUpdateCustomerCommand.SetupProperty(x => x.Id, Guid.NewGuid());

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => updateCustomerCommandHandlerObj.Handle(mockUpdateCustomerCommand.Object, new System.Threading.CancellationToken()));
        }

        [Test]
        public async Task Handle_EntityPropertiesIsSuccessfullyUpdated_VerifyEntitySuccessfullyUpdated()
        {
            //Arrange  
            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>(); 
            var mockCustomer = Domain.Aggregates.Customer.Customer.Create("bla", "bla@bla", DateTime.MinValue, mockCustomerCreationPrerequisiteStrategy.Object);
             
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(m => m.Find(It.IsAny<Expression<Func<Domain.Aggregates.Customer.Customer, bool>>>(), false)).Returns(new List<Domain.Aggregates.Customer.Customer> { mockCustomer }.AsQueryable());

            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.Customers).Returns(mockCustomerRepo.Object);

            var mockCustomerModificationPrerequisiteStrategy = new Mock<ICustomerModificationPrerequisiteStrategy>();

            var updateCustomerCommandHandlerObj = new UpdateCustomerCommandHandler(mockUow.Object, mockCustomerModificationPrerequisiteStrategy.Object);

            var mockUpdateCustomerCommand = new Mock<UpdateCustomerCommand>();
            mockUpdateCustomerCommand.SetupProperty(x => x.Id, mockCustomer.Id);
            mockUpdateCustomerCommand.SetupProperty(x => x.Name, "blaModified");
            mockUpdateCustomerCommand.SetupProperty(x => x.Email, "bla@blaModified");
            mockUpdateCustomerCommand.SetupProperty(x => x.BirthDate, DateTime.Today);
           
            //Act
            var result = await updateCustomerCommandHandlerObj.Handle(mockUpdateCustomerCommand.Object, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(mockCustomerRepo.Object.Find(x => x.Id == mockCustomer.Id).Single().Name == "blaModified");
            Assert.IsTrue(mockCustomerRepo.Object.Find(x => x.Id == mockCustomer.Id).Single().Email == "bla@blaModified");
            Assert.IsTrue(mockCustomerRepo.Object.Find(x => x.Id == mockCustomer.Id).Single().BirthDate == DateTime.Today);
        }
    }
}
