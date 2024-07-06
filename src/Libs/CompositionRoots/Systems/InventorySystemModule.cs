using LightInject;
using Scellecs.Morpeh;
using Systems;

namespace CompositionRoots.Systems;

public class InventorySystemModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<InventorySystem>();
    }
}
