namespace GameUi.Shared.UI;

public sealed class UiVisualStyle
{
    public int PauseMenuWidth { get; init; } = 240;
    public int ButtonWidth { get; init; } = 200;
    public int VerticalSpacing { get; init; } = 4;
    public int HeaderSpacing { get; init; } = 4;
    public int FooterSpacing { get; init; } = 4;
    public string PrimaryButtonStyleName { get; init; } = "blue";

    public static UiVisualStyle Default => new();
}
