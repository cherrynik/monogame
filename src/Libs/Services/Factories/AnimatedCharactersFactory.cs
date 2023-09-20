using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using Services.Math;

namespace Services.Factories;

public static class AnimatedCharactersFactory
{
    private static readonly IReadOnlyList<Direction> Directions =
        new[] { Direction.Right, Direction.Down, Direction.Left, Direction.Up };

    public static SpriteSheet LoadSpriteSheet(GraphicsDevice graphicsDevice, string path)
    {
        AsepriteFile asepriteFile = AsepriteFile.Load(path);
        return SpriteSheetProcessor.Process(graphicsDevice, asepriteFile);
    }

    private static string BuildTag(string action, Direction dir) => $"{action}{dir.ToString()}";

    private static AnimatedSprite CreateAnimation(SpriteSheet spriteSheet, string action, Direction direction)
    {
        AnimatedSprite animatedSprite;

        if (direction == Direction.Left)
        {
            string rightAnimationTag = BuildTag(action, Direction.Right);

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

    public static Dictionary<Direction, AnimatedSprite> CreateAnimations(SpriteSheet spriteSheet, string action)
    {
        Dictionary<Direction, AnimatedSprite> dictionary =
            Directions.ToDictionary(dir => dir, dir => CreateAnimation(spriteSheet, action, dir));

        // Temp hack
        dictionary.Add(Direction.DownLeft, dictionary[Direction.Left]);
        dictionary.Add(Direction.DownRight, dictionary[Direction.Right]);
        dictionary.Add(Direction.UpLeft, dictionary[Direction.Left]);
        dictionary.Add(Direction.UpRight, dictionary[Direction.Right]);

        return dictionary;
    }
}
