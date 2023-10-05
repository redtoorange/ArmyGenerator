using System;
using ArmyGenerator.ArmyData;
using Godot;

namespace ArmyGenerator.ArmyList;

public partial class SelectedUnitList : VBoxContainer
{
    public event Action<int, UnitData> OnUnitRemoved;
    
    [Export] private PackedScene selectUnitListItem;

    public void Initialize()
    {
    }

    public void AddUnit(int unitReferenceId, UnitData unit)
    {
        SelectedUnitListItem listItem = selectUnitListItem.Instantiate<SelectedUnitListItem>();
        listItem.Initialize(unitReferenceId, unit);
        listItem.OnRemovePressed += HandleUnitRemoved;
        AddChild(listItem);
    }
    
    private void HandleUnitRemoved(SelectedUnitListItem listItem)
    {
        OnUnitRemoved?.Invoke(listItem.GetUnitReferenceId(), listItem.GetUnitData());
        listItem.QueueFree();
    }
}