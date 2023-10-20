using System;
using Entitas;
using Entitas.Extended;
using Features;
using GameDesktop.Resources.Internal;
using LightInject;
using Systems;

namespace GameDesktop.CompositionRoots.Features;

internal class CameraFeatureCompositionRoot : ICompositionRoot
{
    private static readonly IMatcher<GameEntity>[] Matchers = { GameMatcher.Transform, GameMatcher.Drawable };

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterSystem(serviceRegistry);
        RegisterFeature(serviceRegistry);
    }

    private static void RegisterSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<IDrawSystem>(factory =>
        {
            var getGroup =
                factory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>(Matcher.AllOf);
            IGroup<GameEntity> group = getGroup(Matchers);

            // return new CameraFollowingSystem(factory.GetInstance<Contexts>(), group);
            return new DefaultDrawSystem(group);
        });
    }

    private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<CameraFeature>();
}
