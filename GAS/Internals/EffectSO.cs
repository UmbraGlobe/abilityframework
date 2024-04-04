using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect Definition")]
public class EffectSO : ScriptableObject
{
    public bool RevertOnRemove;
    [SerializeField] public EDurationPolicy DurationPolicy;
    [HideIf("DurationPolicy", EDurationPolicy.Instant)] [SerializeField] public BaseMagnitudeSO DurationMagnitude;
    [HideIf("DurationPolicy", EDurationPolicy.Instant)] [SerializeField] public float DurationMultiplier;
    [ShowIf("DurationPolicy", EDurationPolicy.HasDuration)] [SerializeField] public float Period;
    [ShowIf("DurationPolicy", EDurationPolicy.HasDuration)] [SerializeField] public bool ExecuteOnApplication;

    [SerializeField] public EffectModifier[] EffectModifiers;
    //[SerializeField] public EffectModifier[] ConditionalModifiers;

    [SerializeField] public EffectTags EffectTags;
}
