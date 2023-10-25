using System.Collections.Generic;
using ArmyGenerator.ArmyData;
using ArmyGenerator.Model;

namespace ArmyGenerator.ImperialAgent;

public class ImperialAgentManager
{
    private int maxCharacters;
    private int currentCharacters;

    private int maxRetinue;
    private int currentRetinue;

    public ImperialAgentManager(int maxCharacters, int maxRetinue, List<SelectArmyUnitModel> currentUnits)
    {
        this.maxCharacters = maxCharacters;
        this.maxRetinue = maxRetinue;

        currentCharacters = 0;
        currentRetinue = 0;

        foreach (SelectArmyUnitModel unitModel in currentUnits)
        {
            if (unitModel.sourceUnitData.armyListData.armyName.Equals(ArmyNames.IMPERIAL_AGENTS))
            {
                string unitType = unitModel.sourceUnitData.type;
                if (unitType.Equals(UnitTypes.CHARACTER) || unitType.Equals(UnitTypes.EPIC_CHARACTER))
                {
                    currentCharacters++;
                }
                else
                {
                    currentRetinue++;
                }
            }
        }
    }
    
    public void UnitAdded(UnitData unitData)
    {
        if (unitData.armyListData.armyName.Equals(ArmyNames.IMPERIAL_AGENTS))
        {
            string unitType = unitData.type;
            if (unitType.Equals(UnitTypes.CHARACTER) || unitType.Equals(UnitTypes.EPIC_CHARACTER))
            {
                currentCharacters++;
            }
            else
            {
                currentRetinue++;
            }
        }
    }

    public bool IsAllowed(UnitData unitData)
    {
        if (unitData.armyListData.armyName.Equals(ArmyNames.IMPERIAL_AGENTS))
        {
            string unitType = unitData.type;
            if (unitType.Equals(UnitTypes.CHARACTER) || unitType.Equals(UnitTypes.EPIC_CHARACTER))
            {
                return currentCharacters < maxCharacters;
            }
            else
            {
                return currentRetinue < maxRetinue;
            }
        }

        return true;
    }
}