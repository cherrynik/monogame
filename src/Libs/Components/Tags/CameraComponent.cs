using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Components.Tags;

public struct CameraComponent : IComponent
{
    public Viewport Viewport;
    public Vector2 Position;

    public CameraComponent(Viewport viewport)
    {
        Viewport = viewport;
    }
}
