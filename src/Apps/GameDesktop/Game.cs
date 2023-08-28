using System.Collections.Generic;
using Entities;
using GameDesktop.Resources;
using LightInject;
using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using Services;
using Stateless;
using SpriteSheet = MonoGame.Aseprite.Sprites.SpriteSheet;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly ServiceContainer _container;
    private SpriteBatch _spriteBatch;
    private Player _player;

    public Game(ServiceContainer container)
    {
        _container = container;
    }

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        ConfigureServices();
    }

    private void RegisterSpriteServices()
    {
        AsepriteFile asepriteFile = AsepriteFile.Load(SpriteSheets.Player);
        _container.Register(_ => SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile));
    }

    private void RegisterMovementServices()
    {
        _container.Register<IMovement, SimpleMovement>();
        _container.Register<IInputScanner, KeyboardScanner>();
    }

    private void RegisterPlayerAnimatedSprites()
    {
        // TODO: Use in state machine, and switch between: run, walk, etc.
        _container.Register<string, AnimatedSprite>((factory, s) =>
            factory.GetInstance<SpriteSheet>().CreateAnimatedSprite(s));

        IReadOnlyList<string> tags = new[] { "Standing", "Walking" };
        IReadOnlyList<RadDir> dirs = new[] { RadDir.Right, RadDir.Down, RadDir.Left, RadDir.Up };

        // TODO: Simplify
        foreach (RadDir dir in dirs)
        {
            foreach (string tag in tags)
            {
                string animationTag = $"{tag}{dir.ToString()}";
                _container.Register(factory =>
                {
                    AnimatedSprite animatedSprite;
                    if (dir.ToString() == RadDir.Left.ToString())
                    {
                        animatedSprite = factory.GetInstance<string, AnimatedSprite>($"{tag}{RadDir.Right}");
                        animatedSprite.FlipHorizontally = true;
                    }
                    else
                    {
                        animatedSprite = factory.GetInstance<string, AnimatedSprite>(animationTag);
                    }

                    animatedSprite.Play();
                    return animatedSprite;
                }, animationTag);
            }
        }
    }

    private Dictionary<RadDir, AnimatedSprite> CreatePlayerAnimation(string action)
    {
        return new Dictionary<RadDir, AnimatedSprite>
        {
            { RadDir.Right, _container.GetInstance<AnimatedSprite>($"{action}Right") },

            // Needs UpRight sprite
            { RadDir.UpRight, _container.GetInstance<AnimatedSprite>($"{action}Right") },
            { RadDir.Up, _container.GetInstance<AnimatedSprite>($"{action}Up") },

            // Needs UpLeft sprite
            { RadDir.UpLeft, _container.GetInstance<AnimatedSprite>($"{action}Left") },
            { RadDir.Left, _container.GetInstance<AnimatedSprite>($"{action}Left") },

            // Needs DownLeft sprite
            { RadDir.DownLeft, _container.GetInstance<AnimatedSprite>($"{action}Left") },
            { RadDir.Down, _container.GetInstance<AnimatedSprite>($"{action}Down") },

            // Needs DownRight sprite
            { RadDir.DownRight, _container.GetInstance<AnimatedSprite>($"{action}Right") }
        };
    }

    private void RegisterPlayerStateMachine()
    {
        _container.Register(_ => new StateMachine<PlayerState, PlayerTrigger>(PlayerState.Idle));
    }

    private void RegisterPlayer()
    {
        RegisterPlayerAnimatedSprites();
        RegisterPlayerStateMachine();

        _container.Register(factory =>
            new Player(factory.GetInstance<IMovement>(),
                factory.GetInstance<IInputScanner>(),
                factory.GetInstance<StateMachine<PlayerState, PlayerTrigger>>(),
                CreatePlayerAnimation("Walking"),
                CreatePlayerAnimation("Standing")));
    }

    private void ConfigureServices()
    {
        _container.Register(_ => _spriteBatch);

        RegisterSpriteServices();

        RegisterMovementServices();
        RegisterPlayer();

        _player = _container.GetInstance<Player>();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _player.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _player.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}
