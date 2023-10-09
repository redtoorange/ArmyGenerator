using System;
using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using ArmyGenerator.Model;
using ArmyGenerator.View;
using Godot;

namespace ArmyGenerator;

public partial class SelectedArmyController : Node
{
    public event Action<int> OnPointsChange;

    [Export] private View.AvailableUnitList availableUnitList;
    [Export] private SelectedUnitList selectedUnitList;

    [Export] private SpinBox desiredArmyPoints;
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
        // Calculate how many points we need to generate
        int desiredPoints = (int)desiredArmyPoints.Value;
        int currentPoints = selectedArmy.GetPoints();
        int deltaPoints = desiredPoints - currentPoints;

        if (deltaPoints <= 0)
        {
            GD.Print("Points already filled");
            return;
        }

        // Create a list of units we can use to fill the remaining points
        List<UnitData> units = new List<UnitData>();
        foreach (KeyValuePair<string, UnitData> pair in armyListData.unitIdToDataMap)
        {
            if (CanAddUnit(pair.Value))
            {
                units.Add(pair.Value);
            }
        }

        while (units.Count > 0)
        {
            // Randomly Add a unit
            int index = GD.RandRange(0, units.Count - 1);
            HandleAddNewUnitAddNewUnitToToSelected(units[index]);

            // Scrub the list of units that don't fit
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (!CanAddUnit(units[i]))
                {
                    units.RemoveAt(i);
                }
            }
        }
    }

    public bool CanAddUnit(UnitData unit)
    {
        // Will adding this unit go over the desired points?
        int desiredPoints = (int)desiredArmyPoints.Value;
        if (unit.FindMinimumKey().points + selectedArmy.GetPoints() > desiredPoints)
        {
            return false;
        }

        // Are we allow to add more of this unit type?
        if (selectedArmy.GetUnitsOfTypeCount(unit) >= unit.maxCount)
        {
            return false;
        }

        return true;
    }

    private void HandleAddNewUnitAddNewUnitToToSelected(UnitData unit)
    {
        // Check to see if we can add or not
        if (!CanAddUnit(unit))
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