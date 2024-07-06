using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Vector2 = System.Numerics.Vector2;

namespace Components.Data;

// The Camera is a system and not a component, so the system has a target as a dependency,
// and exists only in the current world, as well the system has its own behaviour
public struct CameraComponent(Viewport viewport) : IComponent
{
    public Viewport Viewport = viewport;
    public Vector2 Position;

    public Vector2 WorldToScreen(Vector2 other) => other - Position;

    public Vector2 GetCenteredPosition(Viewport viewport, Vector2 off)
    {
        var cameraX = off.X - (float)viewport.Width / 2;
        var cameraY = off.Y - (float)viewport.Height / 2;

        cameraX = MathHelper.Clamp(cameraX, 0, 900 - viewport.Width);
        cameraY = MathHelper.Clamp(cameraY, 0, 600 - viewport.Height);

        return new Vector2(cameraX, cameraY);
    }
}
