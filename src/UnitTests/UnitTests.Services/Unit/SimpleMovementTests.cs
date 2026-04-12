using System.Numerics;
using Services.Movement;

namespace UnitTests.Services;

public class SimpleMovementTests
{
    [Test]
    public void Move_ZeroVelocity_ReturnsOriginalPosition()
    {
        var movement = new SimpleMovement();
        var from = new Vector2(10f, 5f);

        Vector2 result = movement.Move(from, Vector2.Zero);

        Assert.That(result, Is.EqualTo(from));
    }

    [Test]
    public void Move_NonZeroVelocity_UsesNormalizedStep()
    {
        var movement = new SimpleMovement();
        var from = Vector2.Zero;
        var velocity = new Vector2(3f, 4f);

        Vector2 result = movement.Move(from, velocity);

        Assert.That(result.X, Is.EqualTo(0.6f).Within(0.0001f));
        Assert.That(result.Y, Is.EqualTo(0.8f).Within(0.0001f));
    }
}
