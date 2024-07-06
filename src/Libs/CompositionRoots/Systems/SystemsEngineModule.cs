using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace CompositionRoots.Systems;

public class SystemsEngineModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<SystemsEngine>();
    }
}
