using System.Reflection;
using System.Runtime.CompilerServices;
using Scellecs.Morpeh.Collections;

namespace Scellecs.Morpeh.Extended.Extensions;

public static class SystemsGroupsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastList<IInitializer> GetInitializersInfo(this SystemsGroup systemsGroup) =>
        (FastList<IInitializer>)GetInfo(systemsGroup, "initializers");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastList<ISystem> GetSystemsInfo(this SystemsGroup systemsGroup) =>
        (FastList<ISystem>)GetInfo(systemsGroup, "systems");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastList<ISystem> GetCleanupSystemsInfo(this SystemsGroup systemsGroup) =>
        (FastList<ISystem>)GetInfo(systemsGroup, "cleanupSystems");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastList<ISystem> GetFixedSystemsInfo(this SystemsGroup systemsGroup) =>
        (FastList<ISystem>)GetInfo(systemsGroup, "fixedSystems");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastList<ISystem> GetLateSystemsInfo(this SystemsGroup systemsGroup) =>
        (FastList<ISystem>)GetInfo(systemsGroup, "lateSystems");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object GetInfo(this SystemsGroup systemsGroup, string fieldName)
    {
        FieldInfo initializers =
            systemsGroup.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)!;
        return initializers.GetValue(systemsGroup)!;
    }
}
