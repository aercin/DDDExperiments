using Application.Common.Mappings;
using Application.Common.Models;
using Application.Customer.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customer.Queries.GetCustomersWithPagination
{
    public class GetCustomersWithPaginationQuery : IRequest<PaginatedList<CustomerDto>>
    {
        public virtual string Name { get; set; }
        public virtual int PageNumber { get; set; } = 1;
        public virtual int PageSize { get; set; } = 10;
    }

    public class GetCustomerWithPaginationQueryHandler : IRequestHandler<GetCustomersWithPaginationQuery, PaginatedList<CustomerDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetCustomerWithPaginationQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        public Task<PaginatedList<CustomerDto>> Handle(GetCustomersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var result = this._uow.Customers.Find(x => x.Name.ToLower().Contains(request.Name.ToLower()), isNoTracking: true)
                                                  .ProjectTo<CustomerDto>(this._mapper.ConfigurationProvider)
                                                  .MappedPaginatedList<CustomerDto>(request.PageNumber, request.PageSize);

            return Task.FromResult(result);
        }
    }
}
