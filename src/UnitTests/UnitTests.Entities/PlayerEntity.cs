using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Entities;
using Entities.Factories;
using Entities.Factories.Characters;
using Scellecs.Morpeh;

namespace UnitTests.Entities;

public class Tests
{
    // TODO: Systems test
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
}
