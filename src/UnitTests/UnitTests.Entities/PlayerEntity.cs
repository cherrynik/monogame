using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Entities.Factories.Characters;
using Entities.Factories.Meta;
using Features;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using Scellecs.Morpeh;
using Services.Movement;
using Systems;
using Systems.Input.Abstractions;

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
        Entity playerEntity = new PlayerEntityFactory(new InputMovableComponent(),
                new MovableComponent(),
                new TransformComponent(),
                new CameraComponent(),
                new RectangleCollisionComponent(),
                new MovementAnimationsComponent(),
                new CharacterAnimatorComponent(),
                new InventoryComponent())
            .CreateEntity(@in: _world);

        Assert.That(playerEntity, Is.Not.EqualTo(default(Entity)));
    }

    [Test]
    public void PlayerEntity_HasComponents()
    {
        Entity playerEntity = new PlayerEntityFactory(new InputMovableComponent(),
                new MovableComponent(),
                new TransformComponent(),
                new CameraComponent(),
                new RectangleCollisionComponent(),
                new MovementAnimationsComponent(),
                new CharacterAnimatorComponent(),
                new InventoryComponent())
            .CreateEntity(@in: _world);
        var inputMovableStash = _world.GetStash<InputMovableComponent>();
        var movableStash = _world.GetStash<MovableComponent>();
        Assert.Multiple(() =>
        {
            Assert.That(inputMovableStash.Has(playerEntity), Is.True);
            Assert.That(movableStash.Has(playerEntity), Is.True);
        });
    }

    [Test]
    public void WorldSystemAndEntityWorkTogether()
    {
        var mockInputScanner = new Mock<IInputScanner>();
        mockInputScanner.Setup(p => p.GetDirection()).Returns(Vector2.One);

        var rootFeature = new RootFeature(_world,
            new WorldInitializer(_world, new WorldEntityFactory(new WorldComponent()),
                new PlayerEntityFactory(new InputMovableComponent(), new MovableComponent(), new TransformComponent(),
                    new CameraComponent(new Viewport(0, 0, 640, 480)), new RectangleCollisionComponent(),
                    new InventoryComponent()),
                new DummyEntityFactory(new TransformComponent(), new RectangleCollisionComponent())),
            new MovementFeature(_world, new InputSystem(_world, mockInputScanner.Object),
                new MovementSystem(_world, new SimpleMovement())));

        rootFeature.OnAwake();

        rootFeature.OnUpdate(It.IsAny<float>());

        mockInputScanner.Verify(p => p.GetDirection(), Times.Once);
    }
}
