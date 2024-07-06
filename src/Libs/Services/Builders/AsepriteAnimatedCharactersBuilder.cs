using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using Services.Implementations.Math;

namespace Services.Builders;

public class AsepriteAnimatedCharactersBuilder
{
    public Dictionary<Sector, AnimatedSprite> Animations { get; private set; } = new();

    private SpriteSheet? _spriteSheet;

    private static readonly IReadOnlyList<Sector> Directions =
        new[] { Sector.Right, Sector.Down, Sector.Left, Sector.Up };

    public AsepriteAnimatedCharactersBuilder LoadSpriteSheet(GraphicsDevice graphicsDevice, string path)
    {
        AsepriteFile asepriteFile = AsepriteFile.Load(path);
        return new() { _spriteSheet = SpriteSheetProcessor.Process(graphicsDevice, asepriteFile) };
    }

    public AsepriteAnimatedCharactersBuilder CreateAnimations(string action)
    {
        if (_spriteSheet is null) throw new ArgumentNullException();

        Dictionary<Sector, AnimatedSprite> dictionary =
            Directions.ToDictionary(dir => dir, dir => CreateAnimation(_spriteSheet, action, dir));

        // Temp hack
        dictionary.Add(Sector.DownLeft, dictionary[Sector.Left]);
        dictionary.Add(Sector.DownRight, dictionary[Sector.Right]);
        dictionary.Add(Sector.UpLeft, dictionary[Sector.Left]);
        dictionary.Add(Sector.UpRight, dictionary[Sector.Right]);

        return new() { Animations = dictionary };
    }

    private static string BuildTag(string action, Sector dir) => $"{action}{dir.ToString()}";

    private static AnimatedSprite CreateAnimation(SpriteSheet spriteSheet, string action, Sector direction)
    {
        AnimatedSprite animatedSprite;

        if (direction is Sector.Left)
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
}
