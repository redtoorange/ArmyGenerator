using Godot;

public partial class MainMenu : Control
{
    [ExportSubgroup("Scenes")]
    [Export] private PackedScene newArmyScene;
    [Export] private PackedScene loadArmyScene;
    [Export] private PackedScene editDataScene;

    [ExportSubgroup("Buttons")]
    [Export] private Button newArmyButton;
    [Export] private Button loadArmyButton;
    [Export] private Button editDataButton;

    public override void _Ready()
    {
        newArmyButton.Pressed += OnNewArmyClicked;
        loadArmyButton.Pressed += OnLoadArmyClicked;
        editDataButton.Pressed += OnEditDataClicked;
    }

    private void OnNewArmyClicked()
    {
        GetTree().ChangeSceneToPacked(newArmyScene);
    }

    private void OnLoadArmyClicked()
    {
        GetTree().ChangeSceneToPacked(loadArmyScene);
    }

    private void OnEditDataClicked()
    {
        GetTree().ChangeSceneToPacked(editDataScene);
    }
}