using System;
using ArmyGenerator.ArmyData;
using Godot;
using Godot.Collections;

namespace ArmyGenerator.View;

public partial class SelectedUnitListItem : MarginContainer
{
    public delegate void UnitListItemModelsChanged(UnitData unitData, string unitReferenceId, int oldIndex, int newIndex);
    public event UnitListItemModelsChanged OnUnitListItemModelsChanged;

    
    public event Action<SelectedUnitListItem> OnRemovePressed;

    [Export] private Label modelName;
    [Export] private Label modelTypeLabel;
    [Export] private Array<TypeToColorSetting> unitTypeColorMap;
    [Export] private Button removeButton;
    [Export] private OptionButton pointsOption;

    private UnitData unitData;
    private string unitReferenceId;
    private int currentIndex;

    public override void _Ready()
    {
        removeButton.Pressed += () => OnRemovePressed?.Invoke(this);
    }

    public void Initialize(string unitReferenceId, UnitData unitDataValue)
    {
        unitData = unitDataValue;
        this.unitReferenceId = unitReferenceId;

        modelName.Text = unitDataValue.name;
        modelTypeLabel.Text = unitDataValue.type;

        foreach (TypeToColorSetting colorSetting in unitTypeColorMap)
        {
            if (colorSetting.type.Equals(unitDataValue.type, StringComparison.OrdinalIgnoreCase))
            {
                modelTypeLabel.LabelSettings.FontColor = colorSetting.color;
            }
        }

        pointsOption.Clear();
        for (int i = 0; i < unitDataValue.modelCountToPriceMap.Count; i++)
        {
            ModelsToPointData pointData = unitData.modelCountToPriceMap[i];
            string models = (pointData.models == 1) ? "model" : "models";
            string label = $"{pointData.models} {models} - {pointData.points} pts";
            pointsOption.AddItem(label, i);
        }

        if (pointsOption.ItemCount == 1)
        {
            pointsOption.Disabled = true;
        }

        pointsOption.ItemSelected += HandeItemSelected;
        currentIndex = 0;
    }

    private void HandeItemSelected(long index)
    {
        int previousIndex = currentIndex;
        int newIndex = (int) index;
        currentIndex = newIndex;
        
        OnUnitListItemModelsChanged?.Invoke(unitData, unitReferenceId, previousIndex, newIndex);
    }

    public UnitData GetUnitData()
    {
        return unitData;
    }

    public string GetUnitReferenceId()
    {
        return unitReferenceId;
    }
}