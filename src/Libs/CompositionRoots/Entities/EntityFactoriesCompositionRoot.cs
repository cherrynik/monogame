using CompositionRoots.Entities;
using Entities;
using Entities.Items.Rocks;
using Entities.Items.Trees;
using LightInject;

[assembly: CompositionRootType(typeof(EntityFactoriesCompositionRoot))]

namespace CompositionRoots.Entities;

public class EntityFactoriesCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFrom<WorldModule>()
            .RegisterFrom<PlayerModule>()
            .RegisterSingleton<TreeFactory>()
            .RegisterSingleton<AbstractTreeFactory>()
            .RegisterSingleton<PebbleFactory>()
            .RegisterSingleton<AbstractRockFactory>()
            .RegisterSingleton<LdtkEntitiesFactory>();
    }
}
