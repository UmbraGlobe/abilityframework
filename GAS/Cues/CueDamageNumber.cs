
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect/Cues/Damage Number")]
public class CueDamageNumber : BaseCueSO
{
    public override void ExecuteCue(EffectInstance effect)
    {
        if (effect.statChange > 0) NumberSystem.Instance.SpawnNumber(new NumberTextData($"{effect.statChange}", effect.Target.transform.position, NumberType.Damage));
    }
}