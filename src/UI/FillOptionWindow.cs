using ArmyGenerator.GlobalStore;
using Godot;
using Godot.Collections;

public partial class FillOptionWindow : Window
{
    [Export] private Button openOptionsButton;
    [Export] private Array<CheckBox> fillOptions;

    [ExportGroup("Imperial Agents")]
    [Export] private CheckBox imperialAgentsCheckBox;
    
    [Export] private Control imperialAgentsCharacterHolder;
    [Export] private Control imperialAgentsRetinueHolder;
    
    [Export] private SpinBox imperialAgentsCharacterCount;
    [Export] private SpinBox imperialAgentsRetinueCount;

    public override void _Ready()
    {
        CloseRequested += HandleCloseWindow;
        openOptionsButton.Pressed += () => Visible = !Visible;

        foreach (CheckBox checkBox in fillOptions)
        {
            checkBox.Toggled += (bool toggledOn) => HandleButtonToggled(checkBox, toggledOn);
        }
        
        imperialAgentsCheckBox.Toggled  += HandleImperialAgentsCheckBox;
        imperialAgentsCharacterCount.ValueChanged += HandleImperialCharacterAgentCountChange;
        imperialAgentsRetinueCount.ValueChanged += HandleImperialRetinueAgentCountChange;
        
        GlobalDataStore.S.GetFillOptions().ImperialAgentCharacterCount = (int) imperialAgentsCharacterCount.Value;
        GlobalDataStore.S.GetFillOptions().ImperialAgentRetinueCount = (int) imperialAgentsRetinueCount.Value;
    }

    private void HandleImperialCharacterAgentCountChange(double value)
    {
        GlobalDataStore.S.GetFillOptions().ImperialAgentCharacterCount = (int) value;
    }
    
    private void HandleImperialRetinueAgentCountChange(double value)
    {
        GlobalDataStore.S.GetFillOptions().ImperialAgentRetinueCount = (int) value;
    }

    private void HandleImperialAgentsCheckBox(bool toggledOn)
    {
        if (toggledOn)
        {
            GlobalDataStore.S.GetFillOptions().IncludeImperialAgents = true;
            imperialAgentsCharacterHolder.Show();
            imperialAgentsRetinueHolder.Show();
        }
        else
        {
            GlobalDataStore.S.GetFillOptions().IncludeImperialAgents = false;
            imperialAgentsCharacterHolder.Hide();
            imperialAgentsRetinueHolder.Hide();
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