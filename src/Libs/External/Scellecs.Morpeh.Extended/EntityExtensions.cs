namespace Scellecs.Morpeh.Extended;

public static class EntityExtensions
{
    public static ref T AddComponent<T>(this Entity e, T component) where T : struct, IComponent
    {
        ref T added = ref e.AddComponent<T>();
        added = component;

        return ref added;
    }
}
