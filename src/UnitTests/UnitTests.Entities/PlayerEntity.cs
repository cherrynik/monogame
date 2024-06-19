using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Entities.Factories;
using Entities.Factories.Characters;
using Entities.Factories.Items;
using Entities.Factories.Meta;
using Features;
using LDtk;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Services.Factories;
using Services.Movement;
using Systems;

namespace UnitTests.Entities;

public class Tests
{
    private World _world;
    private readonly ServiceContainer _serviceContainer;

    public Tests()
    {
        _world = World.Create();
        _serviceContainer = new(new ContainerOptions
        {
            EnablePropertyInjection = false, EnableCurrentScope = false,
            // LogFactory = _ => entry => logger
            // .ForContext<ServiceContainer>()
            // .Verbose($"{entry.Message}"),
        });

        _serviceContainer.RegisterInstance(_world);

        _serviceContainer.RegisterInstance((IServiceFactory)_serviceContainer);

        _serviceContainer.RegisterSingleton<AbstractEntityFactory>();
        _serviceContainer.RegisterSingleton<RockFactory>();
    }

    [SetUp]
    public void Setup()
    {
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _world.Dispose();
    }

    [Test]
    public void PlayerEntity_IsCreatedInTheWorld()
    {
        Entity playerEntity = new PlayerEntityFactory(
                new NameComponent("Player"),
                new InputMovableComponent(),
                new MovableComponent(),
                new TransformComponent(),
                new CameraComponent(),
                new RectangleColliderComponent(),
                new MovementAnimationsComponent(),
                new CharacterAnimatorComponent(),
                new InventoryComponent())
            .CreateEntity(@in: _world);

        {
            _world.TryGetEntity(playerEntity.ID, out Entity result);

            Assert.That(playerEntity.ID, Is.EqualTo(result.ID));
        }
    }

    [Test]
    public void PlayerEntity_HasComponents()
    {
        Entity playerEntity = new PlayerEntityFactory(
                new NameComponent("Player"),
                new InputMovableComponent(),
                new MovableComponent(),
                new TransformComponent(),
                new CameraComponent(),
                new RectangleColliderComponent(),
                new MovementAnimationsComponent(),
                new CharacterAnimatorComponent(),
                new InventoryComponent())
            .CreateEntity(@in: _world);
        {
            _world.TryGetEntity(playerEntity.ID, out Entity result);

            Assert.Multiple(() =>
            {
                Assert.That(result.Has<InputMovableComponent>(), Is.True);
                Assert.That(result.Has<MovableComponent>(), Is.True);
            });
        }
    }

    [Test]
    [Ignore("Too complicated")]
    public void WorldSystemAndEntityWorkTogether()
    {
        // var mockInputScanner = new Mock<IInputScanner>();
        // mockInputScanner.Setup(p => p.GetDirection()).Returns(Vector2.One);
        //
        // var systemsEngine = new SystemsEngine(_world);
        //
        // var movementFeature = new Feature(_world, systemsEngine, new InputSystem(_world, mockInputScanner.Object),
        //     new MovementSystem(_world, new SimpleMovement()));

        // var mockLdtkLevel = Mock.Of<LDtkLevel>();
        // var mockLdtkWorld = Mock.Of<LDtkWorld>();
        // mockLdtkWorld.Levels = [mockLdtkLevel];
        // mockLdtkWorld.Setup(p => p.LoadLevel(new Guid())).Returns(mockLdtkLevel.Object);

        // var mockLdtkFile = Mock.Of<LDtkFile>();
        // mockLdtkFile.Worlds = [mockLdtkWorld];
        // mockLdtkFile.Setup(p => p.LoadWorld(new Guid())).Returns(mockLdtkWorld.Object);

        // new WorldInitializer(_world,
        //  _serviceContainer.GetInstance<AbstractEntityFactory>(),
        //  mockLdtkFile)

        // var mockWorldInitializer = Mock.Of<WorldInitializer>();
        // var rootFeature = new RootFeature(_world,
        //     systemsEngine,
        //     // mockWorldInitializer
        // );
        //
        // rootFeature.OnAwake();
        // rootFeature.OnFixedUpdate(It.IsAny<float>());
        // rootFeature.OnUpdate(It.IsAny<float>());
        // rootFeature.OnLateUpdate(It.IsAny<float>());
        //
        // mockInputScanner.Verify(p => p.GetDirection(), Times.Once);
    }
}
