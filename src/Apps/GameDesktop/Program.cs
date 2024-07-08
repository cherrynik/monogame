using System;
using System.IO;
using Constants;
using GameDesktop;
using GameDesktop.Builders;
using LightInject;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using ConfigurationBuilder = GameDesktop.Builders.ConfigurationBuilder;

IConfigurationRoot configuration = ConfigurationBuilder.Create();
Environment.SetEnvironmentVariable(EnvironmentNames.AppBaseDirectory,
    Directory.GetParent(AppContext.BaseDirectory)!.FullName);

using Logger logger = LoggerBuilder.Create(configuration);
Log.Logger = logger; // To disable logging, use this instead: Logger.None;

Log.Logger.ForContext<Program>().Verbose("Configuration & Logger (+ Sentry) initialized");

try
{
    // If "using" is used with the container, then the game systems are disposed.
    // For custom graph visualization, as a reference: https://docs.simpleinjector.org/en/latest/diagnostics.html
    using ServiceContainer container = new(
        new ContainerOptions
        {
            EnablePropertyInjection = false,
            EnableCurrentScope = false,
            LogFactory = _ => entry => Log.Logger
                .ForContext<ServiceContainer>()
                .Verbose($"{entry.Message}"),
        });

    container.RegisterInstance<IServiceContainer>(container)
        .RegisterInstance<IServiceFactory>(container)
        .RegisterInstance<IConfiguration>(configuration)
        .RegisterInstance<ILogger>(logger)
        .RegisterFrom<GameCompositionRoot>();

    // If "using" is used with the game instance, only the instance is disposed.
    using var game = container.GetInstance<Game>();
    game.Run();
}
catch (Exception e)
{
#if IS_CI
    if (e.Message.Contains(Errors.FailedToCreateGraphicsDevice)) Environment.Exit(0);
#endif

    Log.Logger.ForContext<Program>().Fatal(e.ToString());
    Environment.Exit(1);
}
