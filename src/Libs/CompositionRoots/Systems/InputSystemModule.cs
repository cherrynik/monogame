using LightInject;
using Scellecs.Morpeh;
using Systems;

namespace CompositionRoots.Systems;

public class InputSystemModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<IInputScanner, KeyboardInput>();
        serviceRegistry.RegisterSingleton<InputSystem>();
    }
}
