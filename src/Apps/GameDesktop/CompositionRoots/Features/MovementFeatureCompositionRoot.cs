using System;
using Entitas;
using Features;
using LightInject;
using Serilog;
using Services;
using Services.Movement;
using Systems;

namespace GameDesktop.CompositionRoots;

public class MovementFeatureCompositionRoot : ICompositionRoot
{
    private static readonly IMatcher<GameEntity>[] Matchers = { GameMatcher.Transform, GameMatcher.Movable };

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterImpl(serviceRegistry);
        RegisterSystem(serviceRegistry);
        RegisterFeature(serviceRegistry);
    }

    private static void RegisterImpl(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<IMovement, SimpleMovement>();

    private static void RegisterSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var movement = factory.GetInstance<IMovement>();

            var getGroup = factory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>();
            IGroup<GameEntity> group = getGroup(Matchers);

            var logger = factory.GetInstance<ILogger>();

            return new MovementSystem(movement, group, logger);
        });
    }

    private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<MovementFeature>();
}
