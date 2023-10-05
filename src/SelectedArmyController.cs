using ArmyGenerator.ArmyData;
using ArmyGenerator.ArmyList;
using ArmyGenerator.Model;
using Godot;

public partial class SelectedArmyController : Node
{
    [Export] private AvailableUnitList availableUnitList;
    [Export] private SelectedUnitList selectedUnitList;
    
    private SelectedArmyModel selectedArmy;
    private ArmyListData armyListData;
    
    public override void _Ready()
    {
        selectedArmy = new SelectedArmyModel();
        armyListData = DataFileLoader.S.loadedArmyListData[0];

        availableUnitList.Initialize(armyListData);
        availableUnitList.OnUnitAddNewUnitToSelected += HandleAddNewUnitAddNewUnitToToSelected;
        
        selectedUnitList.Initialize();
        selectedUnitList.OnUnitRemoved += HandleUnitRemoved;
    }

    private void HandleAddNewUnitAddNewUnitToToSelected(UnitData unit)
    {
        // Check to see if we can add or not
        
        // Add unit to the army
        
        // Update the inventory (available)
        
        // Add unit to the selected view
        selectedUnitList.AddUnit(0, unit);
    }
    
    private void HandleUnitRemoved(int unitReferenceId, UnitData unt)
    {
        // Remove unit from the army
        
        // Update the inventory (available)
    }
}