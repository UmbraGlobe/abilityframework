using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect/Modifier Magnitude/Float")]
public class MagnitudeFloat : BaseMagnitudeSO
{
    [SerializeField]
    private float amount;

    public override void Init(EffectInstance instance)
    {
    }
    public override float CalculateMagnitude(EffectInstance instance)
    {
        return amount;
    }
}
