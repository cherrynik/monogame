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
using Services;

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
        container.Register(factory =>
            new PlayerView(factory.GetInstance<IInputScanner>(),
                new Dictionary<RadDir, Texture2D>
                {
                    { RadDir.Right, Content.Load<Texture2D>(SpriteSheets.PlayerRunningRight) },

                    // Needs UpRight sprite
                    { RadDir.UpRight, Content.Load<Texture2D>(SpriteSheets.PlayerRunningRight) },
                    { RadDir.Up, Content.Load<Texture2D>(SpriteSheets.PlayerRunningUp) },

                    // Needs UpLeft sprite
                    { RadDir.UpLeft, Content.Load<Texture2D>(SpriteSheets.PlayerRunningLeft) },
                    { RadDir.Left, Content.Load<Texture2D>(SpriteSheets.PlayerRunningLeft) },

                    // Needs DownLeft sprite
                    { RadDir.DownLeft, Content.Load<Texture2D>(SpriteSheets.PlayerRunningLeft) },
                    { RadDir.Down, Content.Load<Texture2D>(SpriteSheets.PlayerRunningDown) },

                    // Needs DownRight sprite
                    { RadDir.DownRight, Content.Load<Texture2D>(SpriteSheets.PlayerRunningRight) }
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
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        _player.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _player.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}
