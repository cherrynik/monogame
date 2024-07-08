using System.Numerics;
using Components.Data;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Components.Render;

public interface IRenderComponent : IComponent
{
    public TransformComponent LocalTransform { get; }
    public Vector2 GetOffPivot(TransformComponent parentTransform);
    public void Draw(SpriteBatch spriteBatch, Vector2 at);
}
