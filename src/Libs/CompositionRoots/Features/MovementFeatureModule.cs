using Constants;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Services.Implementations.Movement;
using Systems;

namespace CompositionRoots.Features;

public class MovementFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
                new Feature(factory.GetInstance<World>(), factory.GetInstance<SystemsEngine>(),
                    new InputSystem(factory.GetInstance<World>(), new KeyboardInput()),
                    factory.GetInstance<CollisionSystem>(),
                    // factory.GetInstance<TriggerSystem>(),
                    factory.GetInstance<InventorySystem>(),
                    new MovementSystem(factory.GetInstance<World>(), new SimpleMovement())),
            DINames.Features.Movement
        );
    }
}
