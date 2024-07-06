using Components.Data;
using Components.Render;
using Constants;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Items.Rocks;

public class PebbleFactory(IServiceFactory serviceProvider) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<string, NameComponent>(DiContainerNames.Pebble));
        e.AddComponent(serviceProvider.GetInstance<TransformComponent>());
        e.AddComponent(serviceProvider.GetInstance<ItemComponent>(ItemsTable.Items[ItemId.Rock].Name));
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<SpriteComponent>(DiContainerNames.Pebble));
    }
}
