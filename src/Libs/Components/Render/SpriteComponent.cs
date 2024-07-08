using System.Numerics;
using Components.Data;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;

namespace Components.Render;

public readonly struct SpriteComponent(Sprite sprite, TransformComponent localTransform) : IRenderComponent
{
    public Sprite Sprite { get; } = sprite;
    public TransformComponent LocalTransform { get; } = localTransform;

    public Vector2 GetOffPivot(TransformComponent parentTransform) =>
        parentTransform.GetOffPivot(Sprite.Width, Sprite.Height);

    public void Draw(SpriteBatch spriteBatch, Vector2 at) => Sprite.Draw(spriteBatch, at);
}
