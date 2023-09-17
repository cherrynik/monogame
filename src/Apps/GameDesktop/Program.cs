using System;
using GameDesktop;
using LightInject;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

IConfigurationRoot configuration = ConfigurationFactory.Create();
using Logger logger = LogFactory.Create(configuration);

ContainerOptions containerOptions =
    new ContainerOptions
    {
        LogFactory = _ => entry => logger.ForContext<ServiceContainer>().Verbose($"{entry.Message}")
    };
using ServiceContainer container = new(containerOptions);

try
{
    container.Register(_ => configuration, new PerContainerLifetime());
    container.Register(_ => logger, new PerContainerLifetime());

    container.RegisterFrom<GameCompositionRoot>();

    using Game game = container.GetInstance<Game>();
    game.Run();
}
catch (Exception e)
{
    logger.Error(e.ToString());
}
