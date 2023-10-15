using ArmyGenerator.GlobalStore;
using Godot;
using Godot.Collections;

public partial class FillOptionWindow : Window
{
    [Export] private Button openOptionsButton;
    [Export] private Array<CheckBox> fillOptions; 

    public override void _Ready()
    {
        CloseRequested += HandleCloseWindow;
        openOptionsButton.Pressed += () => Visible = !Visible;

        foreach (CheckBox checkBox in fillOptions)
        {
            checkBox.Toggled += (bool toggledOn) => HandleButtonToggled(checkBox, toggledOn);
        }
    }

    private void HandleButtonToggled(CheckBox checkBox, bool toggledOn)
    {
        switch (checkBox.Name)
        {
            case "NO_CHARACTERS":
                GlobalDataStore.S.GetFillOptions().NoCharacters = toggledOn;
                break;
            case "NO_EPIC_CHARACTERS":
                GlobalDataStore.S.GetFillOptions().NoEpicCharacters = toggledOn;
                break;
            case "NO_TRANSPORTS":
                GlobalDataStore.S.GetFillOptions().NoTransports = toggledOn;
                break;
            case "NO_BATTLELINE":
                GlobalDataStore.S.GetFillOptions().NoBattleLine = toggledOn;
                break;
            case "NO_FORTIFICATIONS":
                GlobalDataStore.S.GetFillOptions().NoFortifications = toggledOn;
                break;
        }
    }

    private void HandleCloseWindow()
    {
        Visible = false;
    }
}