using CompositionRoots;
using Features;
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

// TODO: 1. Камера, зум, границы уровня
// 2. Переход между уровнями
// 3. Меню, экран загрузки, настройки игры, пауза
// 4. Окно, фпс и скорость игры
// 5. Сохранение, загрузка
// 6. Инвентарь, добыча ресурсов, квесты, награды
// 
// Наполнение уровня энтити, компоненты, коллайдеры, рефакторинг
// Пофиксить при движении по диагонали полосы между тайлами
public class Game : Microsoft.Xna.Framework.Game
{
    private const bool IsWindowResizable = true;

    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    [CanBeNull] private ImGuiRenderer _imGuiRenderer;
    private SpriteBatch _spriteBatch;
    private GraphicsDeviceManager _graphicsDeviceManager;
    private Desktop _desktop;

    // TODO: Nez has cool physics & other projects libs to use as deps, ImGUI viewports, solutions, etc.

    // https://gafferongames.com/post/fix_your_timestep/
    // https://lajbert.wordpress.com/2021/05/02/fix-your-timestep-in-monogame/
    private RootFeature _rootFeature;
    private float _fixedDeltaTime;

    public Game(ILogger logger, IServiceContainer container)
    {
        _logger = logger;
        _container = container;

        _logger.ForContext<Game>().Verbose("ctor");
    }

    protected override void Initialize()
    {
        _logger.ForContext<Game>().Verbose($"Initialize(): start; available {GraphicsDevice}");
        _logger.ForContext<Game>().Verbose("Circular dependencies (external) initialization...");
        RegisterGraphicsDeviceManager();
        RegisterSpriteBatch();
        _logger.ForContext<Game>().Verbose("Circular dependencies (external) initialized");
        RegisterWindowSettings();

        _logger.ForContext<Game>().Verbose("Game services initialization...");
        RegisterRootFeature();
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

        // Register UIs before systems onAwake, because we subscribe on systems' events:
        // System ctor() -> UI ctor(System) -> System onAwake & event raise -> UI onEvent
#if DEBUG
        RegisterImGuiRenderer();
#endif
        RegisterMyraUIEnvironment();
        RegisterMyraUI();

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
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _rootFeature.OnUpdate(deltaTime);

        FixedUpdate(deltaTime);

        _rootFeature.OnLateUpdate(deltaTime);
    }

    private void FixedUpdate(float deltaTime)
    {
        _fixedDeltaTime += deltaTime;
        while (_fixedDeltaTime >= TargetElapsedTime.TotalSeconds)
        {
            _rootFeature.OnFixedUpdate(_fixedDeltaTime);
            _fixedDeltaTime -= (float)TargetElapsedTime.TotalSeconds;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _imGuiRenderer?.BeginLayout(gameTime);

        // ImGui.ShowMetricsWindow();

        // For zoom: transformMatrix: Matrix.CreateScale(scaleFactor, scaleFactor, 1f);
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

    private void RegisterWindowSettings()
    {
        Window.AllowUserResizing = IsWindowResizable;
    }

    private void RegisterGraphicsDeviceManager()
    {
        _graphicsDeviceManager = _container.GetInstance<GraphicsDeviceManager>();
        _container.RegisterInstance(GraphicsDevice);
    }

    private void RegisterSpriteBatch()
    {
        _container.RegisterSingleton(_ => new SpriteBatch(_container.GetInstance<GraphicsDevice>()));
        _spriteBatch = _container.GetInstance<SpriteBatch>();
    }

    private void RegisterRootFeature()
    {
        _container.RegisterFrom<RootFeatureCompositionRoot>();
        _rootFeature = _container.GetInstance<RootFeature>();
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

        ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable | ImGuiConfigFlags.ViewportsEnable;
    }

    private void RegisterMyraUIEnvironment() => MyraEnvironment.Game = _container.GetInstance<Game>();

    private void RegisterMyraUI() => _desktop = _container.GetInstance<Desktop>();
}
