using System;
using Components;
using Components.World;
using Entitas;
using Features;
using Features.Factories;
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
    private static readonly string Path = System.IO.Path.Join(
        Environment.GetEnvironmentVariable(EnvironmentVariables.AppBaseDirectory),
        SpriteSheets.Player);

    public void Compose(IServiceRegistry serviceRegistry)
    {
        // Layered registration architecture (horizontally & vertically)
        // Hence, it allows async/multi-threaded registration

        // If it's split with space-line, then it's the end of a group.
        // A group (of registering lines) can be multi-threaded.
        // At the end of a group, the whole group has to be resolved successfully,
        // before going further.
        serviceRegistry.RegisterFrom<FundamentalCompositionRoot>();

        serviceRegistry.RegisterFrom<InputFeatureCompositionRoot>();
        RegisterCreatePlayerEntitySystem(serviceRegistry);
        RegisterAnimatedMovementSystem(serviceRegistry);
        RegisterCameraFollowingSystem(serviceRegistry);
        serviceRegistry.RegisterFrom<MovementFeatureCompositionRoot>();

        // serviceRegistry.RegisterFrom<PlayerEntityCompositionRoot>();
        serviceRegistry.RegisterFrom<StaticEntityCompositionRoot>();

        serviceRegistry.RegisterSingleton(typeof(AbstractFactory<>));
        serviceRegistry.RegisterSingleton<WorldInitializeFeature>();

        // Main entry point
        serviceRegistry.Register<RootFeature>();
    }

    private static void RegisterCreatePlayerEntitySystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<SpriteBatch>().GraphicsDevice;
            SpriteSheet spriteSheet =
                AnimatedCharactersFactory.LoadSpriteSheet(graphicsDevice, Path);

            var idleAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Idle");
            var walkingAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Walking");

            return new MovementAnimationComponent(idleAnimations, walkingAnimations);
        });


        serviceRegistry.Register<CreatePlayerEntitySystem>(new PerContainerLifetime());
    }

    private static void RegisterAnimatedMovementSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            Contexts contexts = factory.GetInstance<Contexts>();
            IAllOfMatcher<GameEntity> animatedMovableMatcher = GameMatcher.AllOf(GameMatcher.Movable,
                GameMatcher.MovementAnimation);
            IGroup<GameEntity> animatedMovableGroup = contexts.game.GetGroup(animatedMovableMatcher);

            var logger = factory.GetInstance<ILogger>();

            return new AnimatedMovementSystem(animatedMovableGroup, logger);
        }, new PerContainerLifetime());
    }

    private static void RegisterCameraFollowingSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(_ => new CameraComponent { Size = new Rectangle(0, 0, 640, 480) });

        serviceRegistry.Register(factory =>
        {
            Contexts contexts = factory.GetInstance<Contexts>();
            IAllOfMatcher<GameEntity> renderMatcher =
                GameMatcher.AllOf(GameMatcher.Transform, GameMatcher.Sprite);
            IGroup<GameEntity> renderGroup = contexts.game.GetGroup(renderMatcher);

            GameEntity target = contexts.game.cameraEntity;

            return new CameraFollowingSystem(target, renderGroup);
        });
    }
}
