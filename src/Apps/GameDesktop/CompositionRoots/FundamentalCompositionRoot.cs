using Entitas;
using LightInject;

namespace GameDesktop.CompositionRoots;

public class FundamentalCompositionRoot : ICompositionRoot
{
    private const string AllOf = "AllOf";
    private const string AnyOf = "AnyOf";

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterContexts(serviceRegistry);
        RegisterAllOfMatcher(serviceRegistry);
    }

    private static void RegisterContexts(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterInstance(Contexts.sharedInstance);

    private static void RegisterAllOfMatcher(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<IMatcher<GameEntity>[], IGroup<GameEntity>>((factory, matchers) =>
        {
            IAllOfMatcher<GameEntity> groupMatcher = GameMatcher.AllOf(matchers);
            var contexts = factory.GetInstance<Contexts>();
            return contexts.game.GetGroup(groupMatcher);
        }, AllOf);
    }
}
