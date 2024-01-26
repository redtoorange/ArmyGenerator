using System;
using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using ArmyGenerator.UI;
using Godot;

namespace ArmyGenerator.View;

public partial class AvailableUnitList : VBoxContainer
{
    public event Action<UnitData> OnUnitAddNewUnitToSelected;

    [Export] private PackedScene unitListItemPrefab;
    [Export] private SelectedUnitList selectedUnitList;

    private Dictionary<string, AvilableUnitListItem> unitIdToListItemMap;
    private UnitInventoryController unitInventoryController;
    private UnitVisibilityController unitVisibilityController;

    public void Initialize(
        ArmyListData currentArmy, 
        UnitInventoryController unitInventoryController,
        UnitVisibilityController unitVisibilityController
        )
    {
        this.unitInventoryController = unitInventoryController;
        this.unitVisibilityController = unitVisibilityController;
        unitIdToListItemMap = new Dictionary<string, AvilableUnitListItem>();

        foreach (KeyValuePair<string, UnitData> unitData in currentArmy.unitIdToDataMap)
        {
            AvilableUnitListItem listItem = unitListItemPrefab.Instantiate<AvilableUnitListItem>();
            listItem.Initialize(unitData.Value, unitInventoryController, unitVisibilityController);
            listItem.OnAddPressed += HandleUnitAdded;

            unitIdToListItemMap.Add(unitData.Key, listItem);

            AddChild(listItem);
            listItem.Visible = !listItem.IsHidden();
        }
    }

    public override void _EnterTree()
    {
        ViewMenuController.onToggleShowHide += HandleToggleShowHide;
    }

    public override void _ExitTree()
    {
        ViewMenuController.onToggleShowHide -= HandleToggleShowHide;
    }

    private void HandleToggleShowHide(bool shouldShowOptions)
    {
        foreach (AvilableUnitListItem unit in unitIdToListItemMap.Values)
        {
            unit.Visible = shouldShowOptions ? true : !unit.IsHidden();
            unit.ToggleShowHideButtonDisplay(shouldShowOptions);
        }
    }

    private void HandleUnitAdded(AvilableUnitListItem listItem)
    {
        OnUnitAddNewUnitToSelected?.Invoke(listItem.GetUnitData());
    }

    public void ResetList(ArmyListData armyListData)
    {
        // Clear old units
        foreach (AvilableUnitListItem unit in unitIdToListItemMap.Values)
        {
            unit.QueueFree();
        }

        unitIdToListItemMap = new Dictionary<string, AvilableUnitListItem>();


        // Add new units
        foreach (KeyValuePair<string, UnitData> unitData in armyListData.unitIdToDataMap)
        {
            AvilableUnitListItem listItem = unitListItemPrefab.Instantiate<AvilableUnitListItem>();
            listItem.Initialize(unitData.Value, unitInventoryController, unitVisibilityController);
            listItem.OnAddPressed += HandleUnitAdded;

            unitIdToListItemMap.Add(unitData.Key, listItem);
            AddChild(listItem);
        }
    }
}