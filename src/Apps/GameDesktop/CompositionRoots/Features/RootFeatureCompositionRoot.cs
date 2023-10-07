using Features;
using GameDesktop.CompositionRoots.Components;
using GameDesktop.CompositionRoots.Entities;
using LightInject;

namespace GameDesktop.CompositionRoots.Features;

public class RootFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        // Layered registration architecture (horizontally & vertically)
        // Hence, it allows async/multi-threaded registration

        // If it's split with space-line, then it's the end of a group.
        // A group (of registering lines) can be multi-threaded.
        // At the end of a group, the whole group has to be resolved successfully,
        // before going further.
        RegisterFundamental(serviceRegistry);

        RegisterComponents(serviceRegistry);
        RegisterEntities(serviceRegistry);

        RegisterFeatures(serviceRegistry);

        // Main entry point
        serviceRegistry.RegisterSingleton<RootFeature>();
    }

    private static void RegisterFundamental(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFrom<FundamentalCompositionRoot>();
    }

    private static void RegisterComponents(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFrom<ComponentsCompositionRoot>();
    }

    private static void RegisterEntities(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFrom<PlayerEntityCompositionRoot>();
        serviceRegistry.RegisterFrom<StaticEntityCompositionRoot>();
    }

    private static void RegisterFeatures(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFrom<WorldInitializeFeatureCompositionRoot>();
        serviceRegistry.RegisterFrom<InputFeatureCompositionRoot>();
        serviceRegistry.RegisterFrom<CameraFeatureCompositionRoot>();
        serviceRegistry.RegisterFrom<MovementFeatureCompositionRoot>();
    }
}
