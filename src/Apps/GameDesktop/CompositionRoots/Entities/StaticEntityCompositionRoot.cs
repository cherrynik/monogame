using Entities;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

internal class StaticEntityCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }


    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient<StaticEntity>();
}
