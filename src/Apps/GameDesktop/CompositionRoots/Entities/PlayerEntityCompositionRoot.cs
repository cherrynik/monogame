using Entities;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

public class PlayerEntityCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }

    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient<PlayerEntity>();
}
