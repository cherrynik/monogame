using Microsoft.Xna.Framework;
using Services;

namespace UnitTests.Services;

public class Rad8Dir
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Right()
    {
        Vector2 right = new(1, 0);
        RadDir radDir = MathUtils.Rad8Dir(right);
        Assert.That(radDir, Is.EqualTo(RadDir.Right));
    }

    [Test]
    public void UpRight()
    {
        Vector2 upRight = new(1, 1);
        RadDir radDir = MathUtils.Rad8Dir(upRight);
        Assert.That(radDir, Is.EqualTo(RadDir.UpRight));
    }

    [Test]
    public void Up()
    {
        Vector2 up = new(0, 1);
        RadDir radDir = MathUtils.Rad8Dir(up);
        Assert.That(radDir, Is.EqualTo(RadDir.Up));
    }

    [Test]
    public void UpLeft()
    {
        Vector2 upLeft = new(-1, 1);
        RadDir radDir = MathUtils.Rad8Dir(upLeft);
        Assert.That(radDir, Is.EqualTo(RadDir.UpLeft));
    }

    [Test]
    public void Left()
    {
        Vector2 left = new(-1, 0);
        RadDir radDir = MathUtils.Rad8Dir(left);
        Assert.That(radDir, Is.EqualTo(RadDir.Left));
    }

    [Test]
    public void DownLeft()
    {
        Vector2 downLeft = new(-1, -1);
        RadDir radDir = MathUtils.Rad8Dir(downLeft);
        Assert.That(radDir, Is.EqualTo(RadDir.DownLeft));
    }


    [Test]
    public void Down()
    {
        Vector2 down = new(0, -1);
        RadDir radDir = MathUtils.Rad8Dir(down);
        Assert.That(radDir, Is.EqualTo(RadDir.Down));
    }

    [Test]
    public void DownRight()
    {
        Vector2 downRight = new(1, -1);
        RadDir radDir = MathUtils.Rad8Dir(downRight);
        Assert.That(radDir, Is.EqualTo(RadDir.DownRight));
    }
}
