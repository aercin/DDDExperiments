using Domain.Aggregates.Customer;
using Domain.Aggregates.StoredEvent;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }
        IStoredEventRepository StoredEvents { get; }
        Task<int> CompleteAsync(CancellationToken cancellationToken);
        void Dispose();
    }
}
