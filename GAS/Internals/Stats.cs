using System;
using System.Collections.Generic;
using UnityEngine;
using static EffectContainer;

[System.Serializable]
public class Stats : GenericDictionary<Stat, StatValue>
{
    public Stats() 
    {
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            Add(stat, new StatValue(0));
        }
        this[Stat.HealthRegeneration].ClampMin = false;
        this[Stat.ManaRegeneration].ClampMin = false;
        this[Stat.EnergyShieldRegeneration].ClampMin = false;
    }

    public event Action<Stat, float> OnStatCurrentChanged;
    public event Action<Stat, float> OnStatMaxChanged;
    public void Evaluate(EStatChange change, Stat stat, bool sendEvent)
    {
        switch (change)
        {
            case EStatChange.Current:
                EvaluateCurrent(stat, sendEvent);
                break;
            case EStatChange.Max:
                EvaluateMax(stat, sendEvent);
                break;
            case EStatChange.Both:
                EvaluateCurrent(stat, sendEvent);
                EvaluateMax(stat, sendEvent);
                break;
            default:
                break;
        }
    }

    private void EvaluateCurrent(Stat stat, bool sendEvent)
    {
        float originalValue = this[stat].Current;
        float newVal = (this[stat].RawCurrent + this[stat].CurrentModifier.Add) * (1 + this[stat].CurrentModifier.Increase / 100f) * (1 + this[stat].CurrentModifier.More / 100f);

        if (this[stat].CurrentModifier.Override != 0)
        {
            this[stat].Current = this[stat].CurrentModifier.Override;
            return;
        }
        else if (this[stat].ClampMin)
        {
            this[stat].Current = Mathf.Clamp(newVal, 0, this[stat].Max);
        }
        else
        {
            this[stat].Current = Mathf.Clamp(newVal, float.MinValue, this[stat].Max);
        }

        if (sendEvent)
        {
            OnStatCurrentChanged?.Invoke(stat, this[stat].Current - originalValue);
        }
    }
    private void EvaluateMax(Stat stat, bool sendEvent)
    {
        float originalValue = this[stat].Max;
        float newValue = (this[stat].RawMax + this[stat].MaxModifier.Add) * (1 + this[stat].MaxModifier.Increase / 100f) * (1 + this[stat].MaxModifier.More / 100f);
        if (this[stat].MaxModifier.Override != 0)
        {
            this[stat].Max = this[stat].MaxModifier.Override;
        }
        else if (this[stat].ClampMin)
        {
            this[stat].Max = Mathf.Clamp(newValue, 0, float.MaxValue);
        }
        else
        {
            this[stat].Max = newValue;
        }

        if (sendEvent)
        {
            OnStatMaxChanged?.Invoke(stat, this[stat].Max - originalValue);
        }
    }
    public void AddModifierValue(EStatChange change, Stat stat, float value, EModifierOperator type, bool sendEvent)
    {
        switch (change)
        { 
            case EStatChange.Current:
                this[stat].AddCurrentMod(value, type);
                EvaluateCurrent(stat, sendEvent);
                break;
            case EStatChange.Max:
                this[stat].AddMaxMod(value, type);
                EvaluateMax(stat, sendEvent);
                break;
            case EStatChange.Both:
                this[stat].AddCurrentMod(value, type);
                EvaluateCurrent(stat, sendEvent);
                this[stat].AddMaxMod(value, type);
                EvaluateMax(stat, sendEvent);
                break;
            default:
                break;
        }
    }

    public void SubtractModifier(EStatChange change, Stat stat, StatModifier mod, bool sendEvent)
    {
        switch (change)
        {
            case EStatChange.Current:
                this[stat].SubtractCurrentMod(mod);
                EvaluateCurrent(stat, sendEvent);
                break;
            case EStatChange.Max:
                this[stat].SubtractMaxMod(mod);
                EvaluateMax(stat, sendEvent);
                break;
            case EStatChange.Both:
                this[stat].SubtractCurrentMod(mod);
                EvaluateCurrent(stat, sendEvent);
                this[stat].SubtractMaxMod(mod);
                EvaluateMax(stat, sendEvent);
                break;
            default:
                break;
        }
    }

    

    public void ChangeRaw(EStatChange change, Stat stat, float value, bool sendEvent)
    {
        switch (change)
        {
            case EStatChange.Current:
                ChangeRawCurrent(stat, value, sendEvent);
                break;
            case EStatChange.Max:
                ChangeRawMax(stat, value, sendEvent);
                break;
            case EStatChange.Both:
                ChangeRawCurrent(stat, value, sendEvent);
                ChangeRawMax(stat, value, sendEvent);
                break;
            default:
                break;
        }
    }

    private void ChangeRawMax(Stat stat, float value, bool sendEvent)
    {
        this[stat].RawMax += value;
        EvaluateMax(stat, sendEvent);
    }

    private void ChangeRawCurrent(Stat stat, float value, bool sendEvent)
    {
        if (this[stat].ClampMin)
        {
            this[stat].RawCurrent = Mathf.Clamp(this[stat].Current + value, 0, this[stat].Max);
        }
        else
        {
            this[stat].RawCurrent = Mathf.Clamp(this[stat].Current + value, float.MinValue, this[stat].Max);
        }
        EvaluateCurrent(stat, sendEvent);
    }


    public void ResetAllCurrentToMax(bool evaluate, bool sendEvent)
    {
        foreach (Stat stat in Keys)
        {
            if (evaluate)
            { 
                EvaluateMax(stat, sendEvent);
            }
            this[stat].RawCurrent = this[stat].Max;
            this[stat].Current = this[stat].Max;
            this[stat].ResetCurrent();
        }
    }

    public static void CombineAll(Stats stats1, Stats stats2, bool sendEvent)
    {
        foreach (var stat in stats1)
        {
            stats1[stat.Key].RawMax += stats2[stat.Key].RawMax;
            stats1.AddModifierValue(EStatChange.Max, stat.Key, stats2[stat.Key].MaxModifier.Add, EModifierOperator.Add, sendEvent);
            stats1.AddModifierValue(EStatChange.Max, stat.Key, stats2[stat.Key].MaxModifier.Increase, EModifierOperator.Increase, sendEvent);
            stats1.AddModifierValue(EStatChange.Max, stat.Key, stats2[stat.Key].MaxModifier.More, EModifierOperator.More, sendEvent);
            stats1.AddModifierValue(EStatChange.Max, stat.Key, stats2[stat.Key].MaxModifier.Override, EModifierOperator.Override, sendEvent);
            stats1.EvaluateMax(stat.Key, sendEvent);
        }

    }

    public void ResetAll()
    {
        foreach (var stat in Keys)
        {
            this[stat].Reset();
        }
    }
}

[System.Serializable]
public class StatValue
{
    public bool ClampMin;
    public float Max;
    public float Current;
    [SerializeField] private StatModifier maxModifier;
    [SerializeField] private StatModifier currentModifier;
    public float RawMax;
    public float RawCurrent;
    public StatModifier MaxModifier => maxModifier;
    public StatModifier CurrentModifier => currentModifier;
    public void Reset()
    {
        RawMax = 0;
        maxModifier = new StatModifier();
    }

    public void ResetCurrent()
    {
        currentModifier = new StatModifier();
    }

    public StatValue(float _rawMax, StatModifier _maxModifier = new StatModifier(), StatModifier _currentModifier = new StatModifier(), bool _clampMin = true)
    {
        Max = _rawMax;
        RawMax = _rawMax;
        RawCurrent = _rawMax;
        Current = _rawMax;
        maxModifier = _maxModifier;
        currentModifier = _currentModifier;
        ClampMin = _clampMin;
    }

    public void SubtractCurrentMod(StatModifier mod)
    {
        currentModifier.Add -= mod.Add;
        currentModifier.Increase -= mod.Increase;
        currentModifier.More -= mod.More;
        currentModifier.Override -= mod.Override;
    }

    public void SubtractMaxMod(StatModifier mod)
    {
        maxModifier.Add -= mod.Add;
        maxModifier.Increase -= mod.Increase;
        maxModifier.More -= mod.More;
        maxModifier.Override -= mod.Override;
    }

    public void AddCurrentMod(float value, EModifierOperator type)
    {
        switch (type)
        {
            case EModifierOperator.Add:
                currentModifier.Add += value;
                break;
            case EModifierOperator.Increase:
                currentModifier.Increase += value;
                break;
            case EModifierOperator.More:
                currentModifier.More += value;
                break;
            case EModifierOperator.Override:
                currentModifier.Override += value;
                break;
            default:
                break;
        }
    }

    public void AddMaxMod(float value, EModifierOperator type)
    {
        switch (type)
        {
            case EModifierOperator.Add:
                maxModifier.Add += value;
                break;
            case EModifierOperator.Increase:
                maxModifier.Increase += value;
                break;
            case EModifierOperator.More:
                maxModifier.More += value;
                break;
            case EModifierOperator.Override:
                maxModifier.Override += value;
                break;
            default:
                break;
        }
    }
}


[System.Serializable]
public class ItemStats : GenericDictionary<StatData, float>
{
}