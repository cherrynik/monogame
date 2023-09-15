using System;
using GameDesktop;
using LightInject;
using Serilog.Core;

ContainerOptions containerOptions =
    new ContainerOptions { LogFactory = _ => entry => Console.WriteLine($@"LightInject: {entry.Message}") };
using ServiceContainer container = new(containerOptions);

container.RegisterFrom<LoggingCompositionRoot>();
using var logger = container.GetInstance<Logger>();

try
{
    container.RegisterFrom<ProgramCompositionRoot>();

    using Game game = container.GetInstance<Game>();
    game.Run();
}
catch (Exception e)
{
    logger.Error(e.ToString());
}
