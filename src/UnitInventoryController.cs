using ArmyGenerator.ArmyData;
using Godot;
using Godot.Collections;

namespace ArmyGenerator;

public partial class UnitInventoryController : Node
{
    private Dictionary<string, int> unitReferenceIdToInventory;

    public override void _Ready()
    {
        if (unitReferenceIdToInventory == null)
        {
            unitReferenceIdToInventory = new Dictionary<string, int>();
        }
    }

    public override void _EnterTree()
    {
        GD.Print("Loading Inventory Preferences");
        FileAccess loadFile = FileAccess.Open("res://InventoryPreferences.save", FileAccess.ModeFlags.Read);
        if (loadFile != null)
        {
            string fileData = loadFile.GetAsText();
            unitReferenceIdToInventory = GD.StrToVar(fileData).AsGodotDictionary<string, int>();
            GD.Print("Successfully loaded Inventory Preferences");
        }
    }

    public override void _ExitTree()
    {
        GD.Print("Saving Inventory Preferences");
        FileAccess saveFile = FileAccess.Open("res://InventoryPreferences.save", FileAccess.ModeFlags.Write);
        string unitData = GD.VarToStr(unitReferenceIdToInventory);
        saveFile.StoreString(unitData);
        GD.Print("Successfully saved Inventory Preferences");
    }

    public bool HasInventoryValue(UnitData unitData)
    {
        return unitReferenceIdToInventory.ContainsKey(unitData.unitId);
    }

    public void SetInventory(UnitData unitData, int inventory)
    {
        unitReferenceIdToInventory[unitData.unitId] = inventory;
    }

    public int GetMaxCount(UnitData unit)
    {
        if (unitReferenceIdToInventory.TryGetValue(unit.unitId, out int value))
        {
            GD.Print($"Found Existing value for {unit.name} using {value} ");
            return value;
        }

        GD.Print($"No value for {unit.name}, using {unit.maxCount}");
        SetInventory(unit, unit.maxCount);

        return unit.maxCount;
    }
}