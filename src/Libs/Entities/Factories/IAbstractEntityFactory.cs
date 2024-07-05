using LDtk;
using Scellecs.Morpeh;

namespace Entities.Factories;

public interface IAbstractEntityFactory
{
    Entity? CreateEntity(string tag, World @in);
}
