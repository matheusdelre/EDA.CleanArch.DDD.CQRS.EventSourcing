﻿using Infrastructure.DependencyInjection.Extensions;
using Infrastructure.DependencyInjection.Options;
using Infrastructure.EventSourcing.EventStore.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;

var builder = Host.CreateDefaultBuilder(args);

builder.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    options.ValidateOnBuild = true;
});

builder.ConfigureAppConfiguration(configurationBuilder =>
{
    configurationBuilder
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables();
});

builder.ConfigureLogging((context, loggingBuilder) =>
{
    Log.Logger = new LoggerConfiguration().ReadFrom
        .Configuration(context.Configuration)
        .CreateLogger();

    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

builder.ConfigureServices((context, services) =>
{
    services.AddApplicationServices();

    services.AddEventStoreRepository();
    services.AddProjectionsRepository();

    services.AddEventStoreDbContext();
    services.AddProjectionsDbContext();

    services.AddMassTransitWithRabbitMqAndQuartz();

    services.AddMessagesFluentValidation();

    services.AddNotificationContext();

    services.ConfigureEventStoreOptions(
        context.Configuration.GetSection(nameof(EventStoreOptions)));

    services.ConfigureSqlServerRetryOptions(
        context.Configuration.GetSection(nameof(SqlServerRetryOptions)));

    services.ConfigureRabbitMqOptions(
        context.Configuration.GetSection(nameof(RabbitMqOptions)));

    services.ConfigureQuartzOptions(
        context.Configuration.GetSection(nameof(QuartzOptions)));
    
    services.ConfigureMassTransitHostOptions(
        context.Configuration.GetSection(nameof(MassTransitHostOptions)));
});

using var host = builder.Build();

var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

applicationLifetime.ApplicationStopping.Register(() =>
{
    Log.Information("Waiting 15s for a graceful termination...");
    Thread.Sleep(15000);
});

applicationLifetime.ApplicationStopped.Register(() =>
{
    Log.Information("Application completely stopped");
    System.Diagnostics.Process.GetCurrentProcess().Kill();
});

try
{
    var environment = host.Services.GetRequiredService<IHostEnvironment>();

    if (environment.IsDevelopment() || environment.IsStaging())
    {
        await using var scope = host.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<EventStoreDbContext>();
        await dbContext.Database.MigrateAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    await host.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await host.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    host.Dispose();
}