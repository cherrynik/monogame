using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Components.Tags;

public struct CameraComponent(Viewport viewport) : IComponent
{
    public Viewport Viewport = viewport;
    public Vector2 Position;
}
