using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using Services;

namespace GameDesktop;

public class AnimatedCharactersFactory
{
    private readonly IReadOnlyList<RadDir> _directions =
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

    public Dictionary<RadDir, AnimatedSprite> CreateAnimations(SpriteSheet spriteSheet, string action)
    {
        Dictionary<RadDir, AnimatedSprite> dictionary =
            _directions.ToDictionary(dir => dir, dir => CreateAnimation(spriteSheet, action, dir));

        // Temp hack
        dictionary.Add(RadDir.DownLeft, dictionary[RadDir.Left]);
        dictionary.Add(RadDir.DownRight, dictionary[RadDir.Right]);
        dictionary.Add(RadDir.UpLeft, dictionary[RadDir.Left]);
        dictionary.Add(RadDir.UpRight, dictionary[RadDir.Right]);

        return dictionary;
    }
}
