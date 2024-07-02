using System;
using System.Collections.Generic;
using System.IO;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using GameDesktop.Resources.Internal;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using Services.Math;


[assembly: CompositionRootType(typeof(GameDesktop.CompositionRoots.Components.ComponentsCompositionRoot))]

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
        serviceRegistry.Register<string, Texture2D>((factory, path) =>
        {
            var fileName = Path.Join(
                Environment.GetEnvironmentVariable(EnvironmentVariable.AppBaseDirectory),
                path
            );
            return Texture2D.FromFile(factory.GetInstance<GraphicsDeviceManager>().GraphicsDevice, fileName);
        });

        // TODO: Automatically get texture & its rect from a tile set (by aseprite?)
        serviceRegistry.RegisterSingleton(factory =>
        {
            var texture = factory.GetInstance<string, Texture2D>("Content/SpriteSheets/Main.png");
            var sprite = new Sprite("Pebble", new TextureRegion("Pebble", texture, new(208, 48, 16, 16)));

            return new SpriteComponent(sprite, factory.GetInstance<TransformComponent>());
        }, "Pebble");

        serviceRegistry.RegisterSingleton(factory =>
        {
            var texture = factory.GetInstance<string, Texture2D>("Content/SpriteSheets/Main.png");
            var sprite = new Sprite("Tree", new TextureRegion("Tree", texture, new(144, 0, 48, 96)));

            return new SpriteComponent(sprite, factory.GetInstance<TransformComponent>());
        }, "Tree");
        RegisterTransformComponent(serviceRegistry);
        RegisterRectangleColliderComponent(serviceRegistry);
        RegisterItemComponent(serviceRegistry);
        RegisterInventoryComponent(serviceRegistry);
    }

    private static void RegisterItemComponent(IServiceRegistry serviceRegistry)
    {
        // TODO: make this accessible globally? So, the name, etc. of an item are reused between classes easily
        // Dictionary<ItemId, Item> items = new()
        //  {
        //      { ItemId.Rock, new Item(name: "Rock", isStackable: true, maximumInStack: 16) }
        //  };

        serviceRegistry.RegisterSingleton(_ => new ItemComponent(ItemId.Rock), ItemsTable.Items[ItemId.Rock].Name);
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
        serviceRegistry.RegisterSingleton(_ => new Viewport(0, 0, 801, 480), "CameraComponent");
        serviceRegistry.RegisterSingleton(factory =>
            new CameraComponent(factory.GetInstance<Viewport>("CameraComponent")));
    }

    private static void RegisterPlayerMovementComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new InputMovableComponent());
        serviceRegistry.RegisterSingleton(_ => new MovableComponent(5f));
    }

    private static void RegisterSpriteComponent(IServiceRegistry serviceRegistry)
    {
        // todo: put non-game strings in internal resources
        serviceRegistry.RegisterSingleton(factory =>
        {
            var getAnimations =
                factory.GetInstance<Func<string, string, Dictionary<Sector, AnimatedSprite>>>("Character");
            Dictionary<Sector, AnimatedSprite> idle = getAnimations(PlayerSpriteSheetPath, "Idle");
            AnimatedSprite defaultSprite = idle[Sector.Down];

            return new SpriteComponent(defaultSprite, factory.GetInstance<TransformComponent>());
        }, "Player");

        // serviceRegistry.RegisterSingleton(factory =>
        // {
        // TODO: add the sprite of a rock
        // })
    }

    private static void RegisterMovementAnimationsComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient(factory =>
        {
            var getAnimations =
                factory.GetInstance<Func<string, string, Dictionary<Sector, AnimatedSprite>>>("Character");

            return new MovementAnimationsComponent(getAnimations(PlayerSpriteSheetPath, "Idle"),
                getAnimations(PlayerSpriteSheetPath, "Walking"));
        }, "PlayerEntity");
    }


    // private static void RegisterCameraComponent(IServiceRegistry serviceRegistry) =>
    // serviceRegistry.RegisterSingleton(_ => new CameraComponent { Size = new Rectangle(0, 0, 640, 480) });

    private static void RegisterTransformComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<string, NameComponent>((factory, name) => new NameComponent(name));
        serviceRegistry.RegisterTransient(_ => new TransformComponent());

        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(0, 4) }, "PlayerAnimations");

        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(316, 116), Pivot = Sector.Up }, "PlayerEntity");

        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(300, 100) }, "DummyEntity");

        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(180, 50) }, "RockEntity");
    }

    private static void RegisterRectangleColliderComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ =>
            new RectangleColliderComponent { Size = new(0, 0, 8, 8), IsTrigger = true }, "DummyEntity");

        serviceRegistry.RegisterSingleton(_ =>
            new RectangleColliderComponent { Size = new(0, 0, 8, 8) }, "PlayerEntity");
    }

    private static void RegisterInventoryComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient(_ =>
        {
            const int count = 9;
            Slot[] slots = new Slot[count];

            // Put items in slots like that:
            slots[3].Item = ItemId.Rock;
            slots[3].Amount = 3;

            return new InventoryComponent(slots);
        });
    }

    private static void RegisterCharacterAnimatorComponent(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var movementAnimations = factory.GetInstance<MovementAnimationsComponent>("PlayerEntity");
            const Sector facing = Sector.Right;

            return new CharacterAnimatorComponent(facing, movementAnimations.IdleAnimations[facing],
                factory.GetInstance<TransformComponent>("PlayerAnimations"));
        }, "PlayerEntity");
    }
}
