using Components.Data;
using Components.Render;
using Constants;
using Entities;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Items.Trees;

public class TreeFactory(IServiceFactory serviceProvider) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<string, NameComponent>(DiContainerNames.Tree));
        e.AddComponent(serviceProvider.GetInstance<TransformComponent>());
        // e.AddComponent(serviceProvider.GetInstance<ItemComponent>(ItemsTable.Items[ItemId.Rock].Name));
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<SpriteComponent>(DiContainerNames.Tree));
    }
}
