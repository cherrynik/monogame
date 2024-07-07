using Constants;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems;

namespace CompositionRoots.Features;

public class InitializeFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        // new WorldInitializer(factory.GetInstance<World>(),
        //     factory.GetInstance<LdtkEntitiesFactory>(),
        //     LdtkResolver.ResolveFromApp(Contents.TileMaps.Test))

        serviceRegistry.RegisterSingleton<WorldInitializer>()
            .RegisterSingleton(factory => new Feature(
                factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                factory.GetInstance<WorldInitializer>()
            ), DiContainerNames.Features.Initialize);
    }
}
