using LightInject;
using Scellecs.Morpeh;
using Systems;

namespace CompositionRoots.Systems;

public class CollisionSystemModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<CollisionSystem>();
    }
}
