using System;
using System.Collections.Generic;

public static class StatCalculator
{
    public static void CalculateStats(EquipmentInstance[] equipment, Stats stats)
    {
        GenericDictionary<ItemStat, float> itemStats = new GenericDictionary<ItemStat, float>();
        foreach (ItemStat stat in Enum.GetValues(typeof(ItemStat)))
        {
            itemStats.Add(stat, 0);
        }

        foreach (EquipmentInstance item in equipment)
        {
            if (item == null) continue;

            foreach (KeyValuePair<StatData, float> affix in item.Implicits)
            {
                itemStats[affix.Key.Stat] += affix.Value;
            }

            foreach (KeyValuePair<StatData, float> affix in item.Affixes)
            {
                itemStats[affix.Key.Stat] += affix.Value;
            }
        }

        foreach (var stat in stats)
        {
            ItemStat[] itemStatsForStat = StatParser.StatToItemStats(stat.Key);
            for (int i = 0; i < itemStatsForStat.Length; i++)
            {
                stats.AddModifierValue(EStatChange.Max, stat.Key, itemStats[itemStatsForStat[i]], StatParser.GetItemStatModifierType(itemStatsForStat[i]), false);
            }

        }
    }
}