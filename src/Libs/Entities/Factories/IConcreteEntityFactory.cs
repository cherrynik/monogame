using Scellecs.Morpeh;

namespace Entities.Factories;

public interface IConcreteEntityFactory : IAbstractEntityFactory
{
    Entity CreateEntity(World @in);
}
