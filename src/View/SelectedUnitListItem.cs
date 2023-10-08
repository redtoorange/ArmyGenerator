using System;
using ArmyGenerator.ArmyData;
using ArmyGenerator.ArmyList;
using Godot;
using Godot.Collections;

public partial class SelectedUnitListItem : MarginContainer
{
    public event Action<SelectedUnitListItem> OnRemovePressed;
    
    [Export] private Label modelName;
    [Export] private Label modelTypeLabel;
    [Export] private Array<TypeToColorSetting> unitTypeColorMap;
    [Export] private Button removeButton;

    private UnitData unitData;
    private string unitReferenceId;

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