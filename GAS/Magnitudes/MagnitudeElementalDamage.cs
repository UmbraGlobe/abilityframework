using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect/Modifier Magnitude/Elemental Damage")]
public class MagnitudeElementalDamage : BaseMagnitudeSO
{
    public override void Init(EffectInstance effect)
    {
    }
    public override float CalculateMagnitude(EffectInstance effect)
    {
        int damage = DamageCalculator.CalcDamageRecieved(effect.Caster, effect.Target);
        return damage;
    }
}

