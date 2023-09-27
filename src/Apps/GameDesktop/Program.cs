using System;
using System.IO;
using GameDesktop;
using GameDesktop.CompositionRoots;
using GameDesktop.Resources;
using LightInject;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
#if IS_CI
using GameDesktop.Resources;
#endif

IConfigurationRoot configuration = ConfigurationFactory.Create();
Environment.SetEnvironmentVariable(EnvironmentVariables.AppBaseDirectory,
    Directory.GetParent(AppContext.BaseDirectory)!.FullName);

using Logger logger = LogFactory.Create(configuration);

logger.ForContext<Program>().Verbose("Configuration & Logger (+ Sentry) initialized");

try
{
    ContainerOptions containerOptions =
        new ContainerOptions
        {
            LogFactory = _ => entry => logger.ForContext<ServiceContainer>().Verbose($"{entry.Message}")
        };
    using ServiceContainer container = new(containerOptions);

    container.Register<IServiceContainer>(_ => container);
    container.Register<IConfiguration>(_ => configuration, new PerContainerLifetime());
    container.Register<ILogger>(_ => logger, new PerContainerLifetime());

    container.RegisterFrom<GameCompositionRoot>();

    using Game game = container.GetInstance<Game>();
    game.Run();
}
catch (Exception e)
{
#if IS_CI
    if (e.Message.Contains(Errors.FailedToCreateGraphicsDevice))
    {
        Environment.Exit(0);
    }
#endif

    logger.ForContext<Program>().Fatal(e.ToString());
    Environment.Exit(1);
}
