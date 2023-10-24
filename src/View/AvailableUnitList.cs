using System;
using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using Godot;

namespace ArmyGenerator.View;

public partial class AvailableUnitList : VBoxContainer
{
    public event Action<UnitData> OnUnitAddNewUnitToSelected;

    [Export] private PackedScene unitListItemPrefab;
    [Export] private SelectedUnitList selectedUnitList;

    private Dictionary<string, AvilableUnitListItem> unitIdToListItemMap;
    private UnitInventoryController unitInventoryController;

    public void Initialize(ArmyListData currentArmy, UnitInventoryController unitInventoryController)
    {
        this.unitInventoryController = unitInventoryController;
        unitIdToListItemMap = new Dictionary<string, AvilableUnitListItem>();

        foreach (KeyValuePair<string, UnitData> unitData in currentArmy.unitIdToDataMap)
        {
            AvilableUnitListItem listItem = unitListItemPrefab.Instantiate<AvilableUnitListItem>();
            listItem.Initialize(unitData.Value, unitInventoryController);
            listItem.OnAddPressed += HandleUnitAdded;

            unitIdToListItemMap.Add(unitData.Key, listItem);

            AddChild(listItem);
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
            listItem.Initialize(unitData.Value, unitInventoryController);
            listItem.OnAddPressed += HandleUnitAdded;

            unitIdToListItemMap.Add(unitData.Key, listItem);
            AddChild(listItem);
        }
    }
}