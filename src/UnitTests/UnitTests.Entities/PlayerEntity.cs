using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Entities;
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
        Entity playerEntity = new PlayerEntity(new PlayerComponent(),
                new MovableComponent(),
                new TransformComponent(),
                new RectangleCollisionComponent(),
                new MovementAnimationsComponent(),
                new CharacterAnimatorComponent())
            .Create(@in: _world);

        {
            _world.TryGetEntity(playerEntity.ID, out Entity result);

            Assert.That(playerEntity.ID, Is.EqualTo(result.ID));
        }
    }

    [Test]
    public void PlayerEntity_HasComponents()
    {
        Entity playerEntity = new PlayerEntity(new PlayerComponent(),
                new MovableComponent(),
                new TransformComponent(),
                new RectangleCollisionComponent(),
                new MovementAnimationsComponent(),
                new CharacterAnimatorComponent())
            .Create(@in: _world);

        {
            _world.TryGetEntity(playerEntity.ID, out Entity result);

            Assert.That(result.Has<PlayerComponent>(), Is.True);
            Assert.That(result.Has<MovableComponent>(), Is.True);
            // ...
        }
    }
}
