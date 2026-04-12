using System.Numerics;
using Components.Data;
using Components.Events.Movement;
using Components.Tags;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems.Render;

namespace UnitTests.Entities;

public class CameraFollowingSystemTests
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
    public void OnUpdate_DoesNotThrow_WhenCameraEntityHasNoTransform()
    {
        var system = new CameraFollowingSystem(_world);
        system.OnAwake();
        Entity cameraEntity = _world.CreateEntity();
        cameraEntity.AddComponent(new CameraComponent(new Viewport(0, 0, 800, 480)));

        Assert.DoesNotThrow(() => system.OnUpdate(0.016f));
        system.Dispose();
    }

    [Test]
    public void OnUpdate_UpdatesCameraPosition_FromTransformAndViewport()
    {
        Entity cameraEntity = _world.CreateEntity();
        cameraEntity.AddComponent(new CameraComponent(new Viewport(0, 0, 800, 480)));
        cameraEntity.AddComponent(new TransformComponent { Position = new Vector2(316f, 116f) });
        var system = new CameraFollowingSystem(_world);
        system.OnAwake();
        var transformStash = _world.GetStash<TransformComponent>();
        var cameraStash = _world.GetStash<CameraComponent>();

        Assert.That(transformStash.Has(cameraEntity), Is.True);
        Assert.That(cameraStash.Has(cameraEntity), Is.True);
        _world.Commit();

        system.OnUpdate(0.016f);

        ref var camera = ref cameraStash.Get(cameraEntity);

        Assert.That(camera.Position.X, Is.EqualTo(-84f));
        Assert.That(camera.Position.Y, Is.EqualTo(-124f));
        system.Dispose();
    }

    [Test]
    public void OnUpdate_SkipsRecenter_WhenNoMovementEventAfterInitialization()
    {
        Entity cameraEntity = _world.CreateEntity();
        cameraEntity.AddComponent(new CameraComponent(new Viewport(0, 0, 800, 480)));
        cameraEntity.AddComponent(new TransformComponent { Position = new Vector2(300f, 100f) });
        cameraEntity.AddComponent(new TransformMovedEvent());
        _world.Commit();

        var system = new CameraFollowingSystem(_world);
        system.OnAwake();
        var cameraStash = _world.GetStash<CameraComponent>();
        var transformStash = _world.GetStash<TransformComponent>();
        var movedEventStash = _world.GetStash<TransformMovedEvent>();

        system.OnUpdate(0.016f);
        _world.Commit();
        movedEventStash.Remove(cameraEntity);
        _world.Commit();
        ref var transform = ref transformStash.Get(cameraEntity);
        transform.Position = new Vector2(500f, 350f);

        system.OnUpdate(0.016f);
        _world.Commit();
        ref var camera = ref cameraStash.Get(cameraEntity);

        Assert.That(camera.Position.X, Is.EqualTo(-100f));
        Assert.That(camera.Position.Y, Is.EqualTo(-140f));
        system.Dispose();
    }
}
