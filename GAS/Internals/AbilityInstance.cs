using System;
using System.Collections;
using UnityEngine;

public struct AbilityCooldown
{
    public float Total;
    public float Remaining;

    public AbilityCooldown(float total, float remaining)
    {
        Total = total;
        Remaining = remaining;
    }
}


[Serializable]
public abstract class AbilityInstance
{
    public BaseAbilitySO Ability;
    protected AbilityCharacter caster;
    public bool isActive;

    public AbilityInstance(BaseAbilitySO ability, AbilityCharacter caster)
    {
        this.Ability = ability;
        this.caster = caster;
    }

    public virtual IEnumerator TryActivate()
    {
        Debug.Log($"Trying to activate {Ability.AbilityName}");   
        if (!CanActivateAbility()) yield break;

        isActive = true;
        yield return PreActivate();
        yield return ActivateAbility();
        EndAbility();
    }

    protected abstract IEnumerator PreActivate();
    protected abstract IEnumerator ActivateAbility();
    public virtual void EndAbility()
    {
        isActive = false;
    }

    public abstract void CancelAbility();

    public virtual bool CanActivateAbility()
    {
        return !isActive
        && CheckGameplayTags()
        && CheckCost()
        && CheckCooldown().Remaining <= 0;
    }

    public virtual AbilityCooldown CheckCooldown()
    {
        float maxDuration = 0;
        if (this.Ability.Cooldown == null) return new AbilityCooldown();
        var cooldownTags = this.Ability.Cooldown.EffectTags.GrantedTags;

        float longestCooldown = 0f;

        // Check if the cooldown tag is granted to the player, and if so, capture the remaining duration for that tag
        for (var i = 0; i < this.caster.AppliedEffects.Count; i++)
        {
            var grantedTags = this.caster.AppliedEffects[i].Instance.EffectSO.EffectTags.GrantedTags;
            for (var iTag = 0; iTag < grantedTags.Length; iTag++)
            {
                for (var iCooldownTag = 0; iCooldownTag < cooldownTags.Length; iCooldownTag++)
                {
                    if (grantedTags[iTag] == cooldownTags[iCooldownTag])
                    {
                        // If this is an infinite GE, then return null to signify this is on CD
                        if (this.caster.AppliedEffects[i].Instance.EffectSO.DurationMultiplier == -1) return new AbilityCooldown()
                        {
                            Remaining = float.MaxValue,
                            Total = 0
                        };

                        var durationRemaining = this.caster.AppliedEffects[i].Instance.DurationRemaining;

                        if (durationRemaining > longestCooldown)
                        {
                            longestCooldown = durationRemaining;
                            maxDuration = this.caster.AppliedEffects[i].Instance.TotalDuration;
                        }
                    }

                }
            }
        }

        return new AbilityCooldown()
        {
            Remaining = longestCooldown,
            Total = maxDuration
        };
    }

    public bool CheckCost()
    {
        return true;
    }

    public abstract bool CheckGameplayTags();

    protected virtual bool CasterHasAllTags(AbilityCharacter caster, TagSO[] tags)
    {
        // If the input ASC is not valid, assume check passed
        if (!caster) return true;

        for (var iAbilityTag = 0; iAbilityTag < tags.Length; iAbilityTag++)
        {
            var abilityTag = tags[iAbilityTag];

            bool requirementPassed = false;
            for (var i = 0; i < caster.AppliedEffects.Count; i++)
            {
                TagSO[] ascGrantedTags = caster.AppliedEffects[i].Instance.EffectSO.EffectTags.GrantedTags;
                for (var j = 0; j < ascGrantedTags.Length; j++)
                {
                    if (ascGrantedTags[j] == abilityTag)
                    {
                        requirementPassed = true;
                    }
                }
            }
            // If any ability tag wasn't found, requirements failed
            if (!requirementPassed) return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if an Ability System Character has none of the listed tags
    /// </summary>
    /// <param name="caster">Ability System Character</param>
    /// <param name="tags">List of tags to check</param>
    /// <returns>True, if the Ability System Character has none of the tags</returns>
    protected virtual bool AscHasNoneTags(AbilityCharacter caster, TagSO[] tags)
    {
        // If the input ASC is not valid, assume check passed
        if (!caster) return true;

        for (var i = 0; i < tags.Length; i++)
        {
            var abilityTag = tags[i];

            bool requirementPassed = true;
            for (var j = 0; j < caster.AppliedEffects.Count; j++)
            {
                TagSO[] ascGrantedTags = caster.AppliedEffects[j].Instance.EffectSO.EffectTags.GrantedTags;
                for (var k = 0; k < ascGrantedTags.Length; k++)
                {
                    if (ascGrantedTags[k] == abilityTag)
                    {
                        requirementPassed = false;
                    }
                }
            }
            // If any ability tag wasn't found, requirements failed
            if (!requirementPassed) return false;
        }
        return true;
    }
}