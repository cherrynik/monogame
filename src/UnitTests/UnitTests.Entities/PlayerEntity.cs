using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Entities.Factories.Characters;
using Entities.Factories.Meta;
using Features;
using Implementations;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Services.Movement;
using Systems;

namespace UnitTests.Entities;

public class MockKeyboardInput : IInputScanner
{
    public Vector2 GetDirection() => new(1, 1);
}

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
        // TODO: Use mocks for deps
        Entity playerEntity = new PlayerEntityFactory(new InputMovableComponent(),
                new MovableComponent(),
                new TransformComponent(),
                new CameraComponent(),
                new RectangleCollisionComponent(),
                new MovementAnimationsComponent(),
                new CharacterAnimatorComponent())
            .CreateEntity(@in: _world);

        {
            _world.TryGetEntity(playerEntity.ID, out Entity result);

            Assert.That(playerEntity.ID, Is.EqualTo(result.ID));
        }
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
                new CharacterAnimatorComponent())
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
        var rootFeature = new RootFeature(_world,
            new WorldInitializer(_world, new WorldEntityFactory(new WorldComponent()),
                new PlayerEntityFactory(new InputMovableComponent(), new MovableComponent(), new TransformComponent(),
                    new CameraComponent(new Viewport(0, 0, 640, 480)), new RectangleCollisionComponent()),
                new DummyEntityFactory(new TransformComponent(), new RectangleCollisionComponent())),
            new MovementFeature(_world, new InputSystem(_world, new MockKeyboardInput()),
                new MovementSystem(_world, new SimpleMovement())));

        rootFeature.OnAwake();

        var player = _world.Filter.With<InputMovableComponent>().Build().First();
        ref var transform = ref player.GetComponent<TransformComponent>();

        Assert.That(transform.Velocity.X, Is.EqualTo(0));
        Assert.That(transform.Position.Y, Is.EqualTo(0));

        rootFeature.OnUpdate(.0f);

        Assert.That(transform.Velocity.X, Is.EqualTo(1));
        // Assert.That(transform.Position.Y, Is.EqualTo(0));
    }
}
