using Domain.Aggregates.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.Property(c => c.Name).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Email).HasColumnType("varchar").HasMaxLength(100).IsRequired();
        }
    }
}
