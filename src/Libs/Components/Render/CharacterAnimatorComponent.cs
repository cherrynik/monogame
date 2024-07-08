using System.Numerics;
using Components.Data;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Components.Render;

public struct CharacterAnimatorComponent(Sector facing, AnimatedSprite animation, TransformComponent transform)
    : IRenderComponent
{
    public Sector Facing = facing;
    public AnimatedSprite Animation = animation;
    public TransformComponent LocalTransform { get; } = transform;

    public Vector2 GetOffPivot(TransformComponent parentTransform) =>
        parentTransform.GetOffPivot(Animation.Width, Animation.Height);

    public void Draw(SpriteBatch spriteBatch, Vector2 at) => Animation.Draw(spriteBatch, at);
}
