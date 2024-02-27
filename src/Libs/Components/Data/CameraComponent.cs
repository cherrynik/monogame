using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Components.Data;

// TODO: Camera is a system and not a component,
// So, the system would have a target as a dependency,
// And exist only in the current world,
// As well, the system has its own behaviour
public struct CameraComponent(Viewport viewport) : IComponent
{
    public Viewport Viewport = viewport;
    public Vector2 Position;

    public Vector2 WorldToScreen(Vector2 other) => other - Position;
}
