using NaughtyAttributes;
using System;
using UnityEngine;

[Serializable]
public struct EffectTags
{
    // The tag that defines this gameplay effect
    [SerializeField] public TagSO AssetTag;

    // The tags this GE grants to the ability system character
    [SerializeField] public TagSO[] GrantedTags;

    // These tags determine if the GE is considered 'on' or 'off'
    [SerializeField] public TagRequireIgnoreContainer OngoingTagRequirements;

    // These tags must be present for this GE to be applied
    [SerializeField] public TagRequireIgnoreContainer ApplicationTagRequirements;

    // Tag requirements that will remove this GE
    [SerializeField] public TagRequireIgnoreContainer RemovalTagRequirements;

    // Remove GE that match these tags
    [SerializeField] public TagSO[] RemoveGameplayEffectsWithTag;

}
[Serializable]
public struct AbilityTags
{
    // This tag describes the Gameplay Ability
    [SerializeField] public TagSO AssetTag;

    // Active Gameplay Abilities (on the same character) that have these tags will be cancelled
    [SerializeField] public TagSO[] CancelAbilitiesWithTags;

    // Gameplay Abilities that have these tags will be blocked from activating on the same character
    [SerializeField] public TagSO[] BlockAbilitiesWithTags;

    // These tags are granted to the character while the ability is active
    [SerializeField] public TagSO[] ActivationOwnedTags;

    // This ability can only be activated if the source character has all of the Required tags and none of the Ignore tags
    [SerializeField] public TagRequireIgnoreContainer CasterTags;

    // This ability can only be activated if the target character has all of the Required tags and none of the Ignore tags
    [SerializeField] public TagRequireIgnoreContainer TargetTags;
}


[Serializable]
public struct TagRequireIgnoreContainer
{
    public TagSO[] RequireTags;
    public TagSO[] IgnoreTags;
}

[Serializable]
public enum EDurationPolicy
{
    Instant,
    HasDuration
}



[Serializable]
public enum EModifierOperator
{
    Add, Increase, More, Raw, Override
}
