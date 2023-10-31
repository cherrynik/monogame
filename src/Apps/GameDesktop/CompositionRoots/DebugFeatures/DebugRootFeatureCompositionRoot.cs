// using Features.Debugging;
using LightInject;
using MonoGame.ImGuiNet;

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
        serviceRegistry.RegisterSingleton(factory => new ImGuiRenderer(factory.GetInstance<Game>()));

        // serviceRegistry.RegisterSingleton(factory =>
        // {
            // return new DrawRectangleCollisionComponentsSystem(factory.GetInstance<ILogger>());
        // });
    }

    // private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        // serviceRegistry.RegisterSingleton<DebugRootFeature>();
}
