using GameUi.App.State;
using Myra.Graphics2D.UI;

namespace GameUi.App.Effects;

public sealed class GameUiEffectsHandler
{
    public bool Apply(UiStore uiStore, Desktop desktop)
    {
        if (uiStore.State.ActiveDialog == UiDialogKind.SettingsNotImplemented)
        {
            Dialog.CreateMessageBox("Settings", "Settings screen is not implemented yet.")
                .ShowModal(desktop);
            uiStore.Dispatch(UiAction.DismissDialog);
        }

        if (uiStore.State.ActiveDialog == UiDialogKind.RestartNotImplemented)
        {
            Dialog.CreateMessageBox("Restart", "Restart flow is not implemented yet.")
                .ShowModal(desktop);
            uiStore.Dispatch(UiAction.DismissDialog);
        }

        if (!uiStore.State.ShouldExit)
        {
            return false;
        }

        uiStore.Dispatch(UiAction.ExitHandled);
        return true;
    }
}
