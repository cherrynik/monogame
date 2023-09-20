using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.Xna.Framework;

namespace Components;

[Unique]
public class CameraComponent : IComponent
{
    public Rectangle Size;
}
