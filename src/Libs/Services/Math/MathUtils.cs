using System.Numerics;
using Vector2 = System.Numerics.Vector2;

namespace Services.Math;

public enum Sector
{
    Right = 0,
    UpRight = 1,
    Up = 2,
    UpLeft = 3,
    Left = 4,
    DownLeft = 5,
    Down = 6,
    DownRight = 7,
    Center = 8
}

public static class MathUtils
{
    private static double GetRadianSector(float x, float y, int sectors)
    {
        double radians = System.Math.Atan2(System.Math.Sign(y), System.Math.Sign(x));

        // Normalize the angle to the range [0, 2π)
        radians %= 2 * System.Math.PI;
        if (radians < 0) radians += 2 * System.Math.PI;

        return System.Math.Floor(radians / (2 * System.Math.PI) * sectors);
    }

    public static Vector2 SectorToVector(Sector sector)
    {
        const int sectors = 8;

        var angle = 360.0 / sectors * (int)sector;

        double radians = angle * (MathF.PI / 180);

        return RadiansToVector(radians);
    }

    public static Sector VectorToSector(Vector2 dir) => (Sector)GetRadianSector(dir.X, dir.Y, sectors: 8);

    // Useful as MonoGame has Y-flipped coordinate system

    public static Sector VectorToSectorYFlipped(Vector2 dir) => (Sector)GetRadianSector(dir.X, -dir.Y, sectors: 8);

    public static Sector VectorTo4Sector(Vector2 dir) => (Sector)GetRadianSector(dir.X, dir.Y, sectors: 4);

    // Useful as MonoGame has Y-flipped coordinate system

    public static Sector VectorTo4SectorYFlipped(Vector2 dir) => (Sector)GetRadianSector(dir.X, dir.Y, sectors: 4);

    public static Sector LdtkPivotToSector(Vector2 entityPivot)
    {
        // LDtk pivot at down center on 3x3 grid has coords of [0f, 0.5f]
        return entityPivot.Equals(Vector2.Zero)
            ? Sector.Center
            : VectorToSectorYFlipped(new Vector2(entityPivot.X - .5f, entityPivot.Y - .5f));
    }

    private static Vector2 RadiansToVector(double radians)
    {
        var x = System.Math.Cos(radians);
        var y = System.Math.Sin(radians);
        return new Vector2((float)System.Math.Round(x), (float)System.Math.Round(y));
    }
}
