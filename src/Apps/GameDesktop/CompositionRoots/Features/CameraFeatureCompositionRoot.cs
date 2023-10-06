using System;
using Entitas;
using Features;
using LightInject;
using Systems;

namespace GameDesktop.CompositionRoots.Features;

public class CameraFeatureCompositionRoot : ICompositionRoot
{
    private static readonly IMatcher<GameEntity>[] Matchers = { GameMatcher.Transform, GameMatcher.Sprite };

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterSystem(serviceRegistry);
        RegisterFeature(serviceRegistry);
    }

    private static void RegisterSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFallback((type, s) => true, request =>
        {
            var getGroup = request.ServiceFactory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>();
            IGroup<GameEntity> group = getGroup(Matchers);

            Contexts contexts = request.ServiceFactory.GetInstance<Contexts>();
            
            return new CameraFollowingSystem(contexts, group);
        }, new PerContainerLifetime());
    }

    private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<CameraFeature>();
}
