using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class AbilityCharacter : MonoBehaviour
{
    [SerializeField] protected readonly Stats stats = new Stats();
    [SerializeField] protected List<AbilityInstance> abilityInstances = new List<AbilityInstance>();
    [SerializeField] protected List<EffectContainer> appliedEffects = new List<EffectContainer>();
    [SerializeField] protected List<EffectSO> startingEffects = new List<EffectSO>();
    public List<AbilityInstance> AbilityInstances => abilityInstances;
    public List<EffectContainer> AppliedEffects => appliedEffects;
    public Stats Stats => stats;

    #region Boilerplate

    [SerializeField] protected SpriteRenderer sprite;

    protected bool isDead = false;
    protected bool isFlashing = false;
    protected IEnumerator Flash()
    {
        if (isFlashing) yield break;
        isFlashing = true;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        isFlashing = false;
    }

    public virtual void OnEnable()
    {
        Debug.Log(name + " enabled");
        stats.OnStatCurrentChanged += StatCurrentChange;
        stats.OnStatMaxChanged += StatMaxChange;
        foreach (var effect in startingEffects)
        {
            EffectInstance instance = EffectInstance.CreateNew(effect, caster: this, target: this);
            ApplyEffectInstanceToSelf(instance);
        }
    }

    public virtual void OnDisable()
    {
        stats.OnStatCurrentChanged -= StatCurrentChange;
        stats.OnStatMaxChanged -= StatMaxChange;
        ClearEffects();
    }

    protected abstract void StatCurrentChange(Stat stat, float value);
    protected abstract void StatMaxChange(Stat stat, float value);
    public abstract void TakeDamage();
    public abstract void Die();

    #endregion

    public List<DamageData> GenerateDamage()
    {
        List<DamageData> damageDatas = new List<DamageData>();
        foreach (var damageStat in StatParser.GetDamageTypes())
        {
            damageDatas.Add(new DamageData(StatParser.StatToDamage(damageStat), (int)stats[damageStat].Current));
        }
        return damageDatas;
    }

    public EffectInstance MakeOutgoingEffect(EffectSO effect)
    {
        return EffectInstance.CreateNew(effect: effect, caster: this);
    }

    public void GrantAbility(AbilityInstance spec)
    {
        this.abilityInstances.Add(spec);
    }

    public void RevokeAbility(AbilityInstance spec)
    {
        this.abilityInstances.Remove(spec);
    }

    public bool ApplyEffectInstanceToSelf(EffectInstance effectInstance)
    {
        if (effectInstance == null) return true;
        effectInstance.SetTarget(this);
        bool tagRequirementsOK = CheckTagRequirementsMet(effectInstance);

        if (tagRequirementsOK == false) return false;

        switch (effectInstance.EffectSO.DurationPolicy)
        {
            case EDurationPolicy.HasDuration:
                ApplyDurationalEffect(effectInstance);
                break;
            case EDurationPolicy.Instant:
                ApplyInstantEffect(effectInstance);
                return true;
        }
        return true;
    }
    private void ApplyInstantEffect(EffectInstance effectInstance)
    {
        for (var i = 0; i < effectInstance.EffectSO.EffectModifiers.Length; i++)
        {
            float postChange = 0;
            float preChange = 0;
            EffectModifier modifier = effectInstance.EffectSO.EffectModifiers[i];
            float magnitude = (modifier.ModifierMagnitude.CalculateMagnitude(effectInstance) * modifier.Multiplier);
            Stat stat = modifier.Stat;
            StatValue statValue = this.Stats[stat];

            if (modifier.StatChanged == EStatChange.Current)
            {
                if (modifier.ModifierOperator == EModifierOperator.Raw)
                {
                    preChange = statValue.RawCurrent;
                    stats.ChangeRaw(EStatChange.Current, stat, magnitude, sendEvent: true);
                    postChange = statValue.RawCurrent;
                }
                else
                {
                    preChange = statValue.Current;
                    stats.AddModifierValue(EStatChange.Current, stat, magnitude, modifier.ModifierOperator, sendEvent: true);
                    postChange = statValue.Current;
                }
            }
            else if (modifier.StatChanged == EStatChange.Max)
            {
                if (modifier.ModifierOperator == EModifierOperator.Raw)
                {
                    preChange = statValue.RawMax;
                    stats.ChangeRaw(EStatChange.Max, stat, magnitude, sendEvent: true);
                    postChange = statValue.RawMax;
                }
                else
                {
                    preChange = statValue.Max;
                    stats.AddModifierValue(EStatChange.Max, stat, magnitude, modifier.ModifierOperator, sendEvent: true);
                    postChange = statValue.Max;
                }
            }
            else if (modifier.StatChanged == EStatChange.Both)
            {
                if (modifier.ModifierOperator == EModifierOperator.Raw)
                {
                    preChange = statValue.RawMax;
                    stats.ChangeRaw(EStatChange.Max, stat, magnitude, sendEvent: true);
                    stats.ChangeRaw(EStatChange.Current, stat, magnitude, sendEvent: true);
                    postChange = statValue.RawMax;
                }
                else
                {
                    //could have errors with preChange and postChange
                    preChange = statValue.Current;
                    stats.AddModifierValue(EStatChange.Max, stat, magnitude, modifier.ModifierOperator, sendEvent: true);
                    stats.AddModifierValue(EStatChange.Current, stat, magnitude, modifier.ModifierOperator, sendEvent: true);
                    postChange = statValue.Current;
                }
            }
            effectInstance.statChange = Mathf.Abs(preChange - postChange);
            modifier.EffectCue?.ExecuteCue(effectInstance);
        }
    }

    /*private void RevertInstantEffect(EffectInstance effectInstance)
    {
        for (var i = 0; i < effectInstance.EffectSO.EffectModifiers.Length; i++)
        {
            EffectModifier modifier = effectInstance.EffectSO.EffectModifiers[i];
            if (modifier.ModifierOperator == EStatModifier.Raw)
            {
                Debug.LogError("Tried to revert raw");
                continue;
            }
            float magnitude = (modifier.ModifierMagnitude.CalculateMagnitude(effectInstance) * modifier.Multiplier);
            Stat stat = modifier.Stat;

            if (effectInstance.EStatChange == EStatChange.Current)
            {
                stats.AddCurrentModifier(stat, -magnitude, modifier.ModifierOperator, modifier.min, Stats[modifier.maxStatClamp].Max, modifier.clampMin, modifier.clampMax, sendEvent: true);
            }
            else if (effectInstance.EStatChange == EStatChange.Max)
            {
                stats.AddMaxModifier(stat, -magnitude, modifier.ModifierOperator, sendEvent: true);
            }
            else if (effectInstance.EStatChange == EStatChange.Both)
            {
                stats.AddMaxModifier(stat, -magnitude, modifier.ModifierOperator, sendEvent: true);
                stats.SetCurrentToMax(stat, evaluate: false, sendEvent: false);
            }
        }
    }*/

    void ApplyDurationalEffect(EffectInstance effectInstance)
    {
        var modifiersToApply = new List<EffectContainer.ModifierContainer>();
        for (var i = 0; i < effectInstance.EffectSO.EffectModifiers.Length; i++)
        {
            EffectModifier modifier = effectInstance.EffectSO.EffectModifiers[i];
            float magnitude = (modifier.ModifierMagnitude.CalculateMagnitude(effectInstance) * modifier.Multiplier);
            Stat stat = modifier.Stat;

            StatModifier mod = new StatModifier(magnitude, modifier.ModifierOperator);

            modifiersToApply.Add(new EffectContainer.ModifierContainer { EffectedStat = stat, Modifier = mod, StatChange = modifier.StatChanged  });
        }
        appliedEffects.Add(new EffectContainer { Instance = effectInstance, BaseModifiers = modifiersToApply.ToArray() });
        //stats.UpdateStats(appliedEffects);
    }

    public void ClearEffects()
    {
        appliedEffects.Clear();
    }

    void CleanEffects()
    {
        for (int i = 0; i < appliedEffects.Count; i++)
        {
            var effect = appliedEffects[i];
            if (effect.Instance.EffectSO.DurationPolicy == EDurationPolicy.Instant) { continue; }

            if (effect.Instance.DurationRemaining <= 0 && effect.Instance.EffectSO.DurationMultiplier != -1) 
            {
                if (appliedEffects[i].Instance.EffectSO.RevertOnRemove)
                {
                    foreach (var stat in appliedEffects[i].AccumulatedModifiers.Keys)
                    {
                        var e = appliedEffects[i].AccumulatedModifiers[stat];

                        stats.SubtractModifier(e.StatChange, stat, e.Modifier, true);
                        stats.Evaluate(e.StatChange, e.EffectedStat, true);
                    }
                }
                appliedEffects.Remove(effect);
            }
        }
    }

    protected virtual void Update()
    {
        TickEffects();
        CleanEffects();

    }

    void TickEffects()
    {
        for (var i = 0; i < this.appliedEffects.Count; i++)
        {
            var effect = this.appliedEffects[i].Instance;
            if (effect.EffectSO.DurationMultiplier != -1)
            {
                effect.UpdateRemainingDuration(Time.deltaTime);
            }

            if (effect.EffectSO.DurationPolicy == EDurationPolicy.Instant) continue;

            // Tick the periodic component
            effect.TickPeriodic(Time.deltaTime, out var executePeriodicTick);
            if (executePeriodicTick)
            {
                appliedEffects[i].AddAccumulatedModifiers();
                ApplyInstantEffect(effect);
            }
        }
    }

    private bool CheckTagRequirementsMet(EffectInstance effect)
    {
        return true;
    }
}
