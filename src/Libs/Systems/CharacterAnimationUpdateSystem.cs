using Components.Data;
using Components.Render;
using Implementations;
using Implementations.Visuals;
using Scellecs.Morpeh;

namespace Systems;

public class CharacterAnimationUpdateSystem(World world) : ISystem
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<CharacterAnimatorComponent>().With<MovementAnimationsComponent>().Build();

        foreach (Entity e in filter) CharacterAnimation.Update(e, deltaTime);
    }

    public void Dispose()
    {
    }
}
