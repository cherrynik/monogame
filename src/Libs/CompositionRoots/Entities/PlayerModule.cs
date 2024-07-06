using Components.Data;
using Components.Render;
using Components.Tags;
using Constants;
using Entities.Characters;
using LightInject;
using MonoGame.Aseprite.Sprites;
using Services.Resolvers;
using Services.Math;

namespace CompositionRoots.Entities;

public class PlayerModule : ICompositionRoot
{
    private const string AsepriteIdleTag = "Idle";
    private const string AsepriteWalkingTag = "Walking";

    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new InputMovableTagComponent(), DiContainerNames.Player);
        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(316, 116), Pivot = Sector.Up }, DiContainerNames.Player);
        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(0, 3) }, DiContainerNames.PlayerAnimationsOffset);
        serviceRegistry.RegisterSingleton(_ =>
            new MovableComponent(7f)); // If 7.5f -> Math.Ceiling fixes this, else Math.Round
        RegisterVisuals(serviceRegistry);
        serviceRegistry.RegisterSingleton(_ =>
            new RectangleColliderComponent { Size = new(0, 0, 8, 8) }, DiContainerNames.Player);
        serviceRegistry.RegisterTransient(_ =>
        {
            const int count = 9;
            Slot[] slots = new Slot[count];

            // Put items in slots like that:
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
