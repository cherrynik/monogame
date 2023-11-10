using Components.Data;
using Entities;
using FontStashSharp;
using GameDesktop.CompositionRoots.Features;
using Implementations;
using MonoGame.ImGuiNet;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Serilog;
using Services.Movement;
using Systems;
using Systems.Debugging;
using Systems.Render;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using Systems.Debugging.Render;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    private ImGuiRenderer _guiRenderer;
    private SpriteBatch _spriteBatch;
    private Desktop _desktop;

    private SystemsGroup _systemsGroup;
    private SystemsGroup _renderSystemsGroup;
    private SystemsGroup _debugSystemsGroup;

    // TODO: Frames updating

    // https://gafferongames.com/post/fix_your_timestep/
    // https://lajbert.wordpress.com/2021/05/02/fix-your-timestep-in-monogame/

    // TODO: Nez has cool physics & other projects libs to use as deps

    public Game(ILogger logger, IServiceContainer container)
    {
        _logger = logger;
        _container = container;

        _logger.ForContext<Game>().Verbose("ctor");
    }

    protected override void Initialize()
    {
        _logger.ForContext<Game>().Verbose($"Initialize(): start; available {GraphicsDevice}");
        _logger.ForContext<Game>().Verbose("SpriteBatch initialization...");

        _container.RegisterSingleton(_ => new SpriteBatch(GraphicsDevice));
        _spriteBatch = _container.GetInstance<SpriteBatch>();

        _guiRenderer = new(this);
        _guiRenderer.RebuildFontAtlas();

        _logger.ForContext<Game>().Verbose("SpriteBatch initialized");

        base.Initialize();

        _logger.ForContext<Game>().Verbose("Initialize(): end");
    }

    protected override void LoadContent()
    {
        // TODO: Logging with game flags (like LOG_MOVEMENT, etc)?
        // todo: pass tru logger & log places
        // TODO: Error handling
        _logger.ForContext<Game>().Verbose("LoadContent(): start");

        _container.RegisterFrom<RootFeatureCompositionRoot>();

        // -----
        var texture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        texture.SetData(new[] { Color.Gold });
        MyraEnvironment.Game = this;
        Stylesheet.Current.ButtonStyle = new()
        {
            Background = new ColoredRegion(new TextureRegion(texture, new Rectangle(0, 0, 15, 15)), Color.Gold),
            Padding = new Thickness(5, 5),
        };
        var grid = new Grid { RowSpacing = 8, ColumnSpacing = 8 };

        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

        var helloWorld = new Label { Id = "label", Text = "Hello, World!" };
        grid.Widgets.Add(helloWorld);

// ComboBox
        var combo = new ComboBox();
        Grid.SetColumn(combo, 1);
        Grid.SetRow(combo, 0);

        combo.Items.Add(new ListItem("Red", Color.Red));
        combo.Items.Add(new ListItem("Green", Color.Green));
        combo.Items.Add(new ListItem("Blue", Color.Blue));
        grid.Widgets.Add(combo);

// Button
        var button = new Button { Content = new Label { Text = "Show" } };
        Grid.SetColumn(button, 0);
        Grid.SetRow(button, 1);

        button.Click += (s, a) =>
        {
            var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
            messageBox.ShowModal(_desktop);
        };

        grid.Widgets.Add(button);

// Spin button
        var spinButton = new SpinButton { Width = 100, Nullable = true };
        Grid.SetColumn(spinButton, 1);
        Grid.SetRow(spinButton, 1);

        grid.Widgets.Add(spinButton);

// Add it to the desktop
        _desktop = new Desktop();
        _desktop.Root = grid;
        // ------

        World world = World.Create();
        var worldEntity = new WorldEntity(new WorldComponent());
        worldEntity.Create(@in: world);

        var player = _container.GetInstance<PlayerEntity>();
        player.Create(@in: world);

        var dummy = _container.GetInstance<DummyEntity>();
        dummy.Create(@in: world);

        _systemsGroup = world.CreateSystemsGroup();

        // TODO: sort if it's on the fixed update, the regular one, or the late one
        _systemsGroup.AddSystem(new InputSystem(world, new KeyboardInput()));
        _systemsGroup.AddSystem(new MovementSystem(world, new SimpleMovement()));
        // TODO: add visible in viewport in some cache for the future calculations

        _systemsGroup.AddSystem(new CharacterMovementAnimationSystem(world));
        _systemsGroup.AddSystem(new CameraFollowingSystem(world
            // new FollowingCamera(_spriteBatch, new Viewport(0, 0, 800, 480))
        ));
#if DEBUG
        _systemsGroup.AddSystem(new FrameCounter(world));
#endif

        // _preRenderSystemsGroup = world.CreateSystemsGroup();

        _renderSystemsGroup = world.CreateSystemsGroup();
        _renderSystemsGroup.AddSystem(new RenderCharacterMovementAnimationSystem(world, _spriteBatch));

#if DEBUG
        _debugSystemsGroup = world.CreateSystemsGroup();
        _debugSystemsGroup.AddSystem(new EntitiesList(world));
        _debugSystemsGroup.AddSystem(new RenderFramesPerSec(world));
        // _debugSystemsGroup.AddSystem(new FrameCounter(world));

        // var pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        // pixel.SetData(new[] { Color.Gold });
        //
        // _debugSystemsGroup.AddSystem(new PivotRenderSystem(world, _spriteBatch, pixel));
#endif

        _logger.ForContext<Game>().Verbose("LoadContent(): end");
    }

    protected override void BeginRun()
    {
        _logger.ForContext<Game>().Verbose("Beginning run...");

        base.BeginRun();

        _logger.ForContext<Game>().Verbose("Running");
    }

    protected override void EndRun()
    {
        _logger.ForContext<Game>().Verbose("Ending run...");

        base.EndRun();

        _logger.ForContext<Game>().Verbose("Ended");
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _systemsGroup.FixedUpdate(deltaTime);

        _systemsGroup.Update(deltaTime);

        _systemsGroup.LateUpdate(deltaTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        // I guess, pre-render, pre-ui systems would go in update, to avoid frames skipping
        // _preRenderSystemsGroup.Update(deltaTime);
        _renderSystemsGroup.Update(deltaTime);
        _spriteBatch.End();

        // after everything, so, it's drawn on top
        _desktop.Render();

#if DEBUG
        _guiRenderer.BeginLayout(gameTime);
        _debugSystemsGroup.Update(deltaTime);
        _guiRenderer.EndLayout();
#endif
    }

    protected override void Dispose(bool disposing)
    {
        _logger.ForContext<Game>().Verbose("Disposing...");

        base.Dispose(disposing);

        _logger.ForContext<Game>().Verbose("Disposed");
    }
}
