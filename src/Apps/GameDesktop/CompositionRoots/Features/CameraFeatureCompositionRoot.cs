using System;
using Entitas;
using Entitas.Extended;
using Features;
using GameDesktop.Resources.Internal;
using LightInject;
using Systems;

namespace GameDesktop.CompositionRoots.Features;

public class CameraFeatureCompositionRoot : ICompositionRoot
{
    private static readonly IMatcher<GameEntity>[] Matchers = { GameMatcher.Transform, GameMatcher.Drawable };

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterSystem(serviceRegistry);
        RegisterFeature(serviceRegistry);
    }

    private static void RegisterSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFallback((type, s) => true, request =>
        {
            var getGroup =
                request.ServiceFactory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>(Matcher.AllOf);
            IGroup<GameEntity> group = getGroup(Matchers);

            // return new CameraFollowingSystem(request.ServiceFactory.GetInstance<Contexts>(), group);
            return new DefaultDrawSystem(group);
        }, new PerContainerLifetime());
    }

    private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<CameraFeature>();
}
