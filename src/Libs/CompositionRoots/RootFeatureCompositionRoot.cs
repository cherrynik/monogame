using CompositionRoots;
using CompositionRoots.Components.Data;
using CompositionRoots.Entities;
using CompositionRoots.Entities.Rocks;
using CompositionRoots.Entities.Trees;
using CompositionRoots.Features;
using CompositionRoots.Helpers;
using CompositionRoots.Systems;
using Constants;
using Entities;
using Features;
using FontStashSharp.RichText;
using LightInject;
using Myra.Graphics2D.UI;
using Scellecs.Morpeh.Extended;
using Services.Resolvers;
using Systems;
using UI.Blocks;
using UI.Factories;
using UI.Feature;
using World = Scellecs.Morpeh.World;

[assembly: CompositionRootType(typeof(RootFeatureCompositionRoot))]

namespace CompositionRoots;

public class RootFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        // Layered registration architecture (horizontally & vertically)
        // Hence, it allows async/multithreaded registration

        // If it's split with space-line, then it's the end of a group.
        // A group (of registering lines) can be multithreaded.
        // At the end of a group, the whole group has to be resolved successfully,
        // before going further.
        serviceRegistry.RegisterFrom<LdtkModule>()
            .RegisterFrom<AsepriteAnimatedCharacterBuilderModule>()
            .RegisterFrom<TextureResolverModule>();

        serviceRegistry.RegisterFrom<NameModule>();

        serviceRegistry.RegisterFrom<TransformModule>();

        serviceRegistry.RegisterFrom<PebbleModule>();
        serviceRegistry.RegisterFrom<TreeModule>();

        serviceRegistry.RegisterFrom<ItemModule>();

        serviceRegistry.RegisterFrom<EntityFactoriesCompositionRoot>();

        serviceRegistry.RegisterSingleton(_ => World.Create());
        serviceRegistry.RegisterFrom<CollisionSystemModule>()
            .RegisterFrom<InventorySystemModule>()
            .RegisterFrom<InputSystemModule>()
            .RegisterFrom<MovementSystemModule>()
            .RegisterFrom<SystemsEngineModule>()
            .RegisterFrom<CameraSystemModule>();

        serviceRegistry
            // .RegisterFrom<InitializeFeatureModule>()
            .RegisterFrom<UpdateFeatureModule>()
            .RegisterFrom<PreRenderFeatureModule>()
            .RegisterFrom<RenderFeatureModule>()
            .RegisterFrom<DebugFeatureModule>();

        serviceRegistry.RegisterSingleton(factory =>
        {
            // ⚠ Order-sensitive zone ⚠ 
            // factory.GetInstance<Feature>(DiContainerNames.Features.Initialize);
            factory.GetInstance<Feature>(DiContainerNames.Features.Update);
            factory.GetInstance<Feature>(DiContainerNames.Features.PreRender);
            factory.GetInstance<Feature>(DiContainerNames.Features.Render);

#if DEBUG
            factory.GetInstance<Feature>(DiContainerNames.Features.Debug);
#endif

            return new RootFeature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                new WorldInitializer(factory.GetInstance<World>(),
                    factory.GetInstance<EntitiesFactory>(),
                    LdtkResolver.ResolveFromApp(Contents.TileMaps.Test))
            );
        });
        RegisterUI(serviceRegistry);
    }

    private static void RegisterUI(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new Panel());
        serviceRegistry.RegisterSingleton(factory =>
            new Inventory(factory.GetInstance<Panel>(), factory.GetInstance<InventorySystem>()));
        serviceRegistry.RegisterSingleton(factory =>
            new Counter(factory.GetInstance<Panel>(),
                new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Left = -30,
                    Top = -50,
                    TextAlign = TextHorizontalAlignment.Right
                }));
        serviceRegistry.RegisterSingleton<Func<Counter>>(factory => factory.GetInstance<Counter>);

        serviceRegistry.RegisterSingleton(factory => new GameVersion(
            factory.GetInstance<Panel>(),
            new Label
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Left = -30,
                Top = -20,
                TextAlign = TextHorizontalAlignment.Right,
                Text = AppVariables.GameVersion
            }));
        serviceRegistry.RegisterSingleton<Func<GameVersion>>(factory => factory.GetInstance<GameVersion>);

        serviceRegistry.RegisterSingleton(factory =>
            new UIFactory(factory.GetInstance<Func<Counter>>(), factory.GetInstance<Func<GameVersion>>(),
                factory.GetInstance<CollisionSystem>()));
        serviceRegistry.RegisterSingleton(factory => new MainScreen(factory.GetInstance<UIFactory>()));

        serviceRegistry.RegisterSingleton(factory =>
        {
            Desktop desktop = new();
            desktop.Root = factory.GetInstance<Panel>();

            factory.GetInstance<Inventory>();
            factory.GetInstance<MainScreen>();

            return desktop;
        });
    }
}
