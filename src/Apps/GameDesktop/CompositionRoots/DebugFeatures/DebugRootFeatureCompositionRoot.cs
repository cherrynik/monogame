// using Features.Debugging;
using LightInject;

namespace GameDesktop.CompositionRoots.DebugFeatures;

internal class DebugRootFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterSystems(serviceRegistry);
        // RegisterFeature(serviceRegistry);
    }

    private static void RegisterSystems(IServiceRegistry serviceRegistry)
    {
        // serviceRegistry.RegisterSingleton(factory =>
        // {
            // return new DrawRectangleCollisionComponentsSystem(factory.GetInstance<ILogger>());
        // });
    }

    // private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        // serviceRegistry.RegisterSingleton<DebugRootFeature>();
}
