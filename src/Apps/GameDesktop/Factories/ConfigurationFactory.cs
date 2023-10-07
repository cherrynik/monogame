using System;
using System.IO;
using GameDesktop.Resources.Internal;
using Microsoft.Extensions.Configuration;

namespace GameDesktop.Factories;

internal static class ConfigurationFactory
{
    private static string BasePath => Directory.GetParent(AppContext.BaseDirectory)!.FullName;

    private static string InEnvironment =>
        Environment.GetEnvironmentVariable(EnvironmentVariable.DotNetEnvironment) ?? "Production";

    public static IConfigurationRoot Create() =>
        new ConfigurationBuilder()
            .SetBasePath(BasePath)
            .AddJsonFile($"{AppVariable.SettingsName}.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{AppVariable.SettingsName}.{InEnvironment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
}
