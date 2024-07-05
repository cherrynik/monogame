using Scellecs.Morpeh;

namespace Entities.Factories;

public interface IEntityFactory
{
    Entity CreateEntity(World @in);
}
