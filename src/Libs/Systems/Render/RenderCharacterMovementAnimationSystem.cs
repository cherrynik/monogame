using Components.Data;
using Components.Events.Movement;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Render;

public class RenderCharacterMovementAnimationSystem : IRenderSystem
{
    public World World { get; set; }
    private readonly SpriteBatch _spriteBatch;
    private Filter _transformFilter = default!;
    private Filter _cameraFilter = default!;
    private Filter _movedEventsFilter = default!;
    private readonly List<Entity> _sortedEntities = [];
    private bool _isSortingDirty = true;
    public RenderCharacterMovementAnimationSystem(World world, SpriteBatch spriteBatch)
    {
        World = world;
        _spriteBatch = spriteBatch;
    }

    public void OnAwake()
    {
        _transformFilter = World.Filter.With<TransformComponent>().Build();
        _cameraFilter = World.Filter.With<CameraComponent>().Build();
        _movedEventsFilter = World.Filter.With<TransformMovedEvent>().Build();
    }

    public void OnUpdate(float deltaTime)
    {
        var transformStash = World.GetStash<TransformComponent>();
        var cameraStash = World.GetStash<CameraComponent>();
        var characterAnimatorStash = World.GetStash<CharacterAnimatorComponent>();
        var spriteStash = World.GetStash<SpriteComponent>();
        if (!TryGetCamera(out var camera, cameraStash))
        {
            return;
        }
        if (_isSortingDirty || HasMovementEvents())
        {
            RebuildSortedEntities(_transformFilter, transformStash, _sortedEntities);
            _isSortingDirty = false;
        }

        foreach (Entity e in _sortedEntities)
        {
            ref var transform = ref transformStash.Get(e);
            var at = transform.Position - camera.Position;

            if (characterAnimatorStash.Has(e))
            {
                ref var animator = ref characterAnimatorStash.Get(e);

                animator.Animation.Draw(_spriteBatch, at);
            }

            if (spriteStash.Has(e))
            {
                ref var sprite = ref spriteStash.Get(e);

                sprite.Sprite.Draw(_spriteBatch, at);
            }
        }
    }

    public void Dispose()
    {
    }

    private bool HasMovementEvents()
    {
        foreach (Entity _ in _movedEventsFilter)
        {
            return true;
        }

        return false;
    }

    private bool TryGetCamera(out CameraComponent camera, Stash<CameraComponent> cameraStash)
    {
        foreach (Entity entity in _cameraFilter)
        {
            camera = cameraStash.Get(entity);
            return true;
        }

        camera = default;
        return false;
    }

    private static void RebuildSortedEntities(Filter filter, Stash<TransformComponent> transformStash, List<Entity> sortedEntities)
    {
        sortedEntities.Clear();

        foreach (Entity e in filter)
        {
            sortedEntities.Add(e);
        }

        sortedEntities.Sort((left, right) =>
        {
            ref var leftTransform = ref transformStash.Get(left);
            ref var rightTransform = ref transformStash.Get(right);
            return leftTransform.Position.Y.CompareTo(rightTransform.Position.Y);
        });
    }
}
