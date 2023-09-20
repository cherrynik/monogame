using Components;
using Components.World;

public partial class GameEntity
{
    // TODO: The kind of code generator?
    public void AddMovementAnimation(MovementAnimationComponent component) =>
        AddMovementAnimation(newFacingDirection: component.FacingDirection,
            newPlayingAnimation: component.PlayingAnimation,
            newIdleAnimations: component.IdleAnimations,
            newWalkingAnimations: component.WalkingAnimations,
            newHasStopped: component.HasStopped);

    public void AddTransform(TransformComponent component) =>
        AddTransform(newPosition: component.Position, newVelocity: component.Velocity);
}
