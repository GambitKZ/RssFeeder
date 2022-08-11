using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Infrastructure.AzureTable;

public static class StartupSetup
{
    public static void AddAzureTableContext(this IServiceCollection services, string connectionString, string tableName)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IRepositoryBase<IFeedItem>>(x =>
        ActivatorUtilities.CreateInstance<AzureTableRepository<IFeedItem>>(x,
                connectionString, tableName));
    }
}