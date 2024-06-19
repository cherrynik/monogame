using System.Numerics;
using Services.Math;

namespace UnitTests.Services;

public class SectorTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Right()
    {
        Vector2 right = new(.5f, 0);
        Sector sector = MathUtils.VectorToSector(right);
        Assert.That(sector, Is.EqualTo(Sector.Right));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(right.X), float.Sign(right.Y))));
    }

    [Test]
    public void UpRight()
    {
        Vector2 upRight = new(.5f, .5f);
        Sector sector = MathUtils.VectorToSector(upRight);
        Assert.That(sector, Is.EqualTo(Sector.UpRight));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(upRight.X), float.Sign(upRight.Y))));
    }

    [Test]
    public void Up()
    {
        Vector2 up = new(0, .5f);
        Sector sector = MathUtils.VectorToSector(up);
        Assert.That(sector, Is.EqualTo(Sector.Up));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(up.X), float.Sign(up.Y))));
    }

    [Test]
    public void UpLeft()
    {
        Vector2 upLeft = new(-.5f, .5f);
        Sector sector = MathUtils.VectorToSector(upLeft);
        Assert.That(sector, Is.EqualTo(Sector.UpLeft));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(upLeft.X), float.Sign(upLeft.Y))));
    }

    [Test]
    public void Left()
    {
        Vector2 left = new(-.5f, 0);
        Sector sector = MathUtils.VectorToSector(left);
        Assert.That(sector, Is.EqualTo(Sector.Left));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(left.X), float.Sign(left.Y))));
    }

    [Test]
    public void DownLeft()
    {
        Vector2 downLeft = new(-.5f, -.5f);
        Sector sector = MathUtils.VectorToSector(downLeft);
        Assert.That(sector, Is.EqualTo(Sector.DownLeft));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(downLeft.X), float.Sign(downLeft.Y))));
    }


    [Test]
    public void Down()
    {
        Vector2 down = new(0, -.5f);
        Sector sector = MathUtils.VectorToSector(down);
        Assert.That(sector, Is.EqualTo(Sector.Down));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(down.X), float.Sign(down.Y))));
    }

    [Test]
    public void DownRight()
    {
        Vector2 downRight = new(.5f, -.5f);
        Sector sector = MathUtils.VectorToSector(downRight);
        Assert.That(sector, Is.EqualTo(Sector.DownRight));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(downRight.X), float.Sign(downRight.Y))));
    }
}
