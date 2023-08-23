using System;
using System.Collections.Generic;
using System.IO;
using Models.Aseprite;
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
using SpriteSheet = Models.Aseprite.SpriteSheet;

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
        ServiceContainer container = new();
        container.Register<IMovement, SimpleMovement>();
        container.Register<IInputScanner, KeyboardScanner>();
        // TODO: PlayerViewAnimated
        AsepriteFile asepriteFile = AsepriteFile.Load(SpriteSheets.Player);
        container.Register(factory =>
            new PlayerView(factory.GetInstance<IInputScanner>(),
                new Dictionary<RadDir, AnimatedSprite>
                {
                    {
                        RadDir.Right,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingRight")
                    },

                    // Needs UpRight sprite
                    {
                        RadDir.UpRight,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingRight")
                    },
                    {
                        RadDir.Up,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingUp")
                    },

                    // Needs UpLeft sprite
                    {
                        RadDir.UpLeft,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingUp")
                    },
                    {
                        RadDir.Left,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingRight")
                    },

                    // Needs DownLeft sprite
                    {
                        RadDir.DownLeft,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingDown")
                    },
                    {
                        RadDir.Down,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingDown")
                    },

                    // Needs DownRight sprite
                    {
                        RadDir.DownRight,
                        SpriteSheetProcessor.Process(GraphicsDevice, asepriteFile).CreateAnimatedSprite("WalkingRight")
                    }
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
        SpriteSheet spriteSheet = SpriteSheet.FromJson(content);

        Console.WriteLine((content, spriteSheet.ToJson()));

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
