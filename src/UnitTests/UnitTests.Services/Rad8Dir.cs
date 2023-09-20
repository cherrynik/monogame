using Microsoft.Xna.Framework;
using Services;
using Services.Math;

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
        Direction direction = MathUtils.Rad8Dir(right);
        Assert.That(direction, Is.EqualTo(Direction.Right));
    }

    [Test]
    public void UpRight()
    {
        Vector2 upRight = new(1, 1);
        Direction direction = MathUtils.Rad8Dir(upRight);
        Assert.That(direction, Is.EqualTo(Direction.UpRight));
    }

    [Test]
    public void Up()
    {
        Vector2 up = new(0, 1);
        Direction direction = MathUtils.Rad8Dir(up);
        Assert.That(direction, Is.EqualTo(Direction.Up));
    }

    [Test]
    public void UpLeft()
    {
        Vector2 upLeft = new(-1, 1);
        Direction direction = MathUtils.Rad8Dir(upLeft);
        Assert.That(direction, Is.EqualTo(Direction.UpLeft));
    }

    [Test]
    public void Left()
    {
        Vector2 left = new(-1, 0);
        Direction direction = MathUtils.Rad8Dir(left);
        Assert.That(direction, Is.EqualTo(Direction.Left));
    }

    [Test]
    public void DownLeft()
    {
        Vector2 downLeft = new(-1, -1);
        Direction direction = MathUtils.Rad8Dir(downLeft);
        Assert.That(direction, Is.EqualTo(Direction.DownLeft));
    }


    [Test]
    public void Down()
    {
        Vector2 down = new(0, -1);
        Direction direction = MathUtils.Rad8Dir(down);
        Assert.That(direction, Is.EqualTo(Direction.Down));
    }

    [Test]
    public void DownRight()
    {
        Vector2 downRight = new(1, -1);
        Direction direction = MathUtils.Rad8Dir(downRight);
        Assert.That(direction, Is.EqualTo(Direction.DownRight));
    }
}
