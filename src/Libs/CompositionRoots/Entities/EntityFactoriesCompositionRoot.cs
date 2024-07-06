using CompositionRoots.Entities;
using Entities.Factories;
using Entities.Factories.Items.Rocks;
using Entities.Factories.Items.Trees;
using LightInject;

[assembly: CompositionRootType(typeof(EntityFactoriesCompositionRoot))]

namespace CompositionRoots.Entities;

public class EntityFactoriesCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFrom<WorldModule>();
        serviceRegistry.RegisterFrom<PlayerModule>();

        serviceRegistry.RegisterSingleton<TreeFactory>();
        serviceRegistry.RegisterSingleton<AbstractTreeFactory>();

        serviceRegistry.RegisterSingleton<PebbleFactory>();
        serviceRegistry.RegisterSingleton<AbstractRockFactory>();

        serviceRegistry.RegisterSingleton<EntitiesFactory>();
    }
}
