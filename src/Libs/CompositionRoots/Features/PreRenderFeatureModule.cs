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
        serviceRegistry.RegisterSingleton<CharacterAnimationUpdateSystem>()
            .RegisterSingleton(factory => new Feature(factory.GetInstance<World>(),
                    factory.GetInstance<SystemsEngine>(),
                    factory.GetInstance<CharacterAnimationUpdateSystem>(),
                    factory.GetInstance<CameraFollowingSystem>()),
                DiContainerNames.Features.PreRender
            );
    }
}
