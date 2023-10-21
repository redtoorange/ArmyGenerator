using System;
using System.Collections;
using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using Godot;
using Godot.Collections;

namespace ArmyGenerator.View;

public partial class SelectedUnitList : VBoxContainer
{
    public event Action<string, UnitData> OnUnitRemoved;
    public event SelectedUnitListItem.UnitListItemModelsChanged OnUnitListModelsChanged;

    [Export] private PackedScene selectUnitListItem;

    private Array<SelectedUnitListItem> selectedUnitList = new Array<SelectedUnitListItem>();

    public void Initialize()
    {
    }

    public void AddUnit(string unitReferenceId, UnitData unit)
    {
        SelectedUnitListItem listItem = selectUnitListItem.Instantiate<SelectedUnitListItem>();
        selectedUnitList.Add(listItem);
        listItem.Initialize(unitReferenceId, unit);
        listItem.OnRemovePressed += HandleUnitRemoved;
        listItem.OnUnitListItemModelsChanged += HandleListItemChanged;
        AddChild(listItem);
    }

    private void HandleListItemChanged(UnitData unitData, string unitReferenceId, int oldIndex, int newIndex)
    {
        OnUnitListModelsChanged?.Invoke(unitData, unitReferenceId, oldIndex, newIndex);
    }

    private void HandleUnitRemoved(SelectedUnitListItem listItem)
    {
        OnUnitRemoved?.Invoke(listItem.GetUnitReferenceId(), listItem.GetUnitData());
        selectedUnitList.Remove(listItem);
        listItem.QueueFree();
    }

    public void ClearList()
    {
        for (int i = selectedUnitList.Count - 1; i >= 0; i--)
        {
            HandleUnitRemoved(selectedUnitList[i]);
        }

        selectedUnitList.Clear();
    }

    public void SortSelectUnits()
    {
        List<SelectedUnitListItem> sortedItems = new List<SelectedUnitListItem>(selectedUnitList);
        sortedItems.Sort(new SelectedUnitComparer());
        
        for (int i = 0; i < sortedItems.Count; i++)
        {
            RemoveChild(sortedItems[i]);
        }

        for (int i = 0; i < sortedItems.Count; i++)
        {
            AddChild(sortedItems[i]);
        }
    }
    
    class SelectedUnitComparer : IComparer<SelectedUnitListItem>
    {
        public int Compare(SelectedUnitListItem x, SelectedUnitListItem y)
        {
            if (x == null && y == null) return 0;
            if (y == null) return 1;
            if (x == null) return -1;

            int xIndex = Int32.Parse(x.GetUnitData().unitId.Split("_")[1]);
            int yIndex = Int32.Parse(y.GetUnitData().unitId.Split("_")[1]);
            
            return xIndex - yIndex;
        }
    }
}