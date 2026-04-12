namespace Scellecs.Morpeh.Extended;

public static class EntityExtensions
{
    public static ref T AddComponent<T>(this Entity e, T component) where T : struct, IComponent
    {
        #pragma warning disable CS0618 // MORPEH: kept for backward-compatible helper API
        ref T added = ref e.AddComponent<T>();
        #pragma warning restore CS0618
        added = component;

        return ref added;
    }
}
