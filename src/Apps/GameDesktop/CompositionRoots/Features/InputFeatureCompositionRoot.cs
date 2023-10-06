using System;
using Entitas;
using Features;
using LightInject;
using Serilog;
using Services.Input;
using Systems;

namespace GameDesktop.CompositionRoots.Features;

public class InputFeatureCompositionRoot : ICompositionRoot
{
    private static readonly IMatcher<GameEntity>[] Matchers =
    {
        GameMatcher.Transform, GameMatcher.Movable, GameMatcher.Player
    };

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterImpl(serviceRegistry);
        RegisterSystem(serviceRegistry);
        RegisterFeature(serviceRegistry);
    }

    private static void RegisterImpl(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<IInputScanner, KeyboardScanner>();

    private static void RegisterSystem(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var inputScanner = factory.GetInstance<IInputScanner>();

            var getGroup = factory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>();
            IGroup<GameEntity> group = getGroup(Matchers);

            var logger = factory.GetInstance<ILogger>();

            return new InputSystem(inputScanner, group, logger);
        });
    }

    private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterSingleton<InputFeature>();
}
