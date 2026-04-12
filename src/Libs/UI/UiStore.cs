namespace GameUi.App.State;

public sealed class UiStore
{
    public UiState State { get; private set; } = UiState.Default;

    public UiState Dispatch(UiAction action)
    {
        State = UiReducer.Reduce(State, action);
        return State;
    }
}
