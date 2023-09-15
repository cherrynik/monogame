using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog.Core;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly Logger _logger;
    private readonly Contexts _contexts;

    public SpriteBatch SpriteBatch { get; private set; }

    public Game(
        Logger logger,
        Contexts contexts)
    {
        _logger = logger;
        _contexts = contexts;

        _logger.ForContext<Game>().Verbose("ctor");
    }

    protected override void Initialize()
    {
        _logger.ForContext<Game>().Verbose($"Initialize(): start; available {GraphicsDevice}");
        _logger.ForContext<Game>().Verbose("SpriteBatch initialization...");

        SpriteBatch = new SpriteBatch(GraphicsDevice);

        _logger.ForContext<Game>().Verbose("SpriteBatch initialized");

        base.Initialize();

        _logger.ForContext<Game>().Verbose("Initialize(): end");
    }

    protected override void LoadContent()
    {
        _logger.ForContext<Game>().Verbose("LoadContent(): start");

        // TODO: DI with ECS?
        // TODO: Projects management (external in Libs/External?)
        // TODO: Logging with game flags (like LOG_MOVEMENT, etc)?
        // TODO: Error handling
        // _gameFeature.Initialize();
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

    private void FixedUpdate(GameTime fixedGameTime)
    {
        // _gameFeature.FixedExecute(fixedGameTime);
    }

    protected override void Update(GameTime gameTime)
    {
        FixedUpdate(gameTime);

        base.Update(gameTime);
        // _gameFeature.Execute(gameTime);

        LateUpdate(gameTime);
    }

    private void LateUpdate(GameTime gameTime)
    {
        // _gameFeature.LateExecute(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // _gameFeature.Draw(gameTime, _spriteBatch);

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        _logger.ForContext<Game>().Verbose("Disposing...");

        base.Dispose(disposing);

        _logger.ForContext<Game>().Verbose("Disposed");
    }
}
