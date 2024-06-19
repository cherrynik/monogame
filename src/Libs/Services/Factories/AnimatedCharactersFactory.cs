using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using Services.Math;

namespace Services.Factories;

public static class AnimatedCharactersFactory
{
    private static readonly IReadOnlyList<Sector> Directions =
        new[] { Sector.Right, Sector.Down, Sector.Left, Sector.Up };

    public static SpriteSheet LoadSpriteSheet(GraphicsDevice graphicsDevice, string path)
    {
        AsepriteFile asepriteFile = AsepriteFile.Load(path);
        return SpriteSheetProcessor.Process(graphicsDevice, asepriteFile);
    }

    private static string BuildTag(string action, Sector dir) => $"{action}{dir.ToString()}";

    private static AnimatedSprite CreateAnimation(SpriteSheet spriteSheet, string action, Sector direction)
    {
        AnimatedSprite animatedSprite;

        if (direction == Sector.Left)
        {
            string rightAnimationTag = BuildTag(action, Sector.Right);

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

    public static Dictionary<Sector, AnimatedSprite> CreateAnimations(SpriteSheet spriteSheet, string action)
    {
        Dictionary<Sector, AnimatedSprite> dictionary =
            Directions.ToDictionary(dir => dir, dir => CreateAnimation(spriteSheet, action, dir));

        // Temp hack
        dictionary.Add(Sector.DownLeft, dictionary[Sector.Left]);
        dictionary.Add(Sector.DownRight, dictionary[Sector.Right]);
        dictionary.Add(Sector.UpLeft, dictionary[Sector.Left]);
        dictionary.Add(Sector.UpRight, dictionary[Sector.Right]);

        return dictionary;
    }
}
