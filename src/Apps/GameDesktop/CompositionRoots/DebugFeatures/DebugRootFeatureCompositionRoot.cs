using System;
using Entitas;
using Features.Debugging;
using GameDesktop.Resources.Internal;
using LightInject;
using Serilog;
using Systems.Debugging;

namespace GameDesktop.CompositionRoots.DebugFeatures;

internal class DebugRootFeatureCompositionRoot : ICompositionRoot
{
    private static readonly IMatcher<GameEntity>[] Matchers = { GameMatcher.RectangleCollision, GameMatcher.Transform };

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterSystems(serviceRegistry);
        RegisterFeature(serviceRegistry);
    }

    private static void RegisterSystems(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var getGroup =
                factory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>(Matcher.AllOf);
            IGroup<GameEntity> group = getGroup(Matchers);

            return new DrawRectangleCollisionComponentsSystem(factory.GetInstance<Contexts>(), group,
                factory.GetInstance<ILogger>());
        });
    }

    private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<DebugRootFeature>();
}
