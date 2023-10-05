using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace ArmyGenerator.ArmyData;

public partial class DataFileLoader : Node
{
    public static DataFileLoader S;

    public List<ArmyListData> loadedArmyListData;

    public override void _EnterTree()
    {
        S = this;
        LoadDataFile();
    }

    private void LoadDataFile()
    {
        FileAccess file = FileAccess.Open("res://data/Unit_Data.json", FileAccess.ModeFlags.Read);
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
            Dictionary armyList = parsedFile[armyName].AsGodotDictionary();
            ArmyListData listData = new ArmyListData(armyName);

            foreach (Variant unitIdVariant in armyList.Keys)
            {
                Dictionary unitEntry = armyList[unitIdVariant].AsGodotDictionary();
                UnitData unitData = new UnitData(
                    unitIdVariant.AsInt32(),
                    unitEntry["Name"].AsString(),
                    unitEntry["Type"].AsString(),
                    unitEntry["Max"].AsInt32()
                );
                unitData.modelCountToPriceMap = ParseUnitPrice(
                    unitEntry["Size"].AsString(),
                    unitEntry["Points"].AsString()
                );

                listData.unitIdToDataMap.Add(unitIdVariant.AsInt32(), unitData);
            }

            loadedArmyListData.Add(listData);
        }

        GD.Print($"Done Loading! Loaded {loadedArmyListData.Count} armies.");
        file.Dispose();
    }

    private Godot.Collections.Dictionary<int, int> ParseUnitPrice(string size, string points)
    {
        Godot.Collections.Dictionary<int, int> costMapping = new Godot.Collections.Dictionary<int, int>();

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
                costMapping.Add(
                    int.Parse(sizes[i]),
                    int.Parse(prices[i])
                );
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
                costMapping.Add(
                    i,
                    i * price
                );
            }
        }

        // This is the normal 1:1 mapping
        else
        {
            costMapping.Add(
                int.Parse(size),
                int.Parse(points)
            );
        }

        return costMapping;
    }
}