using Services;
using Systems;

namespace Entities;

public sealed class GameSystems : Feature
{
    public GameSystems(Contexts contexts)
    {
        Add(new CreateEntitySystem(contexts));
        Add(new InputSystem(contexts, new KeyboardScanner()));
        // Add(new Sys)
    }
}
