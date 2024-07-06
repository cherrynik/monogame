using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Constants;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Factories.Characters;

public class PlayerFactory(IServiceFactory serviceFactory) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<CameraComponent>());
        e.AddComponent(serviceFactory.GetInstance<InputMovableComponent>());
        e.AddComponent(serviceFactory.GetInstance<MovableComponent>());
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<string, NameComponent>(DINames.Player));
        e.AddComponent(serviceFactory.GetInstance<TransformComponent>(DINames.Player));
        e.AddComponent(serviceFactory.GetInstance<RectangleColliderComponent>(DINames.Player));
        e.AddComponent(serviceFactory.GetInstance<InventoryComponent>());
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<MovementAnimationsComponent>(DINames.Player));
        e.AddComponent(serviceFactory.GetInstance<CharacterAnimatorComponent>(DINames.Player));
    }
}
