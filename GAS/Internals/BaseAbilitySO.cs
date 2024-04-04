using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilitySO : ScriptableObject
{
    public string AbilityName;
    public EffectSO Cost;
    public EffectSO Cooldown;

    public abstract AbilityInstance CreateInstance(AbilityCharacter caster);
}
