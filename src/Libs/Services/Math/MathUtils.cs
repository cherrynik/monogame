using Microsoft.Xna.Framework;

namespace Services.Math;

public enum Direction
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
        double radians = System.Math.Atan2(y, x);

        // Normalize the angle to the range [0, 2π)
        radians %= 2 * System.Math.PI;
        if (radians < 0) radians += 2 * System.Math.PI;

        return System.Math.Floor(radians / (2 * System.Math.PI) * sectors);
    }

    public static Direction Rad8Dir(Vector2 dir) => (Direction)RadDir(dir.X, dir.Y, sectors: 8);

    // Useful as MonoGame has Y-flipped coordinate system
    public static Direction Rad8DirYFlipped(Vector2 dir) => (Direction)RadDir(dir.X, -dir.Y, sectors: 8);

    public static Direction Rad4Dir(Vector2 dir) => (Direction)RadDir(dir.X, dir.Y, sectors: 4);

    // Useful as MonoGame has Y-flipped coordinate system
    public static Direction Rad4DirYFlipped(Vector2 dir) => (Direction)RadDir(dir.X, dir.Y, sectors: 4);
}
