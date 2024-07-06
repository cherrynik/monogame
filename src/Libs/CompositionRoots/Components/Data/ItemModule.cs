using Components.Data;
using LightInject;

namespace CompositionRoots.Components.Data;

public class ItemModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new ItemComponent(ItemId.Rock), ItemsTable.Items[ItemId.Rock].Name);
    }
}
