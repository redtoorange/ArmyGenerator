using System;
using ArmyGenerator.ArmyData;
using Godot;
using Godot.Collections;

namespace ArmyGenerator.View;

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

    private UnitInventoryController unitInventoryController;
    private UnitData unitData;

    public override void _Ready()
    {
        addButton.Pressed += () => OnAddPressed?.Invoke(this);
    }

    public void Initialize(UnitData unitDataValue, UnitInventoryController unitInventoryController)
    {
        unitData = unitDataValue;
        this.unitInventoryController = unitInventoryController;

        modelName.Text = unitDataValue.name;
        modelTypeLabel.Text = unitDataValue.type;

        pointCountLabel.Text = unitDataValue.FindMinimumKey().points.ToString();

        inventorySpinner.MaxValue = unitDataValue.maxCount;
        inventorySpinner.Value = this.unitInventoryController.GetMaxCount(unitData);
        inventorySpinner.ValueChanged += HandleInventoryValueChanged;

        foreach (TypeToColorSetting colorSetting in unitTypeColorMap)
        {
            if (colorSetting.type.Equals(unitDataValue.type, StringComparison.OrdinalIgnoreCase))
            {
                modelTypeLabel.LabelSettings.FontColor = colorSetting.color;
            }
        }
    }

    private void HandleInventoryValueChanged(double value)
    {
        unitInventoryController.SetInventory(unitData, (int) value);
    }

    public UnitData GetUnitData()
    {
        return unitData;
    }
}