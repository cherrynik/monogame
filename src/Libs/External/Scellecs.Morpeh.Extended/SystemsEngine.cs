using Scellecs.Morpeh.Extended.Extensions;

namespace Scellecs.Morpeh.Extended;

public class SystemsEngine(World world)
{
    public readonly SystemsGroup Initializers = world.CreateSystemsGroup();
    public readonly SystemsGroup Systems = world.CreateSystemsGroup();
    public readonly SystemsGroup FixedSystems = world.CreateSystemsGroup();
    public readonly SystemsGroup LateSystems = world.CreateSystemsGroup();
    public readonly SystemsGroup CleanupSystems = world.CreateSystemsGroup();
    public readonly SystemsGroup RenderSystems = world.CreateSystemsGroup();

    public int GetLengthSlow() =>
        Initializers.GetInitializersInfo().length + Systems.GetSystemsInfo().length +
        FixedSystems.GetFixedSystemsInfo().length + LateSystems.GetLateSystemsInfo().length +
        CleanupSystems.GetCleanupSystemsInfo().length + RenderSystems.GetSystemsInfo().length;
}
