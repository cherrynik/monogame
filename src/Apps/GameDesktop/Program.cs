using System;
using GameDesktop;
using LightInject;
using Serilog.Core;

using Logger logger = LogFactory.Create();

ContainerOptions containerOptions =
    new ContainerOptions
    {
        LogFactory = _ => entry => logger.ForContext<ServiceContainer>().Verbose("{EntryMessage}", entry.Message)
    };
using ServiceContainer container = new(containerOptions);

try
{
    container.Register(_ => logger, new PerContainerLifetime());

    container.RegisterFrom<GameCompositionRoot>();

    using Game game = container.GetInstance<Game>();
    game.Run();
}
catch (Exception e)
{
    logger.Error(e.ToString());
}
