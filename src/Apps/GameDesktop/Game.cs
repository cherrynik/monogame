﻿using Entities;
using GameDesktop.Resources;
using LightInject;
using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    private void ConfigureServices()
    {
        RegisterSpriteServices();

        RegisterMovementServices();
        RegisterPlayer();

        _player = _container.GetInstance<Player>();
    }

    private void RegisterSpriteServices()
    {
        _container.Register(_ => _spriteBatch);
    }

    private void RegisterMovementServices()
    {
        _container.Register<IMovement, SimpleMovement>();
        _container.Register<IInputScanner, KeyboardScanner>();
    }

    private void RegisterPlayerStateMachine()
    {
        _container.Register(_ => new StateMachine<PlayerState, PlayerTrigger>(PlayerState.Idle));
    }

    private void RegisterPlayer()
    {
        RegisterPlayerStateMachine();

        SpriteSheet spriteSheet = AnimatedCharactersFactory.LoadSpriteSheet(GraphicsDevice, SpriteSheets.Player);
        AnimatedCharactersFactory animatedCharactersFactory = new();

        _container.Register(factory => new PlayerView(
            factory.GetInstance<StateMachine<PlayerState, PlayerTrigger>>(),
            animatedCharactersFactory.CreateAnimations(spriteSheet, "Standing"),
            animatedCharactersFactory.CreateAnimations(spriteSheet, "Walking")
        ));

        _container.Register(factory =>
            new Player(factory.GetInstance<IMovement>(),
                factory.GetInstance<IInputScanner>(),
                factory.GetInstance<PlayerView>()
            ));
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
