using Components.Data;
using Components.Render.Static;
using Entities;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

internal class StaticEntityCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }


    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient(factory => new DummyEntity(
            factory.GetInstance<TransformComponent>("DummyEntity"),
            factory.GetInstance<SpriteComponent>(),
            factory.GetInstance<RectangleCollisionComponent>()));
}
