using System;
using Features;
using GameDesktop.CompositionRoots.Features;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly float _targetTimeStep;

    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    private Entitas.Extended.Feature _rootFeature;
    private SpriteBatch _spriteBatch;
    private float _accumulatedTime;


    public Game(ILogger logger, IServiceContainer container, float targetFramesPerSecond)
    {
        _logger = logger;
        _container = container;
        _targetTimeStep = 1 / targetFramesPerSecond;

        _logger.ForContext<Game>().Verbose("ctor");
    }

    protected override void Initialize()
    {
        _logger.ForContext<Game>().Verbose($"Initialize(): start; available {GraphicsDevice}");
        _logger.ForContext<Game>().Verbose("SpriteBatch initialization...");

        _container.RegisterSingleton(_ => new SpriteBatch(GraphicsDevice));
        _spriteBatch = _container.GetInstance<SpriteBatch>();
        // GraphicsDeviceManager.PreferredBackBufferWidth = 640;
        // GraphicsDeviceManager.PreferredBackBufferHeight = 480;
        // GraphicsDeviceManager.ApplyChanges();

        _logger.ForContext<Game>().Verbose("SpriteBatch initialized");

        base.Initialize();

        _logger.ForContext<Game>().Verbose("Initialize(): end");
    }

    protected override void LoadContent()
    {
        _logger.ForContext<Game>().Verbose("LoadContent(): start");

        _container.RegisterFrom<RootFeatureCompositionRoot>();

        _rootFeature = _container.GetInstance<RootFeature>();

        // TODO: Logging with game flags (like LOG_MOVEMENT, etc)?
        // todo: pass tru logger & log places
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

    // maybe fixed update is incorrect. needs review in the future
    private void FixedUpdate(GameTime gameTime) => _rootFeature.FixedExecute(gameTime);

    protected override void Update(GameTime gameTime)
    {
        _accumulatedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        while (_accumulatedTime >= _targetTimeStep)
        {
            FixedUpdate(gameTime);

            _accumulatedTime -= _targetTimeStep;
        }

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
