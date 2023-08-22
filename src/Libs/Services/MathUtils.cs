using Microsoft.Xna.Framework;

namespace Services;

public enum RadDir
{
    Right,
    UpRight,
    Up,
    UpLeft,
    Left,
    DownLeft,
    Down,
    DownRight
}

public static class MathUtils
{
    private static double Rad8Dir(float x, float y)
    {
        double radians = Math.Atan2(y, x);

        // Normalize the angle to the range [0, 2π)
        radians %= 2 * Math.PI;
        if (radians < 0) radians += 2 * Math.PI;

        const int circleDirections = 8;
        return Math.Floor(radians / (2 * Math.PI) * circleDirections);
    }

    public static RadDir Rad8Dir(Vector2 v) => (RadDir)Rad8Dir(v.X, v.Y);
}
