using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Vector2 = System.Numerics.Vector2;

namespace Components.Data;

// The Camera is a system and not a component, so the system has a target as a dependency,
// and exists only in the current world, as well the system has its own behavior
public struct CameraComponent(Viewport viewport) : IComponent
{
    public Viewport Viewport = viewport;
    public Vector2 Position;

    public Vector2 WorldToScreen(Vector2 other) => other - Position;

    private Vector2 GetCenteredPosition(Vector2 relativelyTo)
    {
        var cameraX = relativelyTo.X - (float)Viewport.Width / 2;
        var cameraY = relativelyTo.Y - (float)Viewport.Height / 2;

        return new Vector2(cameraX, cameraY);
    }

    public Vector2 GetCenteredPosition(Vector2 relativelyTo, Vector2 limitsByAxis)
    {
        var centeredPosition = GetCenteredPosition(relativelyTo);

        var clampedX = Math.Clamp(centeredPosition.X, WorldMetaComponent.ZeroPosition.X, limitsByAxis.X);
        var clampedY = Math.Clamp(centeredPosition.Y, WorldMetaComponent.ZeroPosition.Y, limitsByAxis.Y);

        return new Vector2(clampedX, clampedY);
    }
}
