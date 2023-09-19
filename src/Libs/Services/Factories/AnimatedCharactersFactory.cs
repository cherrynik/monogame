using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using Services.Math;

namespace Services.Factories;

public static class AnimatedCharactersFactory
{
    private static readonly IReadOnlyList<RadDir> Directions =
        new[] { RadDir.Right, RadDir.Down, RadDir.Left, RadDir.Up };

    public static SpriteSheet LoadSpriteSheet(GraphicsDevice graphicsDevice, string path)
    {
        AsepriteFile asepriteFile = AsepriteFile.Load(path);
        return SpriteSheetProcessor.Process(graphicsDevice, asepriteFile);
    }

    private static string BuildTag(string action, RadDir dir) => $"{action}{dir.ToString()}";

    private static AnimatedSprite CreateAnimation(SpriteSheet spriteSheet, string action, RadDir direction)
    {
        AnimatedSprite animatedSprite;

        if (direction == RadDir.Left)
        {
            string rightAnimationTag = BuildTag(action, RadDir.Right);

            animatedSprite = spriteSheet.CreateAnimatedSprite(rightAnimationTag);
            animatedSprite.FlipHorizontally = true;
        }
        else
        {
            string animationTag = BuildTag(action, direction);
            animatedSprite = spriteSheet.CreateAnimatedSprite(animationTag);
        }

        animatedSprite.Play();

        return animatedSprite;
    }

    public static Dictionary<RadDir, AnimatedSprite> CreateAnimations(SpriteSheet spriteSheet, string action)
    {
        Dictionary<RadDir, AnimatedSprite> dictionary =
            Directions.ToDictionary(dir => dir, dir => CreateAnimation(spriteSheet, action, dir));

        // Temp hack
        dictionary.Add(RadDir.DownLeft, dictionary[RadDir.Left]);
        dictionary.Add(RadDir.DownRight, dictionary[RadDir.Right]);
        dictionary.Add(RadDir.UpLeft, dictionary[RadDir.Left]);
        dictionary.Add(RadDir.UpRight, dictionary[RadDir.Right]);

        return dictionary;
    }
}
