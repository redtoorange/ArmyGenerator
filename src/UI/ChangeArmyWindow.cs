using ArmyGenerator;
using ArmyGenerator.ArmyData;
using Godot;

public partial class ChangeArmyWindow : Window
{
    [Export] private SelectedArmyController selectedArmyController;
    [Export] private OptionButton armyOptions;
    [Export] private Button submitButton;

    public override void _Ready()
    {
        for (int i = 0; i < DataFileLoader.S.loadedArmyListData.Count; i++)
        {
            ArmyListData data = DataFileLoader.S.loadedArmyListData[i];
            armyOptions.AddItem(data.armyName, i);
        }
        armyOptions.Selected = 0;

        submitButton.Pressed += HandleChangePressed;

        CloseRequested += HandleCloseRequested;
    }

    private void HandleCloseRequested()
    {
        Hide();
    }

    private void HandleChangePressed()
    {
        selectedArmyController.ChangeArmy(armyOptions.Selected);
        Hide();
    }
}