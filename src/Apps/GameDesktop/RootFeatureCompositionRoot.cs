using Components.Sprites;
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
using Stateless;
using Systems;

namespace GameDesktop;

public class RootFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<IInputScanner, KeyboardScanner>(new PerContainerLifetime());

        serviceRegistry.Register<IMovement, SimpleMovement>(new PerContainerLifetime());

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

        serviceRegistry.Register(factory =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<SpriteBatch>().GraphicsDevice;
            SpriteSheet spriteSheet =
                AnimatedCharactersFactory.LoadSpriteSheet(graphicsDevice, "Content/SpriteSheets/Player.aseprite");

            var idleDirAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Idle");
            var walkingDirAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Walking");

            return new AnimatedMovementComponent(
                new StateMachine<PlayerState, PlayerTrigger>(PlayerState.Idle),
                idleDirAnimations,
                walkingDirAnimations);
        });

        serviceRegistry.Register(factory => new CreatePlayerEntitySystem(factory.GetInstance<Contexts>(),
            factory.GetInstance<AnimatedMovementComponent>()));

        serviceRegistry.Register(factory =>
        {
            Contexts contexts = factory.GetInstance<Contexts>();
            IAllOfMatcher<GameEntity> animatedMovableMatcher = GameMatcher.AllOf(GameMatcher.Movable,
                GameMatcher.AnimatedMovement);
            IGroup<GameEntity> animatedMovableGroup = contexts.game.GetGroup(animatedMovableMatcher);

            var logger = factory.GetInstance<ILogger>();

            return new AnimatedMovementSystem(animatedMovableGroup, logger);
        });

        serviceRegistry.Register<RootFeature>();
    }
}
