using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Constants;
using Entities.Factories.Characters;
using LightInject;
using MonoGame.Aseprite.Sprites;
using Services.Helpers;
using Services.Implementations.Math;

namespace CompositionRoots.Entities;

public class PlayerModule : ICompositionRoot
{
    private const string AsepriteIdleTag = "Idle";
    private const string AsepriteWalkingTag = "Walking";

    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new InputMovableComponent(), DINames.Player);
        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(316, 116), Pivot = Sector.Up }, DINames.Player);
        serviceRegistry.RegisterSingleton(_ =>
            new TransformComponent { Position = new(0, 3) }, DINames.PlayerAnimationsOffset);
        serviceRegistry.RegisterSingleton(_ =>
            new MovableComponent(7f)); // If 7.5f -> Math.Ceiling fixes this, else Math.Round
        RegisterVisuals(serviceRegistry);
        serviceRegistry.RegisterSingleton(_ =>
            new RectangleColliderComponent { Size = new(0, 0, 8, 8) }, DINames.Player);
        serviceRegistry.RegisterTransient(_ =>
        {
            const int count = 9;
            Slot[] slots = new Slot[count];

            // Put items in slots like that:
            slots[3].Item = ItemId.Rock;
            slots[3].Amount = 3;

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
                    DINames.Helpers.AsepriteAnimatedCharacterBuilder);

            var path = FileResolver.ResolveFromApp(Contents.SpriteSheets.Player);

            Dictionary<Sector, AnimatedSprite> idle = getAnimations(path, AsepriteIdleTag);
            AnimatedSprite defaultSprite = idle[Sector.Down];

            return new SpriteComponent(defaultSprite, factory.GetInstance<TransformComponent>());
        }, DINames.Player);

        serviceRegistry.RegisterTransient(factory =>
        {
            var getAnimations =
                factory.GetInstance<Func<string, string, Dictionary<Sector, AnimatedSprite>>>(
                    DINames.Helpers.AsepriteAnimatedCharacterBuilder);

            var path = FileResolver.ResolveFromApp(Contents.SpriteSheets.Player);

            return new MovementAnimationsComponent(
                getAnimations(path, AsepriteIdleTag),
                getAnimations(path, AsepriteWalkingTag));
        }, DINames.Player);

        serviceRegistry.RegisterSingleton(factory =>
        {
            var movementAnimations = factory.GetInstance<MovementAnimationsComponent>(DINames.Player);
            const Sector facing = Sector.Right;
            var transform = factory.GetInstance<TransformComponent>(DINames.PlayerAnimationsOffset);

            return new CharacterAnimatorComponent(facing, movementAnimations.IdleAnimations[facing], transform);
        }, DINames.Player);
    }
}
