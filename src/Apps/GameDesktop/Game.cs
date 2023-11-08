using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Entities;
using GameDesktop.CompositionRoots.Features;
using ImGuiNET;
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

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly ILogger _logger;
    private readonly IServiceContainer _container;

    private ImGuiRenderer _guiRenderer;
    private SpriteBatch _spriteBatch;

    private SystemsGroup _systemsGroup;
    private SystemsGroup _preRenderSystemsGroup;
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

        World world = World.Create();
        var player = _container.GetInstance<PlayerEntity>();
        player.Create(@in: world);

        var dummy = _container.GetInstance<DummyEntity>();
        dummy.Create(@in: world);

        _systemsGroup = world.CreateSystemsGroup();
        _systemsGroup.AddSystem(new InputSystem(world, new KeyboardInput()));
        _systemsGroup.AddSystem(new MovementSystem(world, new SimpleMovement()));

        _preRenderSystemsGroup = world.CreateSystemsGroup();
        _preRenderSystemsGroup.AddSystem(new RenderCharacterMovementSystem(world, _spriteBatch));

        _renderSystemsGroup = world.CreateSystemsGroup();
        _renderSystemsGroup.AddSystem(new CameraSystem(world,
            new FollowingCamera(_spriteBatch, new Viewport(0, 0, 800, 480))));

#if DEBUG
        _debugSystemsGroup = world.CreateSystemsGroup();
        _debugSystemsGroup.AddSystem(new EntitiesList(world));
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
        _preRenderSystemsGroup.Update(deltaTime);
        _renderSystemsGroup.Update(deltaTime);
        _spriteBatch.End();

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
