using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using RssFeeder.Infrastructure.AzureTable;

[assembly: FunctionsStartup(typeof(RssFeeder.Web.AzureFunction.Startup))]

namespace RssFeeder.Web.AzureFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        //builder.Services.AddHttpClient();
        //            builder.Services.AddSingleton<IMyService>((s) => {
        //    return new MyService();
        //    });
        //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();

        builder.Services.AddAzureTableContext(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"),
            Environment.GetEnvironmentVariable("TABLE_NAME"));
    }
}