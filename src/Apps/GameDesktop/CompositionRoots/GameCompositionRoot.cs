using System.Linq;
using GameDesktop.Resources.Internal;
using LightInject;
using Microsoft.Xna.Framework;
using Serilog;

namespace GameDesktop.CompositionRoots;

internal class GameCompositionRoot : ICompositionRoot
{
    private const float TargetFramesPerSecond = 120.0f;
    private const bool IsMouseVisible = true;
    private const bool IsFixedTimeStep = false;

    public void Compose(IServiceRegistry serviceRegistry)
    {
        ServiceRegistration[] services = serviceRegistry.AvailableServices.ToArray();

        var logger =
            (ILogger)services.First(r => r.ServiceType == typeof(ILogger)).Value;
        var container =
            (IServiceContainer)services.First(r => r.ServiceType == typeof(IServiceContainer)).Value;

        Game game = new(logger, container)
        {
            IsMouseVisible = IsMouseVisible,
            IsFixedTimeStep = IsFixedTimeStep,
            Content = { RootDirectory = AppVariable.ContentRootDirectory, },
        };
        serviceRegistry.RegisterInstance(game);

        // Hack. Resolving cycle dependency issue (fundamental architecture)
        // Implicitly adds itself in the game services container.
        GraphicsDeviceManager graphicsDeviceManager = new(game);
        serviceRegistry.RegisterInstance(graphicsDeviceManager);
    }
}
