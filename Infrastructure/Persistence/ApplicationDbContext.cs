using Application.Common.Interfaces;
using Domain.Aggregates.Customer;
using Domain.Aggregates.StoredEvent;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDomainEventService _domainEventService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventService domainEventService) : base(options)
        {
            this._domainEventService = domainEventService; 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<StoredEvent> StoredEvents { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await DispatchEvents();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task DispatchEvents()
        {
            var domainEntities =  ChangeTracker.Entries<AggregateRootBaseEntity>()
                                               .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();


            domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async (domainEvent) =>
                        {
                            await this._domainEventService.Publish(domainEvent);
                        });

            await Task.WhenAll(tasks); 
        }

    }
}
