using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using Godot;

namespace ArmyGenerator.Model;

public class SelectArmyUnitModel
{
    public string unitReferenceId;
    public int modelCount;
    public int pointCount;
    public UnitData sourceUnitData;
}

public class SelectedArmyModel
{
    private int pointCount;
    private Dictionary<string, List<SelectArmyUnitModel>> mapOfSourceIdToUnit;

    public SelectedArmyModel()
    {
        mapOfSourceIdToUnit = new Dictionary<string, List<SelectArmyUnitModel>>();
    }

    public List<SelectArmyUnitModel> GetUnitsOfType(UnitData unitData)
    {
        mapOfSourceIdToUnit.TryGetValue(unitData.unitId, out List<SelectArmyUnitModel> matchingUnits);
        return matchingUnits;
    }

    public int GetUnitsOfTypeCount(UnitData unitData)
    {
        List<SelectArmyUnitModel> units = GetUnitsOfType(unitData);

        if (units == null)
        {
            return 0;
        }

        return units.Count;
    }

    public bool AddToArmy(UnitData unitData, out string unitReferenceId)
    {
        // See if we already have units of this type
        if (mapOfSourceIdToUnit.TryGetValue(unitData.unitId, out List<SelectArmyUnitModel> matchingUnits))
        {
            SelectArmyUnitModel newModel = new SelectArmyUnitModel();
            newModel.unitReferenceId = GenerateReferenceId(matchingUnits, unitData.unitId);
            newModel.modelCount = unitData.FindMinimumKey().models;
            newModel.pointCount = unitData.FindMinimumKey().points;

            pointCount += newModel.pointCount;

            unitReferenceId = newModel.unitReferenceId;
            matchingUnits.Add(newModel);
        }
        // No Units of this Type
        else
        {
            List<SelectArmyUnitModel> newList = new List<SelectArmyUnitModel>();
            SelectArmyUnitModel newModel = new SelectArmyUnitModel();
            newModel.unitReferenceId = GenerateReferenceId(newList, unitData.unitId);
            newModel.modelCount = unitData.FindMinimumKey().models;
            newModel.pointCount = unitData.FindMinimumKey().points;

            pointCount += newModel.pointCount;

            unitReferenceId = newModel.unitReferenceId;
            newList.Add(newModel);

            mapOfSourceIdToUnit.Add(unitData.unitId, newList);
        }

        return true;
    }

    public void RemoveFromArmy(string unitReferenceId, UnitData unitData)
    {
        // Locate the matching units by unitId
        if (mapOfSourceIdToUnit.TryGetValue(unitData.unitId, out List<SelectArmyUnitModel> matchingUnits))
        {
            int index = matchingUnits.FindIndex(i => i.unitReferenceId.Equals(unitReferenceId));
            if (index >= 0)
            {
                SelectArmyUnitModel unit = matchingUnits[index];
                pointCount -= unit.pointCount;
                matchingUnits.RemoveAt(index);
            }
            else
            {
                GD.PrintErr("Tried to remove a unit that is not found");
            }
        }
    }

    public void UpdateItem()
    {
    }

    private string GenerateReferenceId(List<SelectArmyUnitModel> others, string prefix)
    {
        bool isUnique = false;
        string possibleKey = "";

        while (!isUnique)
        {
            possibleKey = prefix + "__" + GD.Randi();
            isUnique = true;
            for (int i = 0; i < others.Count; i++)
            {
                if (others[i].unitReferenceId.Equals(possibleKey))
                {
                    isUnique = false;
                }
            }
        }

        return possibleKey;
    }

    public int GetPoints()
    {
        return pointCount;
    }
}