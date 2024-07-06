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
        serviceRegistry.RegisterSingleton<WorldInitializer>();
        serviceRegistry.RegisterSingleton(factory => new Feature(
            factory.GetInstance<World>(),
            factory.GetInstance<SystemsEngine>(),
            factory.GetInstance<WorldInitializer>()
        ), DiContainerNames.Features.Initialize);
    }
}
