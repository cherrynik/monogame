namespace Constants;

public static class DiContainerNames
{
    public const string PlayerAnimationsOffset = "PlayerAnimationsOffset";
    public const string Player = "PlayerEntity";
    public const string Camera = "CameraComponent";
    public const string Pebble = "Pebble";
    public const string Tree = "Tree";

    public static class Helpers
    {
        public const string AsepriteAnimatedCharacterBuilder = "AsepriteAnimatedCharacterBuilder";

        public const string TextureResolver = "TextureResolver";
    }

    public static class Features
    {
        public const string Root = "Root";
        public const string Initialize = "Initialize";
        public const string Update = "Update";
        public const string PreRender = "PreRender";
        public const string Render = "Render";
        public const string Debug = "Debug";
    }
}
