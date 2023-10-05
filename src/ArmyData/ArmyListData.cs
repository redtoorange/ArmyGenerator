using System.Collections.Generic;

namespace ArmyGenerator.ArmyData;

public class ArmyListData
{
    public string armyName;
    public Dictionary<int, UnitData> unitIdToDataMap;

    public ArmyListData(string armyName)
    {
        this.armyName = armyName;
        unitIdToDataMap = new Dictionary<int, UnitData>();
    }
}