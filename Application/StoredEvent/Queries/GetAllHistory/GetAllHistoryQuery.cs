using Application.StoredEvent.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StoredEvent.Queries.GetAllHistory
{
    public class GetAllHistoryQuery : IRequest<List<CustomerHistoryDto>>
    {
    }

    public class GetAllHistoryQueryHandler : IRequestHandler<GetAllHistoryQuery, List<CustomerHistoryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllHistoryQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        public Task<List<CustomerHistoryDto>> Handle(GetAllHistoryQuery request, CancellationToken cancellationToken)
        {
            var customerHistories = this._uow.StoredEvents.All(isNoTracking: true).OrderBy(x => x.AggregateId).ThenBy(x => x.Timestamp).ToList().Select(item => this._mapper.Map<CustomerHistoryDto>(item)).ToList();

            return Task.FromResult(customerHistories);
        }
    }
}
