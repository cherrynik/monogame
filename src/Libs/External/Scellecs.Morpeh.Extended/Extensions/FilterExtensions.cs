namespace Scellecs.Morpeh.Extended.Extensions;

public static class FilterExtensions
{
    public static IEnumerable<Entity> AsEnumerableSlow(this Filter filter)
    {
        var enumerator = filter.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }
}
