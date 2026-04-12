using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;

namespace GameDesktop.Ui.Myra;

internal sealed class MyraUiEnvironmentInitializer
{
    private const string FontsDirectoryName = "Fonts";
    private const string FontFileName = "Cairopixel.ttf";
    private const int DefaultFontSize = 20;

    public void Initialize(Microsoft.Xna.Framework.Game game)
    {
        MyraEnvironment.Game = game;
        MyraEnvironment.MouseInfoGetter = () => BuildMouseInfo(game);

        SpriteFontBase pixelFont = BuildPixelFont(game.Content.RootDirectory);
        Stylesheet.Current.LabelStyle.Font = pixelFont;
        Stylesheet.Current.ButtonStyle.LabelStyle.Font = pixelFont;
    }

    private static MouseInfo BuildMouseInfo(Microsoft.Xna.Framework.Game game)
    {
        MouseState state = Mouse.GetState();

        return new MouseInfo
        {
            Position = new Point(state.X, state.Y),
            IsLeftButtonDown = game.IsActive && state.LeftButton == ButtonState.Pressed,
            IsMiddleButtonDown = game.IsActive && state.MiddleButton == ButtonState.Pressed,
            IsRightButtonDown = game.IsActive && state.RightButton == ButtonState.Pressed,
            Wheel = state.ScrollWheelValue
        };
    }

    private static SpriteFontBase BuildPixelFont(string contentRootDirectory)
    {
        string fontPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            contentRootDirectory,
            FontsDirectoryName,
            FontFileName);

        byte[] fontData = File.ReadAllBytes(fontPath);
        var fontSystem = new FontSystem();
        fontSystem.AddFont(fontData);

        return fontSystem.GetFont(DefaultFontSize);
    }
}
