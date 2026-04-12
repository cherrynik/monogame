using System.Numerics;
using Components.Data;
using Components.Events.Movement;
using Components.Tags;
using Moq;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Services;
using Systems;
using Systems.Input.Abstractions;

namespace UnitTests.Entities;

public class SystemBehaviorTests
{
    private World _world = default!;

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
    public void InputSystem_UpdatesVelocity_ForInputMovableEntity()
    {
        var inputScanner = new Mock<IInputScanner>();
        inputScanner.Setup(x => x.GetDirection()).Returns(new Vector2(1f, -1f));
        var system = new InputSystem(_world, inputScanner.Object);
        system.OnAwake();

        Entity entity = _world.CreateEntity();
        entity.AddComponent(new InputMovableComponent());
        entity.AddComponent(new TransformComponent());
        _world.Commit();

        system.OnUpdate(0.016f);

        var transformStash = _world.GetStash<TransformComponent>();
        ref var transform = ref transformStash.Get(entity);
        Assert.That(transform.Velocity, Is.EqualTo(new Vector2(1f, -1f)));
        inputScanner.Verify(x => x.GetDirection(), Times.Once);
        system.Dispose();
    }

    [Test]
    public void MovementSystem_UpdatesPosition_UsingMovementService()
    {
        var movement = new Mock<IMovement>();
        movement.Setup(x => x.Move(new Vector2(10f, 5f), new Vector2(2f, 0f)))
            .Returns(new Vector2(11f, 5f));
        var system = new MovementSystem(_world, movement.Object);
        system.OnAwake();

        Entity entity = _world.CreateEntity();
        entity.AddComponent(new InputMovableComponent());
        entity.AddComponent(new TransformComponent
        {
            Position = new Vector2(10f, 5f),
            Velocity = new Vector2(2f, 0f)
        });
        _world.Commit();

        system.OnUpdate(0.016f);

        var transformStash = _world.GetStash<TransformComponent>();
        var movedEventStash = _world.GetStash<TransformMovedEvent>();
        ref var transform = ref transformStash.Get(entity);
        Assert.That(transform.Position, Is.EqualTo(new Vector2(11f, 5f)));
        Assert.That(movedEventStash.Has(entity), Is.True);
        movement.Verify(x => x.Move(new Vector2(10f, 5f), new Vector2(2f, 0f)), Times.Once);
        system.Dispose();
    }

    [Test]
    public void MovementSystem_ClearsTransformMovedEvent_WhenEntityStopsMoving()
    {
        var movement = new Mock<IMovement>();
        movement.Setup(x => x.Move(new Vector2(2f, 2f), new Vector2(1f, 0f)))
            .Returns(new Vector2(3f, 2f));
        movement.Setup(x => x.Move(new Vector2(3f, 2f), Vector2.Zero))
            .Returns(new Vector2(3f, 2f));
        var system = new MovementSystem(_world, movement.Object);
        system.OnAwake();

        Entity entity = _world.CreateEntity();
        entity.AddComponent(new InputMovableComponent());
        entity.AddComponent(new TransformComponent
        {
            Position = new Vector2(2f, 2f),
            Velocity = new Vector2(1f, 0f)
        });
        _world.Commit();

        system.OnUpdate(0.016f);
        _world.Commit();
        var transformStash = _world.GetStash<TransformComponent>();
        var movedEventStash = _world.GetStash<TransformMovedEvent>();
        Assert.That(movedEventStash.Has(entity), Is.True);

        ref var transform = ref transformStash.Get(entity);
        transform.Velocity = Vector2.Zero;
        system.OnUpdate(0.016f);
        _world.Commit();

        Assert.That(movedEventStash.Has(entity), Is.False);
        system.Dispose();
    }
}
