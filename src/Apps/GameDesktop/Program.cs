using System;
using GameDesktop;
using LightInject;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

IConfigurationRoot configuration = ConfigurationFactory.Create();
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
    logger.ForContext<Program>().Fatal(e.ToString());
}
