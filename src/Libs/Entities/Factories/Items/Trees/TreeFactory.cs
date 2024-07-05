using Components.Data;
using Components.Render.Static;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;
using Services.Math;

namespace Entities.Factories.Items.Trees;

public class TreeFactory(IServiceFactory serviceProvider) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<string, NameComponent>("Tree"));
        e.AddComponent(serviceProvider.GetInstance<TransformComponent>());
        // e.AddComponent(serviceProvider.GetInstance<ItemComponent>(ItemsTable.Items[ItemId.Rock].Name));
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<SpriteComponent>("Tree"));
    }
}
