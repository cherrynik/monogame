﻿using GameDesktop.Resources.Internal;
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
        serviceRegistry.Register(factory =>
        {
            Game game = new(factory.GetInstance<ILogger>(),
                factory.GetInstance<IServiceContainer>())
            {
                IsMouseVisible = IsMouseVisible,
                IsFixedTimeStep = IsFixedTimeStep,
                Window = { Title = "Test" },
                Content = { RootDirectory = AppVariable.ContentRootDirectory, },
            };

            // Hack. Resolving cycle dependency issue (fundamental architecture)
            // Implicitly adds itself in the game services container.
            new GraphicsDeviceManager(game);
            
            return game;
        });
    }
}
