using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Entities.Factories.Characters;

// TODO: Might be good, if we create a base Entity class, and derive from that.
// Which will contain components list, it might be easier then
// to debug entities and their components with ImGui.
// But also, for the factory, ig, the view of the entities class will change.
// For now, Imma keep it as it is.
public class PlayerEntityFactory : EntityFactory
{
    private readonly InputMovableComponent _inputMovable;
    private readonly MovableComponent _movable;
    private readonly TransformComponent _transform;
    private readonly CameraComponent _cameraComponent;
    private readonly RectangleCollisionComponent _rectangleCollision;
    private readonly MovementAnimationsComponent _movementAnimations;
    private readonly CharacterAnimatorComponent _characterAnimator;

    public PlayerEntityFactory(InputMovableComponent inputMovable,
        MovableComponent movable,
        TransformComponent transform,
        CameraComponent cameraComponent,
        RectangleCollisionComponent rectangleCollision
    )
    {
        _inputMovable = inputMovable;
        _movable = movable;
        _transform = transform;
        _cameraComponent = cameraComponent;
        _rectangleCollision = rectangleCollision;
    }

    public PlayerEntityFactory(InputMovableComponent inputMovable,
        MovableComponent movable,
        TransformComponent transform,
        CameraComponent cameraComponent,
        RectangleCollisionComponent rectangleCollision,
        MovementAnimationsComponent movementAnimations,
        CharacterAnimatorComponent characterAnimator) : this(inputMovable, movable, transform, cameraComponent,
        rectangleCollision)
    {
        _movementAnimations = movementAnimations;
        _characterAnimator = characterAnimator;
    }

    protected override void AddTags(Entity e)
    {
        e.AddComponent(_cameraComponent);
        e.AddComponent(_inputMovable);
        e.AddComponent(_movable);
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(_transform);
        e.AddComponent(_rectangleCollision);
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(_movementAnimations);
        e.AddComponent(_characterAnimator);
    }
}
