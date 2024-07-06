using Constants;

namespace Services.Resolvers;

public static class FileResolver
{
    public static string ResolveFromApp(string appRelativePath) => Path.Join(
        Environment.GetEnvironmentVariable(EnvironmentNames.AppBaseDirectory),
        appRelativePath);
}
