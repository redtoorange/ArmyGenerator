using System;
using ArmyGenerator.ArmyData;
using Godot;
using Godot.Collections;

namespace ArmyGenerator.View;

public partial class AvilableUnitListItem : MarginContainer
{
    public event Action<AvilableUnitListItem> OnAddPressed;

    [Export] private Array<TypeToColorSetting> unitTypeColorMap;

    [ExportSubgroup("UI")] [Export] private Label modelName;
    [Export] private Label modelTypeLabel;
    [Export] private Label pointCountLabel;
    [Export] private SpinBox inventorySpinner;
    [Export] private Button addButton;

    [Export] private Button showButton;
    [Export] private Button hideButton;

    private bool isHidden = false;


    private UnitInventoryController unitInventoryController;
    private UnitVisibilityController unitVisibilityController;
    private UnitData unitData;
    public bool IsHidden() => isHidden;

    public override void _Ready()
    {
        addButton.Pressed += () => OnAddPressed?.Invoke(this);

        showButton.Pressed += HandleShowHidePressed;
        hideButton.Pressed += HandleShowHidePressed;
    }

    private void HandleShowHidePressed()
    {
        isHidden = !isHidden;
        showButton.Visible = isHidden;
        hideButton.Visible = !isHidden;
        unitVisibilityController.SetVisibility(unitData, !isHidden);
    }

    public void Initialize(
        UnitData unitDataValue,
        UnitInventoryController unitInventoryController,
        UnitVisibilityController unitVisibilityController
    )
    {
        unitData = unitDataValue;
        this.unitInventoryController = unitInventoryController;
        this.unitVisibilityController = unitVisibilityController;

        modelName.Text = unitDataValue.name;
        modelTypeLabel.Text = unitDataValue.type;

        pointCountLabel.Text = unitDataValue.FindMinimumKey().points.ToString();

        inventorySpinner.MaxValue = unitDataValue.maxCount;
        inventorySpinner.Value = this.unitInventoryController.GetMaxCount(unitData);
        inventorySpinner.ValueChanged += HandleInventoryValueChanged;
        isHidden = !this.unitVisibilityController.GetVisibilityPreference(unitDataValue);

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
        unitInventoryController.SetInventory(unitData, (int)value);
    }

    public UnitData GetUnitData()
    {
        return unitData;
    }

    public void ToggleShowHideButtonDisplay(bool shouldShow)
    {
        if (shouldShow)
        {
            addButton.Visible = false;
            showButton.Visible = isHidden;
            hideButton.Visible = !isHidden;
        }
        else
        {
            addButton.Visible = true;
            showButton.Visible = false;
            hideButton.Visible = false;
        }
    }
}