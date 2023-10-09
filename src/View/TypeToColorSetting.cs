using Godot;

namespace ArmyGenerator.View;

[GlobalClass]
public partial class TypeToColorSetting : Resource
{
    [Export] public string type;
    [Export] public Color color;
}