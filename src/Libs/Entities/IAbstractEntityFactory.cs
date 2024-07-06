using LDtk;
using Scellecs.Morpeh;

namespace Entities;

public interface IAbstractEntityFactory
{
    Entity? CreateEntity(string tag, World @in);
}
