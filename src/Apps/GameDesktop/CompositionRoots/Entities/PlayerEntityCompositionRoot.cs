using Components.Data;
using Components.Render.Animation;
using Components.Tags;
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
        serviceRegistry.RegisterTransient(factory => new PlayerEntity(
            factory.GetInstance<PlayerComponent>(),
            factory.GetInstance<MovableComponent>(),
            factory.GetInstance<TransformComponent>("PlayerEntity"),
            factory.GetInstance<RectangleCollisionComponent>(),
            factory.GetInstance<MovementAnimationsComponent>(),
            factory.GetInstance<CharacterAnimatorComponent>("PlayerEntity")));
}
