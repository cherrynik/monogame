using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Entities.Factories.Characters;
using Entities.Factories.Items;
using Entities.Factories.Meta;
using Features;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Services.Movement;
using Systems;

namespace UnitTests.Entities;

public class Tests
{
    private World _world;

    [SetUp]
    public void Setup()
    {
        _world = World.Create();
    }

    [TearDown]
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
    public void WorldSystemAndEntityWorkTogether()
    {
        var mockInputScanner = new Mock<IInputScanner>();
        mockInputScanner.Setup(p => p.GetDirection()).Returns(Vector2.One);

        var systemsEngine = new SystemsEngine(_world);

        var movementFeature = new Feature(_world, systemsEngine, new InputSystem(_world, mockInputScanner.Object),
            new MovementSystem(_world, new SimpleMovement()));

        var rootFeature = new RootFeature(_world,
            systemsEngine,
            new WorldInitializer(_world, new WorldEntityFactory(new WorldMetaComponent()),
                new PlayerEntityFactory(
                    new NameComponent("Player"), new InputMovableComponent(), new MovableComponent(),
                    new TransformComponent(), new CameraComponent(new Viewport(0, 0, 640, 480)),
                    new RectangleColliderComponent(), new InventoryComponent()),
                new DummyEntityFactory(new NameComponent("Dummy"), new TransformComponent(),
                    new RectangleColliderComponent()),
                new RockEntityFactory(new NameComponent("Rock"), new ItemComponent(ItemId.Rock),
                    new TransformComponent())));

        rootFeature.OnAwake();
        rootFeature.OnFixedUpdate(It.IsAny<float>());
        rootFeature.OnUpdate(It.IsAny<float>());
        rootFeature.OnLateUpdate(It.IsAny<float>());

        mockInputScanner.Verify(p => p.GetDirection(), Times.Once);
    }
}
