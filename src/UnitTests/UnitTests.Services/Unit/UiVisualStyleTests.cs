using GameUi.Shared.UI;

namespace UnitTests.Services;

public class UiVisualStyleTests
{
    [Test]
    public void Default_UsesPositiveDimensions()
    {
        UiVisualStyle style = UiVisualStyle.Default;

        Assert.That(style.PauseMenuWidth, Is.GreaterThan(0));
        Assert.That(style.ButtonWidth, Is.GreaterThan(0));
        Assert.That(style.VerticalSpacing, Is.GreaterThan(0));
    }
}
