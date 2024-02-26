using Components.Data;
using Components.Render.Static;
using Entities.Factories.Items;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

public class RockEntityCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }


    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient(factory => new RockEntityFactory(
            new NameComponent("Rock"), // factory.GetInstance<NameComponent>("Rock")
            factory.GetInstance<ItemComponent>("Rock"),
            factory.GetInstance<TransformComponent>("RockEntity"),
            factory.GetInstance<SpriteComponent>() //factory.GetInstance<SpriteComponent>("Rock")
        ));
}
