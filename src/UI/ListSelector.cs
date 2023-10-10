using System;
using ArmyGenerator.ArmyData;
using Godot;

public partial class ListSelector : Control
{
    [Export] private OptionButton armyOptions;
    [Export] private Button startButton;
    [Export(PropertyHint.File, "*.tscn")] private string pathToEditScene;

    public override void _Ready()
    {
        for (int i = 0; i < DataFileLoader.S.loadedArmyListData.Count; i++)
        {
            ArmyListData data = DataFileLoader.S.loadedArmyListData[i];
            armyOptions.AddItem(data.armyName, i);
        }
        armyOptions.Selected = 0;

        startButton.Pressed += HandleStartPressed;
    }

    private void HandleStartPressed()
    {
        DataFileLoader.S.SetSelectedArmy(armyOptions.Selected);
        GetTree().ChangeSceneToFile(pathToEditScene);
    }
}