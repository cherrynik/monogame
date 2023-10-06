using System;
using Components;
using Components.World;
using Entitas;
using Features;
using Features.Factories;
using GameDesktop.CompositionRoots.Entities;
using GameDesktop.CompositionRoots.Features;
using GameDesktop.Resources;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Serilog;
using Services;
using Services.Factories;
using Services.Input;
using Services.Movement;
using Systems;

namespace GameDesktop.CompositionRoots;

public class RootFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        // Layered registration architecture (horizontally & vertically)
        // Hence, it allows async/multi-threaded registration

        // If it's split with space-line, then it's the end of a group.
        // A group (of registering lines) can be multi-threaded.
        // At the end of a group, the whole group has to be resolved successfully,
        // before going further.
        serviceRegistry.RegisterFrom<FundamentalCompositionRoot>();

        serviceRegistry.RegisterFrom<PlayerEntityCompositionRoot>();
        serviceRegistry.RegisterFrom<StaticEntityCompositionRoot>();

        serviceRegistry.RegisterSingleton(typeof(AbstractFactory<>));
        serviceRegistry.RegisterSingleton<WorldInitializeFeature>();

        serviceRegistry.RegisterFrom<InputFeatureCompositionRoot>();
        serviceRegistry.RegisterFrom<CameraFeatureCompositionRoot>();
        serviceRegistry.RegisterFrom<MovementFeatureCompositionRoot>();

        // Main entry point
        serviceRegistry.Register<RootFeature>();
    }
}
