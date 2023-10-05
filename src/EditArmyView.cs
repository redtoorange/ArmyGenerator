using Godot;

public partial class EditArmyView : Control
{
    [Export] private SplitContainer mainViewContainer;

    public override void _Ready()
    {
        Resized += HandleWindowResized;
    }

    private void HandleWindowResized()
    {
        GD.Print("Resized");
        mainViewContainer.SplitOffset = Mathf.RoundToInt(Size.X / 2.0f);
    }

    public override void _Process(double delta)
    {
    }
}