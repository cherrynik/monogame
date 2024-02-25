using System.Reflection;
using System.Runtime.CompilerServices;

namespace Scellecs.Morpeh.Extended;

public static class EntityExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T AddComponent<T>(this Entity e, T component) where T : struct, IComponent
    {
        ref T added = ref e.AddComponent<T>();
        added = component;

        return ref added;
    }

    // Just in case, if u wanna get extension methods,
    // you gotta get it directly from the extension class: typeof(ExtensionClass).GetMethods();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has(this Entity e, Type type) => GetWorld(e).GetReflectionStash(type).Has(e);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static World GetWorld(this Entity e)
    {
        FieldInfo? field = e.GetType().GetField("world", BindingFlags.Instance | BindingFlags.NonPublic);
        return (World)field?.GetValue(e)!;
    }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static int GetLength<T>(this Stash<T> stash) where T : struct, IComponent
    // {
    //     stash.world.ThreadSafetyCheck();
    //     return stash.components.length;
    // }
}
