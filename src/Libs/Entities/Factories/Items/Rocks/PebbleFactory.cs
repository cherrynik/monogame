using Components.Data;
using Components.Render.Static;
using Constants;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Factories.Items.Rocks;

public class PebbleFactory(IServiceFactory serviceProvider) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<string, NameComponent>(DINames.Pebble));
        e.AddComponent(serviceProvider.GetInstance<TransformComponent>());
        e.AddComponent(serviceProvider.GetInstance<ItemComponent>(ItemsTable.Items[ItemId.Rock].Name));
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<SpriteComponent>(DINames.Pebble));
    }
}
