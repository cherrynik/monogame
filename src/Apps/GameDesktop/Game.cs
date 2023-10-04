using Features;
using GameDesktop.CompositionRoots;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    public GraphicsDeviceManager GraphicsDeviceManager;

    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    private Entitas.Extended.Feature _rootFeature;
    private SpriteBatch _spriteBatch;


    public Game(
        ILogger logger,
        IServiceContainer container)
    {
        _logger = logger;
        _container = container;

        _logger.ForContext<Game>().Verbose("ctor");
    }

    protected override void Initialize()
    {
        _logger.ForContext<Game>().Verbose($"Initialize(): start; available {GraphicsDevice}");
        _logger.ForContext<Game>().Verbose("SpriteBatch initialization...");

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        GraphicsDeviceManager.PreferredBackBufferWidth = 640;
        GraphicsDeviceManager.PreferredBackBufferHeight = 480;
        GraphicsDeviceManager.ApplyChanges();

        _logger.ForContext<Game>().Verbose("SpriteBatch initialized");

        base.Initialize();

        _logger.ForContext<Game>().Verbose("Initialize(): end");
    }

    protected override void LoadContent()
    {
        _logger.ForContext<Game>().Verbose("LoadContent(): start");

        _container.RegisterInstance(_spriteBatch);
        _container.RegisterFrom<RootFeatureCompositionRoot>();

        _rootFeature = _container.GetInstance<RootFeature>();

        // TODO: Logging with game flags (like LOG_MOVEMENT, etc)?
        // TODO: Error handling
        _rootFeature.Initialize();
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

    private void FixedUpdate(GameTime fixedGameTime) => _rootFeature.FixedExecute(fixedGameTime);

    protected override void Update(GameTime gameTime)
    {
        FixedUpdate(gameTime);

        base.Update(gameTime);
        _rootFeature.Execute(gameTime);

        LateUpdate(gameTime);
    }

    private void LateUpdate(GameTime gameTime) => _rootFeature.LateExecute(gameTime);

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _rootFeature.Draw(gameTime, _spriteBatch);

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        _logger.ForContext<Game>().Verbose("Disposing...");

        base.Dispose(disposing);

        _logger.ForContext<Game>().Verbose("Disposed");
    }
}
