using System;
using Godot;

namespace ArmyGenerator.UI;

public partial class ViewMenuController : MenuButton
{
    public static Action<bool> onToggleShowHide;
    
    public override void _Ready()
    {
        GetPopup().IdPressed += HandleIdPressed;
    }

    private void HandleIdPressed(long id)
    {
        int index = (int)id;
        PopupMenu menu = GetPopup();
        if (index == 0)
        {
            if (menu.IsItemChecked(index))
            {
                menu.SetItemChecked(index, false);
                onToggleShowHide?.Invoke(false);
            }
            else
            {
                menu.SetItemChecked(index, true);
                onToggleShowHide?.Invoke(true);
            }
        }
    }
}