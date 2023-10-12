using Godot;

public partial class ScrollContainer : Godot.ScrollContainer
{
    [Export] private float width = 16;

    private bool hasBeenSet = false;
    
    public override void _Process(double delta)
    {
        if (!hasBeenSet)
        {
            VScrollBar vScrollBar = GetVScrollBar();
            vScrollBar.Scale = new Vector2(1.5f, 1f);
            vScrollBar.Position -= new Vector2(4, 0);

            hasBeenSet = true;
        }
    }

}