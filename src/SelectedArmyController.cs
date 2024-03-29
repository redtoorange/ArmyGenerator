using System;
using System.Collections.Generic;
using System.Text;
using ArmyGenerator.ArmyData;
using ArmyGenerator.GlobalStore;
using ArmyGenerator.ImperialAgent;
using ArmyGenerator.Model;
using ArmyGenerator.View;
using Godot;

namespace ArmyGenerator;

public partial class SelectedArmyController : Node
{
    public event Action<int> OnPointsChange;

    [Export] private AvailableUnitList availableUnitList;
    [Export] private SelectedUnitList selectedUnitList;
    [Export] private UnitInventoryController unitInventoryController;
    [Export] private UnitVisibilityController unitVisibilityController;

    [Export] private SpinBox desiredArmyPoints;
    [Export] private SpinBox pointsDisplay;

    [Export] private Button fillArmyButton;
    [Export] private Button clearArmyButton;

    private SelectedArmyModel selectedArmy;
    private ArmyListData armyListData;
    private ImperialAgentManager imperialAgentManager;


    public override void _Ready()
    {
        selectedArmy = new SelectedArmyModel();
        armyListData = DataFileLoader.S.GetSelectedArmyList();

        availableUnitList.Initialize(armyListData, unitInventoryController, unitVisibilityController);
        availableUnitList.OnUnitAddNewUnitToSelected += HandleAddNewUnitAddNewUnitToToSelected;

        selectedUnitList.Initialize();
        selectedUnitList.OnUnitRemoved += HandleUnitRemoved;
        selectedUnitList.OnUnitListModelsChanged += HandleUnitListChange;

        fillArmyButton.Pressed += HandleFillArmyPressed;
        clearArmyButton.Pressed += HandleClearArmyPressed;
    }

    private void HandleClearArmyPressed()
    {
        selectedUnitList.ClearList();
    }

    private void HandleUnitListChange(UnitData unitData, string unitReferenceId, int oldIndex, int newIndex)
    {
        selectedArmy.UpdateItem(unitData, unitReferenceId, oldIndex, newIndex);

        OnPointsChange?.Invoke(selectedArmy.GetPoints());
        pointsDisplay.Value = selectedArmy.GetPoints();
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
        ArmyFillOptions fillOptions = GlobalDataStore.S.GetFillOptions();
        foreach (UnitData value in armyListData.unitIdToDataMap.Values)
        {
            if (!fillOptions.IsBanned(value) && CanAddUnit(value))
            {
                units.Add(value);
            }
        }

        // Allow Agents of the imperium 
        imperialAgentManager = null;
        if (fillOptions.IncludeImperialAgents)
        {
            imperialAgentManager = new ImperialAgentManager(
                Mathf.CeilToInt((fillOptions.ImperialAgentCharacterCount * desiredPoints) / 1000.0f),
                Mathf.CeilToInt((fillOptions.ImperialAgentRetinueCount * desiredPoints) / 1000.0f),
                selectedArmy.GetAllUnits()
            );

            ArmyListData agentsData = DataFileLoader.S.GetArmyByName(ArmyNames.IMPERIAL_AGENTS);
            foreach (UnitData value in agentsData.unitIdToDataMap.Values)
            {
                if (!fillOptions.IsBanned(value) && CanAddUnit(value))
                {
                    units.Add(value);
                }
            }
        }

        while (units.Count > 0)
        {
            // Randomly Add a unit
            int index = GD.RandRange(0, units.Count - 1);

            UnitData unitToAdd = units[index];
            
            HandleAddNewUnitAddNewUnitToToSelected(unitToAdd);

            // Scrub the list of units that don't fit
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (!CanAddUnit(units[i]))
                {
                    units.RemoveAt(i);
                }
            }
        }

        selectedUnitList.SortSelectUnits();
    }

    public bool CanAddUnit(UnitData unit)
    {
        if (imperialAgentManager != null && !imperialAgentManager.IsAllowed(unit))
        {
            return false;
        }

        // Will adding this unit go over the desired points?
        int desiredPoints = (int)desiredArmyPoints.Value;
        if (unit.FindMinimumKey().points + selectedArmy.GetPoints() > desiredPoints)
        {
            return false;
        }

        // Are we allow to add more of this unit type?
        if (selectedArmy.GetUnitsOfTypeCount(unit) >= unitInventoryController.GetMaxCount(unit))
        {
            return false;
        }

        // Hidden units are not selectable
        if (!unitVisibilityController.GetVisibilityPreference(unit))
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
        if (imperialAgentManager != null)
        {
            imperialAgentManager.UnitAdded(unit);
        }

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

    public string GetArmyAsString()
    {
        int totalPoints = selectedArmy.GetPoints();
        int maxPoints = (int)desiredArmyPoints.Value;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Points: {totalPoints} / {maxPoints}");
        stringBuilder.Append(selectedArmy.GetArmyAsString());
        return stringBuilder.ToString();
    }

    public void ChangeArmy(int armyOptionsSelected)
    {
        armyListData = DataFileLoader.S.GetArmyList(armyOptionsSelected);
        availableUnitList.ResetList(armyListData);
    }
}