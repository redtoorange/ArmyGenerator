using ArmyGenerator.ArmyData;

namespace ArmyGenerator.GlobalStore;

public class ArmyFillOptions
{
    public bool NoCharacters;
    public bool NoEpicCharacters;
    public bool NoTransports;
    public bool NoBattleLine;
    public bool NoFortifications;

    public bool IsBanned(UnitData unitData)
    {
        switch (unitData.type)
        {
            case "Character":
                return NoCharacters;
            case "Epic Character":
                return NoEpicCharacters;
            case "Battleline":
                return NoBattleLine;
            case "Dedicated Transport":
                return NoTransports;
            case "Fortification":
                return NoFortifications;
        }

        return false;
    }
}