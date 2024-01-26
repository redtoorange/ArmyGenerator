using ArmyGenerator.ArmyData;
using Godot;
using Godot.Collections;

namespace ArmyGenerator;

public partial class UnitVisibilityController : Node
{
    private Dictionary<string, bool> unitReferenceIdToVisibility;

    public override void _Ready()
    {
        if (unitReferenceIdToVisibility == null)
        {
            unitReferenceIdToVisibility = new Dictionary<string, bool>();
        }
    }

    public override void _EnterTree()
    {
        GD.Print("Loading Visibility Preferences");
        FileAccess loadFile = FileAccess.Open("res://VisibilityPreferences.save", FileAccess.ModeFlags.Read);
        if (loadFile != null)
        {
            string fileData = loadFile.GetAsText();
            unitReferenceIdToVisibility = GD.StrToVar(fileData).AsGodotDictionary<string, bool>();
            GD.Print("Successfully loaded Visibility Preferences");
        }
    }

    public override void _ExitTree()
    {
        GD.Print("Saving Visibility Preferences");
        FileAccess saveFile = FileAccess.Open("res://VisibilityPreferences.save", FileAccess.ModeFlags.Write);
        string unitData = GD.VarToStr(unitReferenceIdToVisibility);
        saveFile.StoreString(unitData);
        GD.Print("Successfully saved Visibility Preferences");
    }

    public bool HasVisibilityValue(UnitData unitData)
    {
        return unitReferenceIdToVisibility.ContainsKey(unitData.unitId);
    }

    public void SetVisibility(UnitData unitData, bool visible)
    {
        unitReferenceIdToVisibility[unitData.unitId] = visible;
    }

    public bool GetVisibilityPreference(UnitData unit)
    {
        if (unitReferenceIdToVisibility.TryGetValue(unit.unitId, out bool value))
        {
            string parsed = (value) ? "Visible" : "Hidden";
            GD.Print($"Found Existing value for {unit.name} using \"{parsed}\"");
            return value;
        }

        GD.Print($"No value for {unit.name}, defaulting to \"Visible\"");
        SetVisibility(unit, true);

        return true;
    }
}