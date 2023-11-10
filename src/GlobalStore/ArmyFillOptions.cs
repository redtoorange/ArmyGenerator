using ArmyGenerator.ArmyData;

namespace ArmyGenerator.GlobalStore;

public class ArmyFillOptions
{
    public bool NoCharacters = false;
    public bool NoEpicCharacters = false;
    public bool NoTransports = false;
    public bool NoBattleLine = false;
    public bool NoFortifications = true;
    public bool IncludeImperialAgents = false;
    public int ImperialAgentCharacterCount;
    public int ImperialAgentRetinueCount;

    public bool IsBanned(UnitData unitData)
    {
        switch (unitData.type)
        {
            case UnitTypes.CHARACTER:
                return NoCharacters;
            case UnitTypes.EPIC_CHARACTER:
                return NoEpicCharacters;
            case UnitTypes.BATTLELINE:
                return NoBattleLine;
            case UnitTypes.DEDICATED_TRANSPORT:
                return NoTransports;
            case UnitTypes.FORTIFICATION:
                return NoFortifications;
        }

        return false;
    }
}