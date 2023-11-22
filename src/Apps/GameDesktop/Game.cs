using Features;
using GameDesktop.CompositionRoots.Features;
using GameDesktop.Factories;
using ImGuiNET;
using JetBrains.Annotations;
using MonoGame.ImGuiNet;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Serilog;
using Myra.Graphics2D.UI;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    [CanBeNull] private ImGuiRenderer _imGuiRenderer;
    private SpriteBatch _spriteBatch;
    private Desktop _desktop;

    // TODO: Frames updating
    // TODO: Player position & other things debug showing, input, etc
    // TODO: Nez has cool physics & other projects libs to use as deps

    // https://gafferongames.com/post/fix_your_timestep/
    // https://lajbert.wordpress.com/2021/05/02/fix-your-timestep-in-monogame/
    private RootFeature _rootFeature;

    public Game(ILogger logger, IServiceContainer container)
    {
        _logger = logger;
        _container = container;

        _logger.ForContext<Game>().Verbose("ctor");
    }

    protected override void Initialize()
    {
        _logger.ForContext<Game>().Verbose($"Initialize(): start; available {GraphicsDevice}");
        _logger.ForContext<Game>().Verbose("Circular dependencies initialization...");
        RegisterSpriteBatch();
        RegisterMyraUIEnvironment();
#if DEBUG
        RegisterImGuiRenderer();
#endif
        _logger.ForContext<Game>().Verbose("Circular dependencies initialized");

        _logger.ForContext<Game>().Verbose("Game services initialization...");
        RegisterRootFeature();
        RegisterMyraUI();
        _logger.ForContext<Game>().Verbose("Game services initialized");

        base.Initialize();

        _logger.ForContext<Game>().Verbose("Initialize(): end");
    }

    protected override void LoadContent()
    {
        // TODO: Logging with game flags (like LOG_MOVEMENT, etc)?
        // todo: pass tru logger & log places
        // TODO: Error handling
        _logger.ForContext<Game>().Verbose("LoadContent(): start");

        _rootFeature.OnAwake();

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

        _imGuiRenderer?.BeginLayout(gameTime);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _rootFeature.OnRender(deltaTime);
        _spriteBatch.End();

        _desktop.Render();

        _imGuiRenderer?.EndLayout();
    }

    protected override void Dispose(bool disposing)
    {
        _logger.ForContext<Game>().Verbose("Disposing...");

        base.Dispose(disposing);

        _logger.ForContext<Game>().Verbose("Disposed");
    }

    private void RegisterSpriteBatch()
    {
        _container.RegisterSingleton(_ => new SpriteBatch(GraphicsDevice));
        _spriteBatch = _container.GetInstance<SpriteBatch>();
    }

    private void RegisterMyraUIEnvironment() => MyraEnvironment.Game = this;

    private void RegisterImGuiRenderer()
    {
        _container.RegisterSingleton(factory =>
        {
            ImGuiRenderer imGuiRenderer = new(factory.GetInstance<Game>());
            imGuiRenderer.RebuildFontAtlas();
            return imGuiRenderer;
        });

        _imGuiRenderer = _container.GetInstance<ImGuiRenderer>();

        ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;
    }

    private void RegisterRootFeature()
    {
        _container.RegisterFrom<RootFeatureCompositionRoot>();
        _rootFeature = _container.GetInstance<RootFeature>();
    }

    private void RegisterMyraUI()
    {
        // ComboBox
        var combo = new ComboBox();
        combo.Items.Add(new ListItem("Red", Color.Red));
        combo.Items.Add(new ListItem("Green", Color.Green));
        combo.Items.Add(new ListItem("Blue", Color.Blue));

        // Button
        var button = new Button { Content = new Label { Text = "Show" } };
        button.Click += (s, a) =>
        {
            var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
            messageBox.ShowModal(_desktop);
        };

        var grid = _container.GetInstance<Grid>();
        new UIFactory(grid,
                new Label { Id = "label", Text = "Hello, World!" },
                combo,
                button,
                new SpinButton { Width = 100, Nullable = true })
            .Build();

        _desktop = _container.GetInstance<Desktop>();
    }
}
