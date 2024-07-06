using LightInject;
using Scellecs.Morpeh;
using Implementations.Movement;
using Systems;

namespace CompositionRoots.Systems;

public class MovementSystemModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<IMovement, SimpleMovement>();
        serviceRegistry.RegisterSingleton<MovementSystem>();
    }
}
