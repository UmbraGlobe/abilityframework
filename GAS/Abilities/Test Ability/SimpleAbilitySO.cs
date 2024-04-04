using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Simple Ability")]
public class SimpleAbilitySO : BaseAbilitySO
{
    /// <summary>
    /// Gameplay Effect to apply
    /// </summary>
    public EffectSO GameplayEffect;

    /// <summary>
    /// Creates the Ability Spec, which is instantiated for each character.
    /// </summary>
    /// <param name="caster"></param>
    /// <returns></returns>
    public override AbilityInstance CreateInstance(AbilityCharacter caster)
    {
        var spec = new SimpleAbilityInstance(this, caster);
        return spec;
    }

    /// <summary>
    /// The Ability Spec is the instantiation of the ability.  Since the Ability Spec
    /// is instantiated for each character, we can store stateful data here.
    /// </summary>
    public class SimpleAbilityInstance : AbilityInstance
    {
        public SimpleAbilityInstance(BaseAbilitySO abilitySO, AbilityCharacter caster) : base(abilitySO, caster)
        {
        }
        /// <summary>
        /// What to do when the ability is cancelled.  We don't care about there for this example.
        /// </summary>
        public override void CancelAbility() { }

        /// <summary>
        /// What happens when we activate the ability.
        /// 
        /// In this example, we apply the cost and cooldown, and then we apply the main
        /// gameplay effect
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator ActivateAbility()
        {
            // Apply cost and cooldown
            var cdEffect = this.caster.MakeOutgoingEffect(this.Ability.Cooldown);
            var costEffect = this.caster.MakeOutgoingEffect(this.Ability.Cost);
            this.caster.ApplyEffectInstanceToSelf(cdEffect);
            this.caster.ApplyEffectInstanceToSelf(costEffect);


            // Apply primary effect
            var effectSpec = this.caster.MakeOutgoingEffect((this.Ability as SimpleAbilitySO).GameplayEffect);
            this.caster.ApplyEffectInstanceToSelf(effectSpec);

            yield return null;
        }

        /// <summary>
        /// Checks to make sure Gameplay Tags checks are met. 
        /// 
        /// Since the target is also the character activating the ability,
        /// we can just use Owner for all of them.
        /// </summary>
        /// <returns></returns>
        public override bool CheckGameplayTags()
        {
            return true;
        }

        /// <summary>
        /// Logic to execute before activating the ability.  We don't need to do anything here
        /// for this example.
        /// </summary>
        /// <returns></returns>

        protected override IEnumerator PreActivate()
        {
            yield return null;
        }
    }
}