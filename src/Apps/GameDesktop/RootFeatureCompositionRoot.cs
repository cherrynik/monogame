using Components;
using Components.World;
using Entitas;
using Features;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Serilog;
using Services;
using Services.Factories;
using Services.Input;
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
}
