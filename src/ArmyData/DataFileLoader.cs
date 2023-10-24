using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace ArmyGenerator.ArmyData;

public partial class DataFileLoader : Node
{
    public static DataFileLoader S;

    public List<ArmyListData> loadedArmyListData;

    private int selectedArmy = 0;

    public override void _EnterTree()
    {
        S = this;
        LoadDataFile();
    }

    private void LoadDataFile()
    {
        FileAccess file = FileAccess.Open("res://Units.data", FileAccess.ModeFlags.Read);
        string fileText = file.GetAsText();
        Dictionary parsedFile = Json.ParseString(fileText).AsGodotDictionary();

        // Load the keys for the armys
        List<string> armyNames = new List<string>();
        foreach (Variant key in parsedFile.Keys)
        {
            armyNames.Add(key.AsString());
        }


        loadedArmyListData = new List<ArmyListData>();
        foreach (string armyName in armyNames)
        {
            Array armyList = parsedFile[armyName].AsGodotArray();
            ArmyListData listData = new ArmyListData(armyName);

            foreach (Variant unitIdVariant in armyList)
            {
                Dictionary unitEntry = unitIdVariant.AsGodotDictionary();
                UnitData unitData = new UnitData(
                    unitEntry["UnitId"].AsString(),
                    unitEntry["Name"].AsString(),
                    unitEntry["Type"].AsString(),
                    unitEntry["Max"].AsInt32()
                );
                unitData.modelCountToPriceMap = ParseUnitPrice(
                    unitEntry["Size"].AsString(),
                    unitEntry["Points"].AsString()
                );

                listData.unitIdToDataMap.Add(unitData.unitId, unitData);
            }

            loadedArmyListData.Add(listData);
        }

        GD.Print($"Done Loading! Loaded {loadedArmyListData.Count} armies.");
        file.Dispose();
    }

    private List<ModelsToPointData> ParseUnitPrice(string size, string points)
    {
        List<ModelsToPointData> costMapping = new List<ModelsToPointData>();

        // There is a split operation
        if (size.Contains(","))
        {
            if (!points.Contains(","))
            {
                GD.PrintErr($"Invalid formatting: size: {size} and points: {points}");
                return null;
            }

            string[] sizes = size.Split(",");
            string[] prices = points.Split(",");
            if (sizes.Length != prices.Length)
            {
                GD.PrintErr($"Invalid formatting: size: {size} and points: {points} : tokens must be the same length.");
                return null;
            }

            for (int i = 0; i < sizes.Length; i++)
            {
                costMapping.Add(new ModelsToPointData(
                    int.Parse(sizes[i]), int.Parse(prices[i])
                ));
            }
        }

        // There is a range operation
        else if (size.Contains("-"))
        {
            if (points.Contains(",") || points.Contains("-"))
            {
                GD.PrintErr($"Invalid formatting: size: {size} and points: {points}");
                return null;
            }

            string[] minMax = size.Split("-");
            if (minMax.Length != 2)
            {
                GD.PrintErr($"Invalid formatting: size: {size} range must be min-max format");
                return null;
            }

            int min = int.Parse(minMax[0]);
            int max = int.Parse(minMax[1]);
            int price = int.Parse(points);


            for (int i = min; i <= max; i++)
            {
                costMapping.Add(new ModelsToPointData(
                    i, i * price
                ));
            }
        }

        // This is the normal 1:1 mapping
        else
        {
            costMapping.Add(new ModelsToPointData(
                int.Parse(size), int.Parse(points)
            ));
        }

        return costMapping;
    }

    public void SetSelectedArmy(int armyOptionsSelected)
    {
        selectedArmy = armyOptionsSelected;
    }

    public ArmyListData GetArmyList(int index)
    {
        return loadedArmyListData[index];
    }
    
    public ArmyListData GetSelectedArmyList()
    {
        return loadedArmyListData[selectedArmy];
    }
}