using System;
using Godot.Collections;

namespace ArmyGenerator.ArmyData;

public class UnitData
{
    public int unitId;
    public string name;
    public string type;
    public int maxCount;
    public Dictionary<int, int> modelCountToPriceMap;

    public UnitData(int unitId, string name, string type, int maxCount)
    {
        this.unitId = unitId;
        this.name = name;
        this.type = type;
        this.maxCount = maxCount;
        modelCountToPriceMap = new Dictionary<int, int>();
    }

    public int FindMinimumKey()
    {
        int minKey = Int32.MaxValue;
        foreach (int key in modelCountToPriceMap.Keys)
        {
            if (key < minKey)
            {
                minKey = key;
            }
        }
        return minKey;
    }
}