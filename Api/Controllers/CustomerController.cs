using Application.Common.Models;
using Application.Customer.Commands.CreateCustomer;
using Application.Customer.Commands.DeleteCustomer;
using Application.Customer.Commands.UpdateCustomer;
using Application.Customer.Queries.GetCustomersWithPagination;
using Application.Customer.Queries.Models;
using Application.StoredEvent.Queries.GetAllHistory;
using Application.StoredEvent.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ISender _mediator;

        public CustomerController(ISender mediator)
        {
            this._mediator = mediator;
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<Result<Guid>>> CreateCustomer(CreateCustomerCommand command)
        {
            return await this._mediator.Send(command);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<Result>> UpdateCustomer(UpdateCustomerCommand command)
        {
            return await this._mediator.Send(command);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<Result>> DeleteCustomer(Guid id)
        {
            return await this._mediator.Send(new DeleteCustomerCommand { Id = id });
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedList<CustomerDto>>> GetCustomer([FromQuery] GetCustomersWithPaginationQuery query)
        {
            return await this._mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<CustomerHistoryDto>>> GetCustomerAllHistory([FromQuery] GetAllHistoryQuery query)
        {
            return await this._mediator.Send(query);
        }
    }
}
