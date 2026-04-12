using Microsoft.Xna.Framework.Input;
using Systems.Input;
using Systems.Input.Abstractions;
using KeyboardInputImpl = Systems.Input.KeyboardInput;

namespace UnitTests.Entities;

public class KeyboardInputTests
{
    [Test]
    public void GetDirection_UsesWasdBindings()
    {
        var keyboardInput = new KeyboardInputImpl(new StubKeyboardStateSource(new KeyboardState(Keys.W, Keys.D)));

        var direction = keyboardInput.GetDirection();

        Assert.That(direction.X, Is.EqualTo(1));
        Assert.That(direction.Y, Is.EqualTo(-1));
    }

    [Test]
    public void GetDirection_UsesArrowBindings()
    {
        var keyboardInput = new KeyboardInputImpl(new StubKeyboardStateSource(new KeyboardState(Keys.Left, Keys.Down)));

        var direction = keyboardInput.GetDirection();

        Assert.That(direction.X, Is.EqualTo(-1));
        Assert.That(direction.Y, Is.EqualTo(1));
    }

    [Test]
    public void GetDirection_CancelsAxis_WhenOppositeKeysPressed()
    {
        var keyboardInput = new KeyboardInputImpl(new StubKeyboardStateSource(new KeyboardState(Keys.A, Keys.D, Keys.Up, Keys.Down)));

        var direction = keyboardInput.GetDirection();

        Assert.That(direction.X, Is.EqualTo(0));
        Assert.That(direction.Y, Is.EqualTo(0));
    }

    [Test]
    public void GetDirection_UsesPhysicalKeyboardDirection_WhenAvailable()
    {
        var keyboardInput = new KeyboardInputImpl(
            new StubKeyboardStateSource(new KeyboardState(Keys.Left)),
            new StubPhysicalKeyboardSource(new System.Numerics.Vector2(1, 0)));

        var direction = keyboardInput.GetDirection();

        Assert.That(direction.X, Is.EqualTo(1));
        Assert.That(direction.Y, Is.EqualTo(0));
    }

    private sealed class StubKeyboardStateSource(KeyboardState state) : IKeyboardStateSource
    {
        public KeyboardState GetState() => state;
    }

    private sealed class StubPhysicalKeyboardSource(System.Numerics.Vector2 direction) : IPhysicalKeyboardSource
    {
        public bool TryGetDirection(out System.Numerics.Vector2 result)
        {
            result = direction;
            return true;
        }
    }
}
