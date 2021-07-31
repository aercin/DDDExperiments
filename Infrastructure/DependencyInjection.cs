using Application.Common.Interfaces;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Strategies.Implementations;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Aggregates.StoredEvent;
using Domain.Common;
using FluentValidation;
using Infrastructure.FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidation, FluentValidationService>();

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "DDDSampleOneDB"), ServiceLifetime.Singleton);
                services.AddSingleton<IUnitOfWork, UnitOfWork>();
                services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                services.AddSingleton<ICustomerRepository, CustomerRepository>();
                services.AddSingleton<IStoredEventRepository, StoredEventRepository>();
                services.AddSingleton<IDomainEventService, DomainEventService>();
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                services.AddScoped<ICustomerRepository, CustomerRepository>();
                services.AddScoped<IStoredEventRepository, StoredEventRepository>();
                services.AddScoped<IDomainEventService, DomainEventService>();
            }
            services.AddSingleton<ITimer, TimerService>();
            services.AddScoped<ICustomerCreationPrerequisiteStrategy, CustomerCreationPrerequisiteStrategy1>();
            services.AddScoped<ICustomerModificationPrerequisiteStrategy, CustomerModificationPrerequisiteStrategy1>();
        }
    }
}
