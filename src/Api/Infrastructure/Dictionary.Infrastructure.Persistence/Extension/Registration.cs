using Dictionary.Api.Application.Interfaces.Repositories;
using Dictionary.Infrastructure.Persistence.Context;
using Dictionary.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.Infrastructure.Persistence.Extension;

public static class Registration
{
    public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DictionaryContext>(conf =>
        {
            var connStr = configuration["DictionaryConnectionString"].ToString();
            conf.UseSqlServer(connStr, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        });
        // var seedData = new SeedData();
        // seedData.SeedAsync(configuration).GetAwaiter().GetResult();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}