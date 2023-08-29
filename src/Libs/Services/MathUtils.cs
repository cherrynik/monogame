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
    private static double RadDir(float x, float y, int sectors)
    {
        double radians = Math.Atan2(y, x);

        // Normalize the angle to the range [0, 2π)
        radians %= 2 * Math.PI;
        if (radians < 0) radians += 2 * Math.PI;

        return Math.Floor(radians / (2 * Math.PI) * sectors);
    }

    public static RadDir Rad8Dir(Vector2 dir) => (RadDir)RadDir(dir.X, dir.Y, sectors: 8);

    // Useful as MonoGame has Y-flipped coordinate system
    public static RadDir Rad8DirYFlipped(Vector2 dir) => (RadDir)RadDir(dir.X, -dir.Y, sectors: 8);

    public static RadDir Rad4Dir(Vector2 dir) => (RadDir)RadDir(dir.X, dir.Y, sectors: 4);

    // Useful as MonoGame has Y-flipped coordinate system
    public static RadDir Rad4DirYFlipped(Vector2 dir) => (RadDir)RadDir(dir.X, dir.Y, sectors: 4);
}
