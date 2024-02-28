using System.Linq;
using Components.Data;
using Entities.Factories.Characters;
using Entities.Factories.Items;
using Entities.Factories.Meta;
using Features;
using LightInject;
using GameDesktop.CompositionRoots.Components;
using GameDesktop.CompositionRoots.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.UI;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Services.Movement;
using Systems;
using Systems.Debugging.Diagnostics;
using Systems.Debugging.World;
using Systems.Render;
#if DEBUG
using Systems.Debugging.Render;
using GameDesktop.CompositionRoots.DebugFeatures;
#endif

namespace GameDesktop.CompositionRoots.Features;

internal class RootFeatureCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        // Layered registration architecture (horizontally & vertically)
        // Hence, it allows async/multi-threaded registration

        // If it's split with space-line, then it's the end of a group.
        // A group (of registering lines) can be multi-threaded.
        // At the end of a group, the whole group has to be resolved successfully,
        // before going further.
        RegisterFundamental(serviceRegistry);

        RegisterComponents(serviceRegistry);
        RegisterEntities(serviceRegistry);

        RegisterFeatures(serviceRegistry);

#if DEBUG
        serviceRegistry.RegisterFrom<DebugRootFeatureCompositionRoot>();
#endif

        RegisterEntryPoint(serviceRegistry);
    }

    private static void RegisterFundamental(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterFrom<FundamentalCompositionRoot>();

    private static void RegisterComponents(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterFrom<ComponentsCompositionRoot>();

    private static void RegisterEntities(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterFrom<PlayerEntityCompositionRoot>();
        serviceRegistry.RegisterFrom<StaticEntityCompositionRoot>();
        serviceRegistry.RegisterFrom<RockEntityCompositionRoot>();
    }

    private static void RegisterFeatures(IServiceRegistry serviceRegistry)
    {
        // serviceRegistry.RegisterFrom<WorldInitializeFeatureCompositionRoot>();
        // serviceRegistry.RegisterFrom<InputFeatureCompositionRoot>();
        // serviceRegistry.RegisterFrom<CameraFeatureCompositionRoot>();
        // serviceRegistry.RegisterFrom<MovementFeatureCompositionRoot>();
    }

    private static void RegisterEntryPoint(IServiceRegistry serviceRegistry)
    {
        // var serviceRegistration = (Game)((PerContainerLifetime)serviceRegistry.AvailableServices.First(x => x.ServiceType == typeof(Game)).Lifetime).GetInstance((args, scope) => new object(), new Scope(new ServiceContainer()), Array.Empty<object>());
        // serviceRegistry.RegisterSingleton(factory => new ImGuiRenderer(factory.GetInstance<Game>()));

        // UI
        serviceRegistry.RegisterSingleton(_ =>
        {
            // var grid = new Grid { RowSpacing = 8, ColumnSpacing = 8 };
            //
            // grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            // grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            // grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            // grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            //
            // return grid;

            var panel = new Panel();
            var rightBottomText = new Label()
            {
                Text = "Pre-Alpha v0.1.1",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Left = -30,
                Top = -20
            };
            panel.Widgets.Add(rightBottomText);

            return panel;
        });

        serviceRegistry.RegisterSingleton(factory =>
        {
            Desktop desktop = new();
            desktop.Root = factory.GetInstance<Panel>();

            return desktop;
        });

        // ECS
        serviceRegistry.RegisterSingleton(_ => World.Create());

        // TODO: I guess, I should delete "Features" project,
        // register features actually in another particular place,
        // and create "CompositionRoots" project
        serviceRegistry.RegisterSingleton(factory => new SystemsEngine(factory.GetInstance<World>()));
        serviceRegistry.RegisterSingleton(factory =>
        {
            // ⚠ Order-sensitive zone ⚠ 
            var movement = new Feature(factory.GetInstance<World>(), factory.GetInstance<SystemsEngine>(),
                new InputSystem(factory.GetInstance<World>(), new KeyboardInput()),
                new CollisionSystem(factory.GetInstance<World>()),
                new MovementSystem(factory.GetInstance<World>(), new SimpleMovement()));

            var preRender = new Feature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                new CharacterMovementAnimationSystem(factory.GetInstance<World>()),
                new CameraFollowingSystem(factory.GetInstance<World>()));

            var render = new Feature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                new RenderCharacterMovementAnimationSystem(factory.GetInstance<World>(),
                    factory.GetInstance<SpriteBatch>()));
#if DEBUG
            const int w = 3, h = 3;
            Texture2D pivotPixel = new(factory.GetInstance<SpriteBatch>().GraphicsDevice, w, h);
            pivotPixel.SetData(Enumerable.Repeat(Color.Red, w * h).ToArray());

            Texture2D colliderPixel = new(factory.GetInstance<SpriteBatch>().GraphicsDevice, w, h);
            colliderPixel.SetData(Enumerable.Repeat(Color.LawnGreen, w * h).ToArray());

            var debug = new Feature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                new SystemsList(factory.GetInstance<World>(),
                    factory.GetInstance<SystemsEngine>()),
                new EntitiesList(factory.GetInstance<World>()),
                new FrameCounter(factory.GetInstance<World>()),
                new RenderFramesPerSec(factory.GetInstance<World>()),
                new RectangleColliderRenderSystem(factory.GetInstance<World>(), factory.GetInstance<SpriteBatch>(),
                    colliderPixel),
                new PivotRenderSystem(factory.GetInstance<World>(), factory.GetInstance<SpriteBatch>(), pivotPixel)
            );
#endif

            return new RootFeature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                new WorldInitializer(factory.GetInstance<World>(), new WorldEntityFactory(new WorldMetaComponent()),
                    factory.GetInstance<PlayerEntityFactory>(),
                    factory.GetInstance<DummyEntityFactory>(),
                    factory.GetInstance<RockEntityFactory>())
            );
        });
    }
    // serviceRegistry.RegisterSingleton<RootFeature>();
}
