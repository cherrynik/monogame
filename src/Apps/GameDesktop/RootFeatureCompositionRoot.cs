﻿using Components;
using Components.World;
using Entitas;
using Features;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Serilog;
using Services;
using Services.Factories;
using Services.Input;
using Services.Math;
using Services.Movement;
using Systems;

namespace GameDesktop;

public class RootFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterServices(serviceRegistry);
        RegisterInputSystem(serviceRegistry);
        RegisterMovementSystem(serviceRegistry);
        RegisterCreatePlayerEntitySystem(serviceRegistry);
        RegisterAnimatedMovementSystem(serviceRegistry);
        RegisterCameraFollowingSystem(serviceRegistry);
        RegisterCreateStaticEntitySystem(serviceRegistry);

        serviceRegistry.Register<RootFeature>();
    }

    private static void RegisterServices(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<IInputScanner, KeyboardScanner>(new PerContainerLifetime());

        serviceRegistry.Register<IMovement, SimpleMovement>(new PerContainerLifetime());
    }

    private static void RegisterInputSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            Contexts contexts = factory.GetInstance<Contexts>();
            IAllOfMatcher<GameEntity> inputMovableMatcher = GameMatcher.AllOf(GameMatcher.Transform,
                GameMatcher.Movable,
                GameMatcher.Player);
            IGroup<GameEntity> inputMovableGroup = contexts.game.GetGroup(inputMovableMatcher);

            var inputScanner = factory.GetInstance<IInputScanner>();
            var logger = factory.GetInstance<ILogger>();

            return new InputSystem(inputScanner, inputMovableGroup, logger);
        }, new PerContainerLifetime());
    }

    private static void RegisterMovementSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            Contexts contexts = factory.GetInstance<Contexts>();
            IAllOfMatcher<GameEntity> movableMatcher = GameMatcher.AllOf(GameMatcher.Transform,
                GameMatcher.Movable);
            IGroup<GameEntity> movableGroup = contexts.game.GetGroup(movableMatcher);

            var movement = factory.GetInstance<IMovement>();
            var logger = factory.GetInstance<ILogger>();

            return new MovementSystem(movement, movableGroup, logger);
        }, new PerContainerLifetime());
    }

    private static void RegisterCreatePlayerEntitySystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<SpriteBatch>().GraphicsDevice;
            SpriteSheet spriteSheet =
                AnimatedCharactersFactory.LoadSpriteSheet(graphicsDevice, "Content/SpriteSheets/Player.aseprite");

            var idleAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Idle");
            var walkingAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Walking");

            return new MovementAnimationComponent(idleAnimations, walkingAnimations);
        });

        serviceRegistry.Register<TransformComponent>();

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

    private void RegisterCreateStaticEntitySystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<SpriteBatch>().GraphicsDevice;
            SpriteSheet spriteSheet =
                AnimatedCharactersFactory.LoadSpriteSheet(graphicsDevice, "Content/SpriteSheets/Player.aseprite");

            AnimatedSprite animatedSprite =
                AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Idle")[Direction.Down];
            return new SpriteComponent(animatedSprite);
        });

        serviceRegistry.Register(factory =>
        {
            var transform = factory.GetInstance<TransformComponent>();
            transform.Position = new Vector2(143, 85);

            return new CreateStaticEntitySystem(
                factory.GetInstance<Contexts>(),
                transform,
                factory.GetInstance<SpriteComponent>());
        });
    }
}
