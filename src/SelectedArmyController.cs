using System;
using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using ArmyGenerator.ArmyList;
using ArmyGenerator.Model;
using Godot;

public partial class SelectedArmyController : Node
{
    public event Action<int> OnPointsChange;
    
    [Export] private AvailableUnitList availableUnitList;
    [Export] private SelectedUnitList selectedUnitList;
    [Export] private SpinBox pointsDisplay;
    [Export] private Button fillArmyButton;
    
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

        fillArmyButton.Pressed += HandleFillArmyPressed;
    }

    private void HandleFillArmyPressed()
    {
        throw new NotImplementedException();
    }

    private void HandleAddNewUnitAddNewUnitToToSelected(UnitData unit)
    {
        // Check to see if we can add or not
        List<SelectArmyUnitModel> unitModels = selectedArmy.GetUnitsOfType(unit);
        if (unitModels != null && unitModels.Count >= unit.maxCount)
        {
            return;
        }
        
        // Add unit to the army
        selectedArmy.AddToArmy(unit, out string unitReferenceId);
        
        // Update the inventory (available)
        
        // Add unit to the selected view
        selectedUnitList.AddUnit(unitReferenceId, unit);
        
        OnPointsChange?.Invoke(selectedArmy.GetPoints());
        pointsDisplay.Value = selectedArmy.GetPoints();
    }
    
    private void HandleUnitRemoved(string unitReferenceId, UnitData unt)
    {
        // Remove unit from the army
        selectedArmy.RemoveFromArmy(unitReferenceId, unt);
        
        // Update the inventory (available)

        OnPointsChange?.Invoke(selectedArmy.GetPoints());
        pointsDisplay.Value = selectedArmy.GetPoints();
    }
}