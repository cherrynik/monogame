using Components.Data;
using Components.Render;
using Components.Tags;
using Constants;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Characters;

public class PlayerFactory(IServiceFactory serviceFactory) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<CameraComponent>());
        e.AddComponent(serviceFactory.GetInstance<InputMovableTagComponent>());
        e.AddComponent(serviceFactory.GetInstance<MovableComponent>());
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<string, NameComponent>(DiContainerNames.Player));
        e.AddComponent(serviceFactory.GetInstance<TransformComponent>(DiContainerNames.Player));
        e.AddComponent(serviceFactory.GetInstance<RectangleColliderComponent>(DiContainerNames.Player));
        e.AddComponent(serviceFactory.GetInstance<InventoryComponent>());
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<MovementAnimationsComponent>(DiContainerNames.Player));
        e.AddComponent(serviceFactory.GetInstance<CharacterAnimatorComponent>(DiContainerNames.Player));
    }
}
