using System;
using ArmyGenerator.ArmyData;
using ArmyGenerator.ArmyList;
using Godot;
using Godot.Collections;

public partial class AvilableUnitListItem : MarginContainer
{
    public event Action<AvilableUnitListItem> OnAddPressed;

    [Export] private Array<TypeToColorSetting> unitTypeColorMap;

    [ExportSubgroup("UI")]
    [Export] private Label modelName;
    [Export] private Label modelTypeLabel;
    [Export] private Label pointCountLabel;
    [Export] private SpinBox inventorySpinner;

    [Export] private Button addButton;

    private UnitData unitData;

    public override void _Ready()
    {
        addButton.Pressed += () => OnAddPressed?.Invoke(this);
    }

    public void Initialize(UnitData unitDataValue)
    {
        unitData = unitDataValue;

        modelName.Text = unitDataValue.name;
        modelTypeLabel.Text = unitDataValue.type;

        pointCountLabel.Text = unitDataValue.modelCountToPriceMap[unitDataValue.FindMinimumKey()].ToString();

        inventorySpinner.MaxValue = unitDataValue.maxCount;
        inventorySpinner.Value = unitDataValue.maxCount;

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
}