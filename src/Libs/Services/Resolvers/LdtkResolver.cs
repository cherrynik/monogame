using LDtk;

namespace Services.Resolvers;

public static class LdtkResolver
{
    public static LDtkFile ResolveFromApp(string appRelativePath) =>
        LDtkFile.FromFile(FileResolver.ResolveFromApp(appRelativePath));
}
