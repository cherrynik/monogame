using System;
using System.IO;
using GameDesktop;
using GameDesktop.CompositionRoots;
using GameDesktop.Factories;
using GameDesktop.Resources.Internal;
using LightInject;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

IConfigurationRoot configuration = ConfigurationFactory.Create();
Environment.SetEnvironmentVariable(EnvironmentVariable.AppBaseDirectory,
    Directory.GetParent(AppContext.BaseDirectory)!.FullName);

using Logger logger = LogFactory.Create(configuration);

logger.ForContext<Program>().Verbose("Configuration & Logger (+ Sentry) initialized");

try
{
    using ServiceContainer container = new(
        new ContainerOptions
        {
            EnablePropertyInjection = false,
            EnableCurrentScope = false,
            LogFactory = _ => entry => logger
                .ForContext<ServiceContainer>()
                .Verbose($"{entry.Message}"),
        });

    container.RegisterInstance<IServiceContainer>(container);
    container.RegisterInstance<IConfiguration>(configuration);
    container.RegisterInstance<ILogger>(logger);

    container.RegisterFrom<GameCompositionRoot>();

    using var game = container.GetInstance<Game>();
    game.Run();
}
catch (Exception e)
{
#if IS_CI
    if (e.Message.Contains(Error.FailedToCreateGraphicsDevice))
    {
        Environment.Exit(0);
    }
#endif

    logger.ForContext<Program>().Fatal(e.ToString());
    Environment.Exit(1);
}
