using LDtk;

namespace Services.Helpers;

public class LdtkResolver
{
    public static LDtkFile ResolveFromApp(string appRelativePath) =>
        LDtkFile.FromFile(FileResolver.ResolveFromApp(appRelativePath));
}
