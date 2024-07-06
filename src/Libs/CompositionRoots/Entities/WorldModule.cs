using Components.Data;
using Entities.Factories.Meta;
using LightInject;

namespace CompositionRoots.Entities;

public class WorldModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<WorldMetaComponent>();

        serviceRegistry.RegisterSingleton<WorldEntityFactory>();
    }
}
