using Components.Data;
using Components.Render;
using Components.Tags;
using Constants;
using Entities;
using LightInject;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Sprites;
using Services.Resolvers;
using Services.Math;
using Vector2 = System.Numerics.Vector2;

namespace CompositionRoots.Entities;

public class PlayerModule : ICompositionRoot
{
    private const string AsepriteIdleTag = "Idle";
    private const string AsepriteWalkingTag = "Walking";
    private static readonly Vector2 DefaultPosition = new(316, 116); // Overwritten by Ldtk at the WorldInitializer
    private static readonly Rectangle Collider = new(0, 0, 8, 8);
    private const float MovementSpeed = 7.5f; // If 7.5f -> Math.Ceiling fixes this, else Math.Round
    private const int InventorySlotsCount = 9;

    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new InputMovableTagComponent(), DiContainerNames.Player);
        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = DefaultPosition, Pivot = Sector.Up }, DiContainerNames.Player);
        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(0, 3) }, DiContainerNames.PlayerAnimationsOffset);
        serviceRegistry.RegisterSingleton(_ =>
            new MovableComponent(MovementSpeed));
        RegisterVisuals(serviceRegistry);
        serviceRegistry.RegisterSingleton(_ =>
            new RectangleColliderComponent { Size = Collider }, DiContainerNames.Player);
        serviceRegistry.RegisterTransient(_ =>
        {
            Slot[] slots = new Slot[InventorySlotsCount];

            slots[3].Put(ItemId.Rock, 3);

            return new InventoryComponent(slots);
        });

        serviceRegistry.RegisterSingleton<PlayerFactory>();
    }

    private static void RegisterVisuals(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var getAnimations =
                factory.GetInstance<Func<string, string, Dictionary<Sector, AnimatedSprite>>>(
                    DiContainerNames.Helpers.AsepriteAnimatedCharacterBuilder);

            var path = FileResolver.ResolveFromApp(Contents.SpriteSheets.Player);

            Dictionary<Sector, AnimatedSprite> idle = getAnimations(path, AsepriteIdleTag);
            AnimatedSprite defaultSprite = idle[Sector.Down];

            return new SpriteComponent(defaultSprite, factory.GetInstance<TransformComponent>());
        }, DiContainerNames.Player);

        serviceRegistry.RegisterTransient(factory =>
        {
            var getAnimations =
                factory.GetInstance<Func<string, string, Dictionary<Sector, AnimatedSprite>>>(
                    DiContainerNames.Helpers.AsepriteAnimatedCharacterBuilder);

            var path = FileResolver.ResolveFromApp(Contents.SpriteSheets.Player);

            return new MovementAnimationsComponent(
                getAnimations(path, AsepriteIdleTag),
                getAnimations(path, AsepriteWalkingTag));
        }, DiContainerNames.Player);

        serviceRegistry.RegisterSingleton(factory =>
        {
            var movementAnimations = factory.GetInstance<MovementAnimationsComponent>(DiContainerNames.Player);
            const Sector facing = Sector.Right;
            var transform = factory.GetInstance<TransformComponent>(DiContainerNames.PlayerAnimationsOffset);

            return new CharacterAnimatorComponent(facing, movementAnimations.IdleAnimations[facing], transform);
        }, DiContainerNames.Player);
    }
}
