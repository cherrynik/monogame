using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Components.Data;

// Camera is a system and not a component, so the system has a target as a dependency,
// and exists only in the current world, as well the system has its own behaviour
public struct CameraComponent(Viewport viewport) : IComponent
{
    public Viewport Viewport = viewport;
    public Vector2 Position;

    public Vector2 WorldToScreen(Vector2 other) => other - Position;
}
