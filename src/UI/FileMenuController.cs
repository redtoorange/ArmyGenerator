using Godot;

public partial class FileMenuController : MenuButton
{
    private static readonly int CHANGE_LIST_ID = 0;
    private static readonly int EXPORT_TO_CLIP_BOARD_ID = 2;
    private static readonly int EXPORT_TO_TEXT_ID = 3;

    public override void _Ready()
    {
        GetPopup().IdPressed += HandleIdPressed;
    }

    private void HandleIdPressed(long id)
    {
        if (id == CHANGE_LIST_ID)
        {
            GD.Print("Changing List!");
        }
        else if (id == EXPORT_TO_CLIP_BOARD_ID)
        {
        }
        else if (id == EXPORT_TO_TEXT_ID)
        {
        }
    }
}