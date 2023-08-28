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
        // TODO: PlayerViewAnimated
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

        _container.Register(
            factory =>
            {
                AnimatedSprite walkingRight = factory.GetInstance<string, AnimatedSprite>("WalkingRight");
                walkingRight.Play();
                return walkingRight;
            },
            "WalkingRight");

        _container.Register(
            factory =>
            {
                AnimatedSprite standingRight = factory.GetInstance<string, AnimatedSprite>("StandingRight");
                standingRight.Play();
                return standingRight;
            },
            "StandingRight");

        _container.Register(
            factory =>
            {
                AnimatedSprite animatedSprite = factory.GetInstance<string, AnimatedSprite>("WalkingRight");
                animatedSprite.FlipHorizontally = true;
                animatedSprite.Play();
                return animatedSprite;
            },
            "WalkingLeft"
        );

        _container.Register(
            factory =>
            {
                AnimatedSprite animatedSprite = factory.GetInstance<string, AnimatedSprite>("StandingRight");
                animatedSprite.FlipHorizontally = true;
                animatedSprite.Play();
                return animatedSprite;
            },
            "StandingLeft");

        _container.Register(
            factory =>
            {
                var walkingUp = factory.GetInstance<string, AnimatedSprite>("WalkingUp");
                walkingUp.Play();
                return walkingUp;
            },
            "WalkingUp"
        );

        _container.Register(
            factory =>
            {
                AnimatedSprite standingUp = factory.GetInstance<string, AnimatedSprite>("StandingUp");
                standingUp.Play();
                return standingUp;
            },
            "StandingUp");


        _container.Register(
            factory =>
            {
                var walkingDown = factory.GetInstance<string, AnimatedSprite>("WalkingDown");
                walkingDown.Play();
                return walkingDown;
            },
            "WalkingDown"
        );

        _container.Register(
            factory =>
            {
                var standingDown = factory.GetInstance<string, AnimatedSprite>("StandingDown");
                standingDown.Play();
                return standingDown;
            },
            "StandingDown"
        );
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
                new Dictionary<RadDir, AnimatedSprite>
                {
                    { RadDir.Right, factory.GetInstance<AnimatedSprite>("WalkingRight") },

                    // Needs UpRight sprite
                    { RadDir.UpRight, factory.GetInstance<AnimatedSprite>("WalkingRight") },
                    { RadDir.Up, factory.GetInstance<AnimatedSprite>("WalkingUp") },

                    // Needs UpLeft sprite
                    { RadDir.UpLeft, factory.GetInstance<AnimatedSprite>("WalkingLeft") },
                    { RadDir.Left, factory.GetInstance<AnimatedSprite>("WalkingLeft") },

                    // Needs DownLeft sprite
                    { RadDir.DownLeft, factory.GetInstance<AnimatedSprite>("WalkingLeft") },
                    { RadDir.Down, factory.GetInstance<AnimatedSprite>("WalkingDown") },

                    // Needs DownRight sprite
                    { RadDir.DownRight, factory.GetInstance<AnimatedSprite>("WalkingRight") }
                }, new Dictionary<RadDir, AnimatedSprite>
                {
                    { RadDir.Right, factory.GetInstance<AnimatedSprite>("StandingRight") },

                    // Needs UpRight sprite
                    { RadDir.UpRight, factory.GetInstance<AnimatedSprite>("StandingRight") },
                    { RadDir.Up, factory.GetInstance<AnimatedSprite>("StandingUp") },

                    // Needs UpLeft sprite
                    { RadDir.UpLeft, factory.GetInstance<AnimatedSprite>("StandingLeft") },
                    { RadDir.Left, factory.GetInstance<AnimatedSprite>("StandingLeft") },

                    // Needs DownLeft sprite
                    { RadDir.DownLeft, factory.GetInstance<AnimatedSprite>("StandingLeft") },
                    { RadDir.Down, factory.GetInstance<AnimatedSprite>("StandingDown") },

                    // Needs DownRight sprite
                    { RadDir.DownRight, factory.GetInstance<AnimatedSprite>("StandingRight") }
                }));
    }

    private void ConfigureServices()
    {
        _container.Register(_ => _spriteBatch);

        RegisterSpriteServices();

        RegisterMovementServices();
        RegisterPlayer();

        // example of importing JSON data config
        // TODO: Configs as a class, so we can call it Configs.Player.<Action>.<Direction> or make a tool for autogen?
        // string content = File.ReadAllText(Configs.PlayerRunningUp);

        // TODO: dictionary with a key containing png (spritesheet) and a value of JSON data (from aseprite).
        // Then, this is what a View class should have passing tru
        // SpriteSheet spriteSheet = SpriteSheet.FromJson(content);

        // Console.WriteLine((content, spriteSheet.ToJson()));

        _player = _container.GetInstance<Player>();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // if (
        //     GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
        //     || Keyboard.GetState().IsKeyDown(Keys.Escape)
        // )
        //     Exit();

        _player.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _player.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}
