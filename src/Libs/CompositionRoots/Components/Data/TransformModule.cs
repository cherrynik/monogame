using Components.Data;
using CompositionRoots.Components.Data;
using LightInject;

[assembly: CompositionRootType(typeof(TransformModule))]

namespace CompositionRoots.Components.Data;

public class TransformModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient(_ => new TransformComponent());
    }
}
