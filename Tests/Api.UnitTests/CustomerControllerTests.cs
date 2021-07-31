using Api.Controllers;
using Application.Customer.Commands.CreateCustomer;
using Application.Customer.Commands.DeleteCustomer;
using Application.Customer.Commands.UpdateCustomer;
using Application.Customer.Queries.GetCustomersWithPagination;
using Application.StoredEvent.Queries.GetAllHistory;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Api.UnitTests
{
    /// <summary>
    /// Test class name convention is Name Of Class which is tested + "Tests"
    /// Test method name convention is Name Of Method which is tested + "_" + scenario + "_" + expected behaviour
    /// </summary>
    [TestFixture]
    public class CustomerControllerTests
    {
        Mock<ISender> mockSender;
        CustomerController customerControllerObj;

        [SetUp]
        public void Setup()
        {
            //It is run once before every test's running
            //Arrange
            mockSender = new Mock<ISender>();
            customerControllerObj = new CustomerController(mockSender.Object);
        }

        [TearDown]
        public void CleanUp()
        {//It is run once after every test's running

        }

        [Test]
        public async Task CreateCustomer_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            await customerControllerObj.CreateCustomer(It.IsAny<CreateCustomerCommand>());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<CreateCustomerCommand>(), default), Times.Once);
        }

        [Test]
        public async Task UpdateCustomer_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            await customerControllerObj.UpdateCustomer(It.IsAny<UpdateCustomerCommand>());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<UpdateCustomerCommand>(), default), Times.Once);
        }

        [Test]
        public async Task DeleteCustomer_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            await customerControllerObj.DeleteCustomer(It.IsAny<Guid>());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<DeleteCustomerCommand>(), default), Times.Once);
        }

        [Test]
        public async Task GetCustomer_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            await customerControllerObj.GetCustomer(It.IsAny<GetCustomersWithPaginationQuery>());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<GetCustomersWithPaginationQuery>(), default), Times.Once);
        }

        [Test]
        public async Task GetCustomerAllHistory_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            await customerControllerObj.GetCustomerAllHistory(It.IsAny<GetAllHistoryQuery>());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<GetAllHistoryQuery>(), default), Times.Once);
        }
    }
}