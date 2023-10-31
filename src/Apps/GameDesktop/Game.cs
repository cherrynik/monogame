using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Entities;
using GameDesktop.CompositionRoots.Features;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Serilog;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    private SpriteBatch _spriteBatch;

    // TODO: Frames updating
    // https://gafferongames.com/post/fix_your_timestep/
    // https://lajbert.wordpress.com/2021/05/02/fix-your-timestep-in-monogame/

    // TODO: Nez has cool physics & other projects to use in the project as deps

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

        World world = World.Create();
        var player = _container.GetInstance<PlayerEntity>();
        player.Create(@in: world);

        var dummy = _container.GetInstance<DummyEntity>();
        dummy.Create(@in: world);

        // _logger.ForContext<Game>().Verbose(e.ToString()!);
        // _logger.ForContext<Game>().Verbose(e2.ToString()!);

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

    private void FixedUpdate(GameTime gameTime)
    {
    }

    protected override void Update(GameTime gameTime)
    {
        FixedUpdate(gameTime);

        base.Update(gameTime);

        LateUpdate(gameTime);
    }

    private void LateUpdate(GameTime gameTime)
    {
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        _logger.ForContext<Game>().Verbose("Disposing...");

        base.Dispose(disposing);

        _logger.ForContext<Game>().Verbose("Disposed");
    }
}
