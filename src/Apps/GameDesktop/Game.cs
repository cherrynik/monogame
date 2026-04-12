using System;
using Features;
using GameDesktop.CompositionRoots.Features;
using GameDesktop.Ui.Myra;
using GameUi.App.Effects;
using GameUi.App.Input;
using GameUi.App.State;
using GameUi.Pages.Game;
using GameUi.Shared.UI;
using ImGuiNET;
using JetBrains.Annotations;
using MonoGame.ImGuiNet;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using Serilog;
using Services.Input;

namespace GameDesktop;

internal class Game : Microsoft.Xna.Framework.Game
{
    private readonly ILogger _logger;
    private readonly IServiceContainer _container;
    private readonly MyraUiEnvironmentInitializer _myraUiEnvironmentInitializer;

    [CanBeNull] private ImGuiRenderer _imGuiRenderer;
    private SpriteBatch _spriteBatch;
    private Desktop _desktop = default!;
    private GameUiView _uiView = default!;
    private UiStore _uiStore = new();
    private readonly UiTheme _uiTheme = UiTheme.Default;
    private readonly UiVisualStyle _uiStyle = UiVisualStyle.Default;
    private readonly GameInputRouter _inputRouter = new(new SdlKeyboardStateSource());
    private readonly GameUiEffectsHandler _uiEffectsHandler = new();

    // TODO: Frames updating
    // TODO: Player position & other things debug showing, input, etc
    // TODO: Nez has cool physics & other projects libs to use as deps

    // https://gafferongames.com/post/fix_your_timestep/
    // https://lajbert.wordpress.com/2021/05/02/fix-your-timestep-in-monogame/
    private RootFeature _rootFeature;

    public Game(ILogger logger, IServiceContainer container, MyraUiEnvironmentInitializer myraUiEnvironmentInitializer)
    {
        _logger = logger;
        _container = container;
        _myraUiEnvironmentInitializer = myraUiEnvironmentInitializer;

        _logger.ForContext<Game>().Verbose("ctor");
    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = true;
        _logger.ForContext<Game>().Verbose($"Initialize(): start; available {GraphicsDevice}");
        _logger.ForContext<Game>().Verbose("Circular dependencies (external) initialization...");
        RegisterSpriteBatch();
        RegisterMyraUIEnvironment();
        RegisterImGuiRenderer();
        _logger.ForContext<Game>().Verbose("Circular dependencies (external) initialized");

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
        _logger.ForContext<Game>().Verbose("Beginning to run...");

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
        GameInputFrame inputFrame = _inputRouter.Capture();

        if (inputFrame.TogglePauseRequested)
        {
            DispatchUi(UiAction.TogglePause);
        }

        if (_uiStore.State.IsPaused && inputFrame.PauseMenuAction.HasValue)
        {
            DispatchUi(inputFrame.PauseMenuAction.Value);
        }

        RenderUi();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Camera must update even while paused (viewport may change from fullscreen toggle)
        _rootFeature.OnLateUpdate(deltaTime);

        if (_uiStore.State.IsPaused)
        {
            return;
        }

        _rootFeature.OnFixedUpdate(deltaTime);

        _rootFeature.OnUpdate(deltaTime);
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

    private void RegisterMyraUIEnvironment()
    {
        _myraUiEnvironmentInitializer.Initialize(this);
    }

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
        var grid = _container.GetInstance<Grid>();
        _desktop = _container.GetInstance<Desktop>();
        _uiView = new GameUiView(grid, _uiTheme, _uiStyle, DispatchUi);
        _uiStore = new UiStore();
        RenderUi();
    }

    private void DispatchUi(UiAction action)
    {
        _uiStore.Dispatch(action);
        if (_uiEffectsHandler.Apply(_uiStore, _desktop))
        {
            Exit();
        }
    }

    private void RenderUi()
    {
        _uiView.Render(_uiStore.State);
    }
}
