using System.Collections.Generic;
using System.IO;
using Entities;
using FrontEnd;
using GameDesktop.Resources;
using LightInject;
using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using Services;
using SpriteSheet = MonoGame.Aseprite.Sprites.SpriteSheet;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private const string ContentRootDirectory = "Content";
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player _player;

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = ContentRootDirectory;

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Really, XNA or MonoGame have created a puzzle by passing "this" into GraphicsDeviceManager,
        // creating an unsolvable circular dependency problem, by doing so.
        // So, for now, ole tha entry-point logic will be here.
        // TODO: Normalize DI workflow
        using ServiceContainer container = new();

        container.Register<IMovement, SimpleMovement>();
        container.Register<IInputScanner, KeyboardScanner>();

        // TODO: PlayerViewAnimated
        AsepriteFile asepriteFile = AsepriteFile.Load(SpriteSheets.Player);
        container.Register(_ => SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile));
        // TODO: State Machine for animations

        container.Register(
            factory =>
            {
                AnimatedSprite walkingRight =
                    factory.GetInstance<SpriteSheet>().CreateAnimatedSprite("WalkingRight");
                walkingRight.Play();
                return walkingRight;
            },
            "WalkingRight");

        container.Register(
            factory =>
            {
                AnimatedSprite animatedSprite = factory.GetInstance<AnimatedSprite>("WalkingRight");
                animatedSprite.FlipHorizontally = true;
                animatedSprite.Play();
                return animatedSprite;
            },
            "WalkingLeft"
        );

        container.Register(
            factory =>
            {
                var walkingUp = factory.GetInstance<SpriteSheet>().CreateAnimatedSprite("WalkingUp");
                walkingUp.Play();
                return walkingUp;
            },
            "WalkingUp"
        );

        container.Register(
            factory =>
            {
                var walkingDown = factory.GetInstance<SpriteSheet>().CreateAnimatedSprite("WalkingDown");
                walkingDown.Play();
                return walkingDown;
            },
            "WalkingDown"
        );

        container.Register(factory =>
            new PlayerView(factory.GetInstance<IInputScanner>(),
                new Dictionary<RadDir, AnimatedSprite>
                {
                    { RadDir.Right, factory.GetInstance<AnimatedSprite>("WalkingRight") },

                    // Needs UpRight sprite
                    { RadDir.UpRight, factory.GetInstance<AnimatedSprite>("WalkingRight") },
                    { RadDir.Up, factory.GetInstance<AnimatedSprite>("WalkingUp") },

                    // Needs UpLeft sprite
                    { RadDir.UpLeft, factory.GetInstance<AnimatedSprite>("WalkingUp") },
                    { RadDir.Left, factory.GetInstance<AnimatedSprite>("WalkingLeft") },

                    // Needs DownLeft sprite
                    { RadDir.DownLeft, factory.GetInstance<AnimatedSprite>("WalkingDown") },
                    { RadDir.Down, factory.GetInstance<AnimatedSprite>("WalkingDown") },

                    // Needs DownRight sprite
                    { RadDir.DownRight, factory.GetInstance<AnimatedSprite>("WalkingRight") }
                }
            ));

        container.Register(factory =>
            new Player(factory.GetInstance<IMovement>(),
                factory.GetInstance<IInputScanner>(), factory.GetInstance<PlayerView>()));

        // example of importing JSON data config
        // TODO: Configs as a class, so we can call it Configs.Player.<Action>.<Direction> or make a tool for autogen?
        string content = File.ReadAllText(Configs.PlayerRunningUp);

        // TODO: dictionary with a key containing png (spritesheet) and a value of JSON data (from aseprite).
        // Then, this is what a View class should have passing tru
        // SpriteSheet spriteSheet = SpriteSheet.FromJson(content);

        // Console.WriteLine((content, spriteSheet.ToJson()));

        _player = container.GetInstance<Player>();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        _player.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _player.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}
