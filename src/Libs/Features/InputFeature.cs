using Systems;

namespace Features;

public sealed class InputFeature : Entitas.Extended.Feature
{
    public InputFeature(InputSystem inputSystem)
    {
        Add(inputSystem);
    }
}
