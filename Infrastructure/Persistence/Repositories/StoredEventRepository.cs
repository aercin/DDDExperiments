using Domain.Aggregates.StoredEvent;

namespace Infrastructure.Persistence.Repositories
{
    public class StoredEventRepository : GenericRepository<StoredEvent>, IStoredEventRepository
    {
        public StoredEventRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
