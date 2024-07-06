using System;
using System.Linq;
using Constants;
using GameDesktop;
using LightInject;
using Microsoft.Xna.Framework;
using Serilog;

[assembly: CompositionRootType(typeof(GameCompositionRoot))]

namespace GameDesktop;

internal class GameCompositionRoot : ICompositionRoot
{
    private const float SecondInMs = 1_000.0f;

    private const float TargetFramesPerSecond = 120.0f;
    private const bool IsFixedTimeStep = false;
    private const bool IsVSyncOn = false;

    private const bool IsMouseVisible = true;
    private const bool IsFullScreen = false;
    private const int WindowWidth = 640;
    private const int WindowHeight = 360;

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
            TargetElapsedTime = TimeSpan.FromMilliseconds(SecondInMs / TargetFramesPerSecond),
            Content = { RootDirectory = Contents.RootDirectory, },
        };
        serviceRegistry.RegisterInstance(game);

        // Hack. Resolving cycle dependency issue (fundamental architecture)
        // Implicitly adds itself in the game services' container.
        GraphicsDeviceManager graphicsDeviceManager = new(game)
        {
            SynchronizeWithVerticalRetrace = IsVSyncOn,
            IsFullScreen = IsFullScreen,
            PreferredBackBufferWidth = WindowWidth,
            PreferredBackBufferHeight = WindowHeight
        };
        serviceRegistry.RegisterInstance(graphicsDeviceManager);
    }
}
