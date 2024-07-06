using CompositionRoots.Systems;
using Constants;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Implementations.Movement;
using Systems;
using MovementSystem = Systems.MovementSystem;

namespace CompositionRoots.Features;

public class UpdateFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
                new Feature(factory.GetInstance<World>(),
                    factory.GetInstance<SystemsEngine>(),
                    factory.GetInstance<InputSystem>(),
                    factory.GetInstance<CollisionSystem>(),
                    factory.GetInstance<InventorySystem>(),
                    factory.GetInstance<MovementSystem>()),
            DiContainerNames.Features.Update
        );
    }
}
