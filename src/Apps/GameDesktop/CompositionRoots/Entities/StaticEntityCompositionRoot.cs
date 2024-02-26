using Components.Data;
using Components.Render.Static;
using Entities;
using Entities.Factories;
using Entities.Factories.Characters;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

internal class StaticEntityCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }


    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient(factory => new DummyEntityFactory(
            new NameComponent("Dummy"), // factory.GetInstance<NameComponent>("Dummy")
            factory.GetInstance<TransformComponent>("DummyEntity"),
            factory.GetInstance<SpriteComponent>(),
            factory.GetInstance<RectangleCollisionComponent>()));
}
