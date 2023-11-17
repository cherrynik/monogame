using Components.Data;
using Entities;
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
using Scellecs.Morpeh.Extended;
using Systems.Debugging.Render;

namespace GameDesktop;

public class WorldInitializer : IInitializer
{
    public World World { get; set; }
    private readonly WorldEntity _worldEntity;
    private readonly PlayerEntity _playerEntity;
    private readonly DummyEntity _dummyEntity;

    public WorldInitializer(World world,
        WorldEntity worldEntity,
        PlayerEntity playerEntity,
        DummyEntity dummyEntity)
    {
        World = world;
        _worldEntity = worldEntity;
        _playerEntity = playerEntity;
        _dummyEntity = dummyEntity;
    }

    public void OnAwake()
    {
        _worldEntity.Create(@in: World);
        _playerEntity.Create(@in: World);
        _dummyEntity.Create(@in: World);
    }

    public void Dispose()
    {
    }
}

public class RenderFeature : Feature
{
    public RenderFeature(World world,
        RenderCharacterMovementAnimationSystem renderCharacterMovementAnimationSystem) : base(world)
    {
        Add(renderCharacterMovementAnimationSystem);
    }
}

public class PreRenderFeature : Feature
{
    public PreRenderFeature(World world,
        CharacterMovementAnimationSystem characterMovementAnimationSystem,
        CameraFollowingSystem cameraFollowingSystem) : base(world)
    {
        Add(characterMovementAnimationSystem);
        Add(cameraFollowingSystem);
    }
}

public class MovementFeature : Feature
{
    public MovementFeature(World world,
        InputSystem inputSystem,
        MovementSystem movementSystem) : base(world)
    {
        Add(inputSystem);
        Add(movementSystem);
    }
}

public class RootFeature : Feature
{
    public RootFeature(World world,
        WorldInitializer worldInitializer,
        // MovementFeature movementFeature,
        InputSystem inputSystem,
        MovementSystem movementSystem,
        CharacterMovementAnimationSystem characterMovementAnimationSystem,
        CameraFollowingSystem cameraFollowingSystem,
        RenderCharacterMovementAnimationSystem renderCharacterMovementAnimationSystem) : base(world)
    {
        Add(worldInitializer);
        // Add(movementFeature);
        Add(inputSystem);
        Add(movementSystem);
        Add(characterMovementAnimationSystem);
        Add(cameraFollowingSystem);
        Add(renderCharacterMovementAnimationSystem);
    }
}

public static class MainWorld
{
    public static SystemsGroup AddSystems(World @in, InputSystem inputSystem, MovementSystem movementSystem)
    {
        SystemsGroup systemsGroup = @in.CreateSystemsGroup();

        // TODO: sort systems if they're on the fixed, regular, or late update
        // TODO: add visible in viewport in some cache for the future calculations

#if DEBUG
        systemsGroup.AddSystem(new FrameCounter(@in));
#endif

        return systemsGroup;
    }
}

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    private ImGuiRenderer _guiRenderer;
    private SpriteBatch _spriteBatch;
    private Desktop _desktop;

    // TODO: Frames updating
    // TODO: Player position & other things debug showing, input, etc
    // TODO: Nez has cool physics & other projects libs to use as deps

    // https://gafferongames.com/post/fix_your_timestep/
    // https://lajbert.wordpress.com/2021/05/02/fix-your-timestep-in-monogame/
    private RootFeature _rootFeature;
    private SystemsGroup _debugSystemsGroup;

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

        _rootFeature = new RootFeature(world,
            new WorldInitializer(world, new WorldEntity(new WorldComponent()),
                _container.GetInstance<PlayerEntity>(),
                _container.GetInstance<DummyEntity>()),
            // new MovementFeature(world,
            new InputSystem(world, new KeyboardInput()),
            new MovementSystem(world, new SimpleMovement()),
            // ),
            new CharacterMovementAnimationSystem(world),
            new CameraFollowingSystem(world),
            new RenderCharacterMovementAnimationSystem(world, _spriteBatch));

        _rootFeature.OnAwake();

#if DEBUG
        _debugSystemsGroup = world.CreateSystemsGroup();
        _debugSystemsGroup.AddSystem(new EntitiesList(world));
        _debugSystemsGroup.AddSystem(new RenderFramesPerSec(world));
        //
        _debugSystemsGroup.AddSystem(new FrameCounter(world));
        //
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
        _rootFeature.OnFixedUpdate(deltaTime);

        _rootFeature.OnUpdate(deltaTime);

        _rootFeature.OnLateUpdate(deltaTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _rootFeature.OnRender(deltaTime);
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
