using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Factories.Characters;

// TODO: Might be good, if we create a base Entity class, and derive from that.
// Which will contain components list, it might be easier then
// to debug entities and their components with ImGui.
// But also, for the factory, ig, the view of the entities class will change.
// For now, Imma keep it as it is.
public class PlayerEntityFactory(
    NameComponent nameComponent,
    InputMovableComponent inputMovable,
    MovableComponent movable,
    TransformComponent transform,
    CameraComponent cameraComponent,
    RectangleColliderComponent rectangleCollider,
    InventoryComponent inventoryComponent)
    : EntityFactory
{
    private readonly MovementAnimationsComponent _movementAnimations;
    private readonly CharacterAnimatorComponent _characterAnimator;

    public PlayerEntityFactory(
        NameComponent nameComponent,
        InputMovableComponent inputMovable,
        MovableComponent movable,
        TransformComponent transform,
        CameraComponent cameraComponent,
        RectangleColliderComponent rectangleCollider,
        MovementAnimationsComponent movementAnimations,
        CharacterAnimatorComponent characterAnimator,
        InventoryComponent inventoryComponent) : this(nameComponent, inputMovable, movable, transform, cameraComponent,
        rectangleCollider, inventoryComponent)
    {
        _movementAnimations = movementAnimations;
        _characterAnimator = characterAnimator;
    }

    protected override void AddTags(Entity e)
    {
        e.AddComponent(cameraComponent);
        e.AddComponent(inputMovable);
        e.AddComponent(movable);
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(nameComponent);
        e.AddComponent(transform);
        e.AddComponent(rectangleCollider);
        e.AddComponent(inventoryComponent);
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(_movementAnimations);
        e.AddComponent(_characterAnimator);
    }
}
