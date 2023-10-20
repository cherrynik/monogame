using Components.Data;
using Entities;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

internal class PlayerEntityCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }

    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient(factory => new PlayerEntity(factory.GetInstance<Contexts>(),
            factory.GetInstance<MovementAnimationComponent>(),
            factory.GetInstance<TransformComponent>("Player"), factory.GetInstance<CameraComponent>(),
            factory.GetInstance<RectangleCollisionComponent>()));
}
