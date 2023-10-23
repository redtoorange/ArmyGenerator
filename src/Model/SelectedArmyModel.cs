using System;
using System.Collections.Generic;
using System.Text;
using ArmyGenerator.ArmyData;
using Godot;

namespace ArmyGenerator.Model;

public class SelectArmyUnitModel
{
    public string unitReferenceId;
    public int modelCount;
    public int pointCount;
    public UnitData sourceUnitData;

    public override string ToString()
    {
        return $"  - [{modelCount}] {sourceUnitData.name} - {pointCount}pts";
    }
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
            newModel.sourceUnitData = unitData;

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
            newModel.sourceUnitData = unitData;

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
                GD.PrintErr("Tried to remove a unit that is not found: " + unitReferenceId);
            }
        }
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

    public void UpdateItem(UnitData unitData, string unitReferenceId, int oldIndex, int newIndex)
    {
        if (mapOfSourceIdToUnit.TryGetValue(unitData.unitId, out List<SelectArmyUnitModel> matchingUnits))
        {
            int index = matchingUnits.FindIndex(i => i.unitReferenceId.Equals(unitReferenceId));
            if (index >= 0)
            {
                SelectArmyUnitModel unit = matchingUnits[index];
                pointCount -= unit.pointCount;

                unit.modelCount = unitData.modelCountToPriceMap[newIndex].models;
                unit.pointCount = unitData.modelCountToPriceMap[newIndex].points;

                pointCount += unit.pointCount;
            }
            else
            {
                GD.PrintErr("Tried to unit a unit that is not found: " + unitReferenceId);
            }
        }
    }

    public string GetArmyAsString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        List<SelectArmyUnitModel> characters = new List<SelectArmyUnitModel>();
        List<SelectArmyUnitModel> battleLine = new List<SelectArmyUnitModel>();
        List<SelectArmyUnitModel> transports = new List<SelectArmyUnitModel>();
        List<SelectArmyUnitModel> fortifications = new List<SelectArmyUnitModel>();
        List<SelectArmyUnitModel> other = new List<SelectArmyUnitModel>();

        foreach (List<SelectArmyUnitModel> units in mapOfSourceIdToUnit.Values)
        {
            foreach (SelectArmyUnitModel unit in units)
            {
                switch (unit.sourceUnitData.type)
                {
                    case "Character":
                    case "Epic Character":
                        characters.Add(unit);
                        break;
                    case "Battleline":
                        battleLine.Add(unit);
                        break;
                    case "Dedicated Transport":
                        transports.Add(unit);
                        break;
                    case "Fortification":
                        fortifications.Add(unit);
                        break;
                    case "Other":
                        other.Add(unit);
                        break;
                }
            }
        }

        if (characters.Count > 0)
        {
            stringBuilder.AppendLine("# Characters");
            characters.Sort(new SelectedUnitComparer());
            foreach (SelectArmyUnitModel unit in characters)
            {
                stringBuilder.AppendLine(unit.ToString());
            }
        }

        if (battleLine.Count > 0)
        {
            stringBuilder.AppendLine("# Battleline");
            battleLine.Sort(new SelectedUnitComparer());
            foreach (SelectArmyUnitModel unit in battleLine)
            {
                stringBuilder.AppendLine(unit.ToString());
            }
        }


        if (transports.Count > 0)
        {
            stringBuilder.AppendLine("# Dedicated Transports");
            transports.Sort(new SelectedUnitComparer());
            foreach (SelectArmyUnitModel unit in transports)
            {
                stringBuilder.AppendLine(unit.ToString());
            }
        }


        if (fortifications.Count > 0)
        {
            stringBuilder.AppendLine("# Fortifications");
            fortifications.Sort(new SelectedUnitComparer());
            foreach (SelectArmyUnitModel unit in fortifications)
            {
                stringBuilder.AppendLine(unit.ToString());
            }
        }


        if (other.Count > 0)
        {
            stringBuilder.AppendLine("# Other");
            other.Sort(new SelectedUnitComparer());
            foreach (SelectArmyUnitModel unit in other)
            {
                stringBuilder.AppendLine(unit.ToString());
            }
        }


        return stringBuilder.ToString();
    }

    class SelectedUnitComparer : IComparer<SelectArmyUnitModel>
    {
        public int Compare(SelectArmyUnitModel x, SelectArmyUnitModel y)
        {
            if (x == null && y == null) return 0;
            if (y == null) return 1;
            if (x == null) return -1;

            int xIndex = Int32.Parse(x.sourceUnitData.unitId.Split("_")[1]);
            int yIndex = Int32.Parse(y.sourceUnitData.unitId.Split("_")[1]);

            return xIndex - yIndex;
        }
    }
}