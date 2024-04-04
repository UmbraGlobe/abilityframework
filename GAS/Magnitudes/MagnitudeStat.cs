using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect/Modifier Magnitude/Stat")]
public class MagnitudeStat : BaseMagnitudeSO
{
    public enum ECaptureWho
    {
        Caster,
        Target
    }

    public enum ECaptureWhen
    {
        OnCreation,
        EveryApplication
    }

    [SerializeField] Stat stat;
    float value;
    [SerializeField] ECaptureWho captureWho;
    [SerializeField] ECaptureWhen captureWhen;

    public override void Init(EffectInstance instance)
    {
        Capture(instance);
    }
    public override float CalculateMagnitude(EffectInstance instance)
    {
        switch (captureWhen)
        {
            case ECaptureWhen.OnCreation:
                return value;
            case ECaptureWhen.EveryApplication:
                return Capture(instance);
            default:
                return 0;
        }
    }

    public float Capture(EffectInstance instance)
    {
        if (captureWho == ECaptureWho.Caster)
        {
            value = instance.Caster.Stats[stat].Current;
        }
        else
        {
            if (instance.Target == null) return 0;  
            value = instance.Target.Stats[stat].Current;
        }
        return value;
    }


}
