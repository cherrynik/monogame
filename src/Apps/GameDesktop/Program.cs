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
    // "Using" keyword should be used either with the container or with the game instance.
    // Otherwise, you'll get the double-disposing behavior.
    ServiceContainer container = new(
        new ContainerOptions
        {
            EnablePropertyInjection = false,
            EnableCurrentScope = false,
            LogFactory = _ => entry => Log.Logger
                .ForContext<ServiceContainer>()
                .Verbose($"{entry.Message}"),
        });

    container.RegisterInstance<IServiceContainer>(container);
    container.RegisterInstance<IServiceFactory>(container);

    container.RegisterInstance<IConfiguration>(configuration);
    container.RegisterInstance<ILogger>(logger);

    container.RegisterFrom<GameCompositionRoot>();

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
