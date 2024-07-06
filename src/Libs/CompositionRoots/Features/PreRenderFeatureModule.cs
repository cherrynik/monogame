using Constants;
using LightInject;
using Microsoft.Xna.Framework;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Services.Implementations.Movement;
using Systems;
using Systems.Render;

namespace CompositionRoots.Features;

public class PreRenderFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory => new Feature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                new CharacterMovementAnimationSystem(factory.GetInstance<World>()),
                new CameraFollowingSystem(factory.GetInstance<World>(),
                    factory.GetInstance<GraphicsDeviceManager>())),
            DINames.Features.PreRender
        );
    }
}
