using Components.Sprites;
using Entitas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;
using Stateless;
using Vector2 = System.Numerics.Vector2;

namespace Systems;

public sealed class CreateEntity : IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly MovementAnimatedSprites _movementAnimatedSprites;

    public CreateEntity(Contexts contexts, MovementAnimatedSprites movementAnimatedSprites)
    {
        _contexts = contexts;
        _movementAnimatedSprites = movementAnimatedSprites;
    }

    public void Initialize()
    {
        CreatePlayer();

        Console.WriteLine("Initialized.");
    }

    private void CreatePlayer()
    {
        GameEntity? e = _contexts.game.CreateEntity();

        e.isPlayer = true;
        e.isMovable = true;

        e.AddTransform(new Vector2(), new Vector2());
        e.AddRectangleCollision(new Rectangle(0, 0, 16, 16));

        e.ReplaceComponent(0, _movementAnimatedSprites);
    }
}
