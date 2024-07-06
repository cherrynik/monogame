using System.Numerics;
using Components.Data;

namespace Components.Render;

public interface IPivotted
{
    public Vector2 GetOffPivot(TransformComponent parentTransform);
}
