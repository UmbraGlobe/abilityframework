using System.Collections.Generic;
using UnityEngine;

public class EffectContainer
{
    public EffectInstance Instance;
    public ModifierContainer[] BaseModifiers;
    public Dictionary<Stat, ModifierContainer> AccumulatedModifiers = new Dictionary<Stat, ModifierContainer>();

    public void AddAccumulatedModifiers()
    {
        foreach (var container in BaseModifiers)
        {
            if (AccumulatedModifiers.ContainsKey(container.EffectedStat))
            {
                Debug.Log("Combining " + container.EffectedStat);
                AccumulatedModifiers[container.EffectedStat].SetModifier(StatModifier.Combine(AccumulatedModifiers[container.EffectedStat].Modifier, container.Modifier));
            }
            else
            {
                Debug.Log("Adding " + container.EffectedStat);
                AccumulatedModifiers.Add(container.EffectedStat, new ModifierContainer(container));
            }
        }
    }

    public class ModifierContainer
    {
        public Stat EffectedStat;
        public StatModifier Modifier;
        public EStatChange StatChange;

        public ModifierContainer()
        {
            EffectedStat = default;
            Modifier = new StatModifier();
            StatChange = default;
        }
        public ModifierContainer(ModifierContainer container)
        {
            EffectedStat = container.EffectedStat;
            Modifier = container.Modifier;
            StatChange = container.StatChange;
        }
        public void SetModifier(StatModifier modifier)
        {
            Modifier = modifier;
        }
    }
}

