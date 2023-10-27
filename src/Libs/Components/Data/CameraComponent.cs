using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.Xna.Framework;

namespace Entitas.Components.Data;

[Unique]
public class CameraComponent : IComponent
{
    public Rectangle Size;
}

// Camera is a system but a component,
// So, the system would have a target as a dependency,
// And exist only in the current world,
// As well, the system has its own behaviour,
// Thus, there's no reason to migrate the component
