using Components.Data;
using Entities.Meta;
using LightInject;

namespace CompositionRoots.Entities;

public class WorldModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new WorldMetaComponent(new(10_000, 10_000)))
            .RegisterSingleton<WorldEntityFactory>();
    }
}
