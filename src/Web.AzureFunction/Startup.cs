using System;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using RssFeeder.Infrastructure.AzureTable;

[assembly: FunctionsStartup(typeof(RssFeeder.Web.AzureFunction.Startup))]

namespace RssFeeder.Web.AzureFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        //builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddAzureTableContext(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"),
            "rssFeed");
    }
}