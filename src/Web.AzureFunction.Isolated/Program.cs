using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssFeeder.Infrastructure.AzureTable;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.Services.AddAzureTableContext(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"),
           "rssFeed");

        // Add Application's ConfigureServices so Mediator will work
        builder.Services.AddApplicationServices();
    })
    .Build();

host.Run();