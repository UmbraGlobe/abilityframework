using UnityEngine;

public abstract class BaseMagnitudeSO : ScriptableObject
{
    public abstract void Init(EffectInstance instance);

    public abstract float CalculateMagnitude(EffectInstance instance);
}
