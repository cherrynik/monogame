namespace Services.Input;

public interface IScancodeKeyboardStateSource
{
    bool TryGetState(out ScancodeKeyboardState state);
}
