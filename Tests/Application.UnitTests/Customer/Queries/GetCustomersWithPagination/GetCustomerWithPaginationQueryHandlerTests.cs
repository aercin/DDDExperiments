using Application.Common.Mappings;
using Application.Customer.Queries.GetCustomersWithPagination;
using AutoMapper;
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

namespace Application.UnitTests.Customer.Queries.GetCustomersWithPagination
{
    [TestFixture]
    public class GetCustomerWithPaginationQueryHandlerTests
    {
        [TestCase("alp", 2, 4, 2, 6)]
        [TestCase("alper", 1, 2, 2, 2)]
        public async Task Handle_IsGettingCorrectResultForGivenCriteria_VerifyGettingCorrectResultForGivenCriteria(string name, int pageNumber, int pageSize, int pageItemCount, int totalItemCount)
        {
            //Arrange 
            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();

            var customerItemList = new List<Domain.Aggregates.Customer.Customer>
            {
                Domain.Aggregates.Customer.Customer.Create("alp1", "alp1@alp1", DateTime.MinValue, mockCustomerCreationPrerequisiteStrategy.Object),
                Domain.Aggregates.Customer.Customer.Create("alp2", "alp2@alp2", DateTime.MinValue, mockCustomerCreationPrerequisiteStrategy.Object),
                Domain.Aggregates.Customer.Customer.Create("alp3", "alp3@alp3", DateTime.MinValue, mockCustomerCreationPrerequisiteStrategy.Object),
                Domain.Aggregates.Customer.Customer.Create("alp4", "alp4@alp4", DateTime.MinValue, mockCustomerCreationPrerequisiteStrategy.Object),
                Domain.Aggregates.Customer.Customer.Create("alper1", "alper1@alper1", DateTime.MinValue, mockCustomerCreationPrerequisiteStrategy.Object),
                Domain.Aggregates.Customer.Customer.Create("alper2", "alper2@alper2", DateTime.MinValue, mockCustomerCreationPrerequisiteStrategy.Object)
            };

            Mock<ICustomerRepository> mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(m => m.Find(It.IsAny<Expression<Func<Domain.Aggregates.Customer.Customer, bool>>>(), true)).Returns(customerItemList.Where(x => x.Name.ToLower().Contains(name.ToLower())).AsQueryable());

            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.Customers).Returns(mockCustomerRepo.Object);

            var mappingProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            IMapper mapper = new Mapper(configuration);

            var getCustomerWithPaginationQueryHandlerObj = new GetCustomerWithPaginationQueryHandler(mockUow.Object, mapper);

            var mockGetCustomersWithPaginationQuery = new Mock<GetCustomersWithPaginationQuery>();
            mockGetCustomersWithPaginationQuery.SetupProperty(x => x.Name, name);
            mockGetCustomersWithPaginationQuery.SetupProperty(x => x.PageNumber, pageNumber);
            mockGetCustomersWithPaginationQuery.SetupProperty(x => x.PageSize, pageSize);

            //Act
            var result = await getCustomerWithPaginationQueryHandlerObj.Handle(mockGetCustomersWithPaginationQuery.Object, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(result.Items.Count == pageItemCount);
            Assert.IsTrue(result.TotalCount == totalItemCount);
        }
    }
}
