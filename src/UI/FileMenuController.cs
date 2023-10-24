using ArmyGenerator;
using Godot;

public partial class FileMenuController : MenuButton
{
    private static readonly int CHANGE_LIST_ID = 0;
    private static readonly int EXPORT_TO_CLIP_BOARD_ID = 2;
    private static readonly int EXPORT_TO_TEXT_ID = 3;

    [Export] private CopiedNotification copiedNotificationToast;
    [Export] private SelectedArmyController selectedArmyController;
    [Export] private ChangeArmyWindow changeArmyWindow;

    public override void _Ready()
    {
        GetPopup().IdPressed += HandleIdPressed;
    }

    private void HandleIdPressed(long id)
    {
        if (id == CHANGE_LIST_ID)
        {
            changeArmyWindow.Show();
        }
        else if (id == EXPORT_TO_CLIP_BOARD_ID)
        {
            copiedNotificationToast.Show();
            DisplayServer.ClipboardSet(selectedArmyController.GetArmyAsString());
        }
        else if (id == EXPORT_TO_TEXT_ID)
        {
        }
    }
}