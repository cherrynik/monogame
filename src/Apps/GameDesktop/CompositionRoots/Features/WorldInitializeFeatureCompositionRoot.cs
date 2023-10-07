using Features;
using LightInject;

namespace GameDesktop.CompositionRoots.Features;

public class WorldInitializeFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterFeature(serviceRegistry);
    }

    private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<WorldInitializeFeature>();
}
