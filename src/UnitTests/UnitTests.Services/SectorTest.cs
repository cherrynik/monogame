using System.Numerics;
using Services.Implementations.Math;

namespace UnitTests.Services;

public class SectorTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Center()
    {
        Vector2 center = new(0, 0);
        Sector sector = MathUtils.VectorToSector(center);
        Assert.That(sector, Is.EqualTo(Sector.Right));

        Vector2 inverted = MathUtils.SectorToVector(Sector.Center);
        Assert.That(inverted, Is.EqualTo(new Vector2(float.Sign(center.X), float.Sign(center.Y))));
    }

    [Test]
    public void Right()
    {
        Vector2 right = new(.5f, 0);
        Sector sector = MathUtils.VectorToSector(right);
        Assert.That(sector, Is.EqualTo(Sector.Right));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(1, 0);
        Assert.That(inverted, Is.EqualTo(expected));
    }

    [Test]
    public void UpRight()
    {
        Vector2 upRight = new(.5f, .5f);
        Sector sector = MathUtils.VectorToSector(upRight);
        Assert.That(sector, Is.EqualTo(Sector.UpRight));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(1, 1);
        Assert.That(inverted, Is.EqualTo(expected));
    }

    [Test]
    public void Up()
    {
        Vector2 up = new(0, .5f);
        Sector sector = MathUtils.VectorToSector(up);
        Assert.That(sector, Is.EqualTo(Sector.Up));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(0, 1);
        Assert.That(inverted, Is.EqualTo(expected));
    }

    [Test]
    public void UpLeft()
    {
        Vector2 upLeft = new(-.5f, .5f);
        Sector sector = MathUtils.VectorToSector(upLeft);
        Assert.That(sector, Is.EqualTo(Sector.UpLeft));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(-1, 1);
        Assert.That(inverted, Is.EqualTo(expected));
    }

    [Test]
    public void Left()
    {
        Vector2 left = new(-.5f, 0);
        Sector sector = MathUtils.VectorToSector(left);
        Assert.That(sector, Is.EqualTo(Sector.Left));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(-1, 0);
        Assert.That(inverted, Is.EqualTo(expected));
    }

    [Test]
    public void DownLeft()
    {
        Vector2 downLeft = new(-.5f, -.5f);
        Sector sector = MathUtils.VectorToSector(downLeft);
        Assert.That(sector, Is.EqualTo(Sector.DownLeft));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(-1, -1);
        Assert.That(inverted, Is.EqualTo(expected));
    }


    [Test]
    public void Down()
    {
        Vector2 down = new(0, -.5f);
        Sector sector = MathUtils.VectorToSector(down);
        Assert.That(sector, Is.EqualTo(Sector.Down));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(0, -1);
        Assert.That(inverted, Is.EqualTo(expected));
    }

    [Test]
    public void DownRight()
    {
        Vector2 downRight = new(.5f, -.5f);
        Sector sector = MathUtils.VectorToSector(downRight);
        Assert.That(sector, Is.EqualTo(Sector.DownRight));

        Vector2 inverted = MathUtils.SectorToVector(sector);
        Vector2 expected = new Vector2(1, -1);
        Assert.That(inverted, Is.EqualTo(expected));
    }
}
