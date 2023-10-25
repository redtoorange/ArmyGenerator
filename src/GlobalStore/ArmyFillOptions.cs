using ArmyGenerator.ArmyData;

namespace ArmyGenerator.GlobalStore;

public class ArmyFillOptions
{
    public bool NoCharacters;
    public bool NoEpicCharacters;
    public bool NoTransports;
    public bool NoBattleLine;
    public bool NoFortifications;
    public bool IncludeImperialAgents;
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