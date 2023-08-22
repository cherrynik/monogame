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
    const int Sectors = 8;

    private static double Rad8Dir(float x, float y)
    {
        double radians = Math.Atan2(y, x);

        // Normalize the angle to the range [0, 2π)
        radians %= 2 * Math.PI;
        if (radians < 0) radians += 2 * Math.PI;

        return Math.Floor(radians / (2 * Math.PI) * Sectors);
    }

    public static RadDir Rad8Dir(Vector2 dir) => (RadDir)Rad8Dir(dir.X, dir.Y);

    // Useful as MonoGame has Y-flipped coordinate system
    public static RadDir Rad8DirYFlipped(Vector2 dir) => (RadDir)Rad8Dir(dir.X, -dir.Y);
}
