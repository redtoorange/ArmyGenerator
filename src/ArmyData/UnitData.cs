using System.Collections.Generic;

namespace ArmyGenerator.ArmyData;

public class ModelsToPointData
{
    public int models;
    public int points;

    public ModelsToPointData(int models, int points)
    {
        this.models = models;
        this.points = points;
    }
}

public class UnitData
{
    public ArmyListData armyListData;
    public string unitId;
    public string name;
    public string type;
    public int maxCount;

    public List<ModelsToPointData> modelCountToPriceMap;

    public UnitData(ArmyListData armyListData, string unitId, string name, string type, int maxCount)
    {
        this.armyListData = armyListData;
        this.unitId = unitId;
        this.name = name;
        this.type = type;
        this.maxCount = maxCount;
        modelCountToPriceMap = new List<ModelsToPointData>();
    }

    public ModelsToPointData FindMinimumKey()
    {
        return modelCountToPriceMap[0];
    }
}