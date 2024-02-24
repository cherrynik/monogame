using System;
using System.Collections.Generic;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using GameDesktop.Resources.Internal;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services.Math;

namespace GameDesktop.CompositionRoots.Components;

internal class ComponentsCompositionRoot : ICompositionRoot
{
    private static readonly string PlayerSpriteSheetPath = System.IO.Path.Join(
        Environment.GetEnvironmentVariable(EnvironmentVariable.AppBaseDirectory),
        Resources.SpriteSheet.Player);

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterTagComponents(serviceRegistry);
        RegisterDataComponents(serviceRegistry);
        RegisterStaticRenderComponents(serviceRegistry);
        RegisterAnimatedRenderComponents(serviceRegistry);

        // Removed camera as it's a system, existing globally and singly
    }

    private static void RegisterDataComponents(IServiceRegistry serviceRegistry)
    {
        RegisterTransformComponent(serviceRegistry);
        RegisterRectangleCollisionComponent(serviceRegistry);
        RegisterInventoryComponent(serviceRegistry);
    }

    private static void RegisterTagComponents(IServiceRegistry serviceRegistry)
    {
        RegisterCameraComponent(serviceRegistry);
        RegisterPlayerMovementComponent(serviceRegistry);
    }


    private static void RegisterStaticRenderComponents(IServiceRegistry serviceRegistry)
    {
        RegisterSpriteComponent(serviceRegistry);
    }

    private static void RegisterAnimatedRenderComponents(IServiceRegistry serviceRegistry)
    {
        RegisterMovementAnimationsComponent(serviceRegistry);
        RegisterCharacterAnimatorComponent(serviceRegistry);
    }

    private static void RegisterCameraComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new Viewport(0, 0, 800, 480), "CameraComponent");
        serviceRegistry.RegisterSingleton(factory =>
            new CameraComponent(factory.GetInstance<Viewport>("CameraComponent")));
    }

    private static void RegisterPlayerMovementComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new InputMovableComponent());
        serviceRegistry.RegisterSingleton(_ => new MovableComponent());
    }

    private static void RegisterSpriteComponent(IServiceRegistry serviceRegistry)
    {
        // todo: put non-game strings in internal resources
        serviceRegistry.RegisterSingleton(factory =>
        {
            var getAnimations =
                factory.GetInstance<Func<string, string, Dictionary<Direction, AnimatedSprite>>>("Character");
            Dictionary<Direction, AnimatedSprite> idle = getAnimations(PlayerSpriteSheetPath, "Idle");
            AnimatedSprite defaultSprite = idle[Direction.Down];

            return new SpriteComponent(defaultSprite);
        });
    }

    private static void RegisterMovementAnimationsComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient(factory =>
        {
            var getAnimations =
                factory.GetInstance<Func<string, string, Dictionary<Direction, AnimatedSprite>>>("Character");

            return new MovementAnimationsComponent(getAnimations(PlayerSpriteSheetPath, "Idle"),
                getAnimations(PlayerSpriteSheetPath, "Walking"));
        }, "PlayerEntity");
    }


    // private static void RegisterCameraComponent(IServiceRegistry serviceRegistry) =>
    // serviceRegistry.RegisterSingleton(_ => new CameraComponent { Size = new Rectangle(0, 0, 640, 480) });

    private static void RegisterTransformComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(316, 116) }, "PlayerEntity");

        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(300, 100) }, "DummyEntity");
    }

    private static void RegisterRectangleCollisionComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient(_ =>
            new RectangleCollisionComponent { Size = new(0, 0, 8, 8) });
    }

    private static void RegisterInventoryComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient(_ =>
            new InventoryComponent([]));
    }

    private static void RegisterCharacterAnimatorComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var movementAnimations = factory.GetInstance<MovementAnimationsComponent>("PlayerEntity");
            const Direction facing = Direction.Right;

            return new CharacterAnimatorComponent(facing, movementAnimations.IdleAnimations[facing]);
        }, "PlayerEntity");
    }
}
