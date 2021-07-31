using Domain.Aggregates.Customer;
using Domain.Aggregates.StoredEvent;
using Domain.Common;
using Infrastructure.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
            this.Customers = new CustomerRepository(_context);
            this.StoredEvents = new StoredEventRepository(_context);
        }

        public ICustomerRepository Customers { get; private set; }
        public IStoredEventRepository StoredEvents { get; private set; } 

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await this._context.SaveChangesAsync(cancellationToken);
        } 
        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
