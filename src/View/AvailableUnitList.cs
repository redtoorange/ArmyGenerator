using System;
using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using ArmyGenerator.ArmyList;
using Godot;

public partial class AvailableUnitList : VBoxContainer
{
    public event Action<UnitData> OnUnitAddNewUnitToSelected;
    
    [Export] private PackedScene unitListItemPrefab;
    [Export] private SelectedUnitList selectedUnitList;

    private Dictionary<string, AvilableUnitListItem> unitIdToListItemMap;

    public void Initialize(ArmyListData currentArmy)
    {
        unitIdToListItemMap = new Dictionary<string, AvilableUnitListItem>();
        
        foreach (KeyValuePair<string, UnitData> unitData in currentArmy.unitIdToDataMap)
        {
            GD.Print($"Adding {unitData.Value.name}");
            AvilableUnitListItem listItem = unitListItemPrefab.Instantiate<AvilableUnitListItem>();
            listItem.Initialize(unitData.Value);
            listItem.OnAddPressed += HandleUnitAdded;
            
            unitIdToListItemMap.Add(unitData.Key, listItem);
            
            AddChild(listItem);
        }
    }

    private void HandleUnitAdded(AvilableUnitListItem listItem)
    {
        OnUnitAddNewUnitToSelected?.Invoke(listItem.GetUnitData());
    }
}