using System.Numerics;
using ImGuiNET;

namespace Systems.Debugging;

public static class DebugTheme
{
    // Text
    public static readonly Vector4 TextDim = Hex(0x565f89);

    // Entity types
    public static readonly Vector4 PlayerColor = Hex(0x7aa2f7);
    public static readonly Vector4 CameraEntityColor = Hex(0x73daca);
    public static readonly Vector4 ObjectColor = Hex(0x787c99);

    // Component types
    public static readonly Vector4 TransformColor = Hex(0x7aa2f7);
    public static readonly Vector4 CameraColor = Hex(0x73daca);
    public static readonly Vector4 CollisionColor = Hex(0xf7768e);
    public static readonly Vector4 AnimatorColor = Hex(0xe0af68);
    public static readonly Vector4 InventoryColor = Hex(0xbb9af7);
    public static readonly Vector4 TagColor = Hex(0x9ece6a);

    // Accent
    public static readonly Vector4 Accent = Hex(0x7aa2f7);
    public static readonly Vector4 Good = Hex(0x9ece6a);
    public static readonly Vector4 Warn = Hex(0xe0af68);

    private static bool _applied;

    public static void EnsureApplied()
    {
        if (_applied) return;
        _applied = true;
        Apply();
    }

    private static void Apply()
    {
        var s = ImGui.GetStyle();

        // Pixel art: sharp edges everywhere
        s.WindowRounding = 0;
        s.ChildRounding = 0;
        s.FrameRounding = 0;
        s.GrabRounding = 0;
        s.PopupRounding = 0;
        s.ScrollbarRounding = 0;
        s.TabRounding = 0;

        s.WindowBorderSize = 1;
        s.FrameBorderSize = 0;
        s.ChildBorderSize = 1;

        s.WindowPadding = new Vector2(10, 8);
        s.FramePadding = new Vector2(6, 3);
        s.ItemSpacing = new Vector2(8, 5);
        s.ItemInnerSpacing = new Vector2(6, 4);
        s.IndentSpacing = 14;
        s.ScrollbarSize = 10;

        // Backgrounds
        Set(s, ImGuiCol.WindowBg, 0x16161e);
        Set(s, ImGuiCol.ChildBg, 0x1a1b26);
        Set(s, ImGuiCol.PopupBg, 0x16161e);

        // Borders
        Set(s, ImGuiCol.Border, 0x292e42);
        Set(s, ImGuiCol.BorderShadow, 0x000000, 0f);

        // Text
        Set(s, ImGuiCol.Text, 0xa9b1d6);
        Set(s, ImGuiCol.TextDisabled, 0x565f89);

        // Headers
        Set(s, ImGuiCol.Header, 0x1e2336);
        Set(s, ImGuiCol.HeaderHovered, 0x282e44);
        Set(s, ImGuiCol.HeaderActive, 0x242a40);

        // Title bar
        Set(s, ImGuiCol.TitleBg, 0x0f0f17);
        Set(s, ImGuiCol.TitleBgActive, 0x16161e);
        Set(s, ImGuiCol.TitleBgCollapsed, 0x0f0f17);

        // Frames
        Set(s, ImGuiCol.FrameBg, 0x1a1b2e);
        Set(s, ImGuiCol.FrameBgHovered, 0x222338);
        Set(s, ImGuiCol.FrameBgActive, 0x2a2c44);

        // Scrollbar
        Set(s, ImGuiCol.ScrollbarBg, 0x12121c);
        Set(s, ImGuiCol.ScrollbarGrab, 0x333654);
        Set(s, ImGuiCol.ScrollbarGrabHovered, 0x3d4168);
        Set(s, ImGuiCol.ScrollbarGrabActive, 0x4a4f7c);

        // Buttons
        Set(s, ImGuiCol.Button, 0x1e2336);
        Set(s, ImGuiCol.ButtonHovered, 0x282e44);
        Set(s, ImGuiCol.ButtonActive, 0x323856);

        // Separators
        Set(s, ImGuiCol.Separator, 0x292e42);
        Set(s, ImGuiCol.SeparatorHovered, 0x292e42);
        Set(s, ImGuiCol.SeparatorActive, 0x292e42);

        // Resize grip
        Set(s, ImGuiCol.ResizeGrip, 0x292e42, 0.25f);
        Set(s, ImGuiCol.ResizeGripHovered, 0x3d4268, 0.67f);
        Set(s, ImGuiCol.ResizeGripActive, 0x4a5080, 0.95f);

        // Plot
        Set(s, ImGuiCol.PlotLines, 0x7aa2f7);
        Set(s, ImGuiCol.PlotHistogram, 0x73daca);
    }

    private static Vector4 Hex(uint rgb, float a = 1f) =>
        new(((rgb >> 16) & 0xFF) / 255f, ((rgb >> 8) & 0xFF) / 255f, (rgb & 0xFF) / 255f, a);

    private static void Set(ImGuiStylePtr s, ImGuiCol col, uint rgb, float a = 1f) =>
        s.Colors[(int)col] = Hex(rgb, a);
}
