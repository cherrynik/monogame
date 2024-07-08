using Components.Data;
using Scellecs.Morpeh;

namespace Implementations;

public class EntitiesSorting
{
    public static IOrderedEnumerable<Entity> GetEntitiesSortedByY(IEnumerable<Entity> entities) =>
        entities.OrderBy(x => x.GetComponent<TransformComponent>().Position.Y);
}
