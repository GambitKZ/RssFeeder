﻿using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RssFeeder.Infrastructure.AzureTable;

[assembly: FunctionsStartup(typeof(RssFeeder.Web.AzureFunction.Startup))]

namespace RssFeeder.Web.AzureFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddAzureTableContext(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"),
            "rssFeed");

        // Add Application's ConfigureServices so Mediator will work
        builder.Services.AddApplicationServices();
    }
}