using Constants;
using LightInject;
using Microsoft.Xna.Framework;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Implementations.Movement;
using Systems;
using Implementations.Camera;
using Systems.Render;

namespace CompositionRoots.Features;

public class PreRenderFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<CharacterMovementAnimationSystem>()
            .RegisterSingleton(factory => new Feature(factory.GetInstance<World>(),
                    factory.GetInstance<SystemsEngine>(),
                    factory.GetInstance<CharacterMovementAnimationSystem>(),
                    factory.GetInstance<CameraFollowingSystem>()),
                DiContainerNames.Features.PreRender
            );
    }
}
