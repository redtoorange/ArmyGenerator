using System.Collections.Generic;
using ArmyGenerator.ArmyData;

namespace ArmyGenerator.Model;


public class SelectArmyUnitModel
{
    public int unitReferenceId;
    public int modelCount;
    public int pointCount;
    public UnitData sourceUnitData;
}


public class SelectedArmyModel
{
    private Dictionary<int, List<SelectArmyUnitModel>> mapOfSourceIdToUnit;
    
    public bool AddToArmy(UnitData unitData, out int unitReferenceId)
    {
        // See if we already have units of this type
        if (mapOfSourceIdToUnit.TryGetValue(unitData.unitId, out List<SelectArmyUnitModel> matchingUnits))
        {
            SelectArmyUnitModel newModel = new SelectArmyUnitModel();
            newModel.unitReferenceId = (unitData.unitId * 1000) + 1;
            unitReferenceId = newModel.unitReferenceId;
            matchingUnits.Add(newModel);
        }
        // No Units of this Type
        else
        {
            List<SelectArmyUnitModel> newList = new List<SelectArmyUnitModel>();

            SelectArmyUnitModel newModel = new SelectArmyUnitModel();
            newModel.unitReferenceId = (unitData.unitId * 1000) + 1;
            unitReferenceId = newModel.unitReferenceId;
            newList.Add(newModel);
            
            mapOfSourceIdToUnit.Add(unitData.unitId, newList);
        }
        
        return true;
    }

    public void RemoveFromArmy(int unitReferenceId, UnitData unitData)
    {
        // Locate the matching units by unitId
        if (mapOfSourceIdToUnit.TryGetValue(unitData.unitId, out List<SelectArmyUnitModel> matchingUnits))
        {
            
        }
    }

    public void UpdateItem()
    {
        
    }
}