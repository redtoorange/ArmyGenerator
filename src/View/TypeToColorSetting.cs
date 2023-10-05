using Godot;

namespace ArmyGenerator.ArmyList;

[GlobalClass]
public partial class TypeToColorSetting : Resource
{
    [Export] public string type;
    [Export] public Color color;
}