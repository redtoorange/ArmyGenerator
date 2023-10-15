using Godot;

namespace ArmyGenerator.UI;

[GlobalClass]
public partial class FillOptionResource : Resource
{
    [Export] public CheckBox checkBox;
    [Export] public string id;
}