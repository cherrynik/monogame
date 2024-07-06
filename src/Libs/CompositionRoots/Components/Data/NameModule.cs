using Components.Data;
using LightInject;

namespace CompositionRoots.Components.Data;

public class NameModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<string, NameComponent>((_, name) => new NameComponent(name));
    }
}
