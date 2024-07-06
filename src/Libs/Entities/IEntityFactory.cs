using Scellecs.Morpeh;

namespace Entities;

public interface IEntityFactory
{
    Entity CreateEntity(World @in);
}
