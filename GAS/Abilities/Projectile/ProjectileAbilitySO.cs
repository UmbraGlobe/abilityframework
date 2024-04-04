using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Projectile Ability")]
public class ProjectileAbilitySO : BaseAbilitySO
{
    public EffectSO Effect;
    public Projectile ProjectilePrefab;
    public Sprite ProjectileSprite;
    public AudioClip AttackSound;
    public EffectSO[] StatusEffects;

    public override AbilityInstance CreateInstance(AbilityCharacter caster)
    {
        var instance = new ProjectileAbilityInstance(this, caster);
        return instance;
    }
    public class ProjectileAbilityInstance : AbilityInstance
    {
        private Projectile projectilePrefab => (Ability as ProjectileAbilitySO).ProjectilePrefab;
        private Sprite projectileSprite => (Ability as ProjectileAbilitySO).ProjectileSprite;
        private AudioClip attackSound => (Ability as ProjectileAbilitySO).AttackSound;
        private EffectSO effect => (Ability as ProjectileAbilitySO).Effect;
        private EffectSO[] statusEffects => (Ability as ProjectileAbilitySO).StatusEffects;

        public ProjectileAbilityInstance(BaseAbilitySO abilitySO, AbilityCharacter caster) : base(abilitySO, caster)
        {
        }

        public override void CancelAbility() { }
        protected override IEnumerator ActivateAbility()
        {
            // Apply cost and cooldown
            var cdEffect = this.caster.MakeOutgoingEffect(this.Ability.Cooldown);
            var costEffect = this.caster.MakeOutgoingEffect(this.Ability.Cost);
            this.caster.ApplyEffectInstanceToSelf(cdEffect);
            this.caster.ApplyEffectInstanceToSelf(costEffect);

            AttackRanged();


            //var effectSpec = this.caster.MakeOutgoingEffect((this.Ability as SimpleAbilityScriptableObject).GameplayEffect);
            //this.caster.ApplyEffectInstanceToSelf(effectSpec);

            yield return null;
        }

        private void CalcAngleAndSpawn(Vector2 direction)
        {
            float spreadAngle = (10f * ((int)caster.Stats[Stat.Projectiles].Current - 1) / 3f);
            float angleBetweenProjectiles = spreadAngle / ((int)caster.Stats[Stat.Projectiles].Current - 1);

            for (int i = 0; i < (int)caster.Stats[Stat.Projectiles].Current; i++)
            {
                Vector2 newDirection = Quaternion.Euler(0, 0, -spreadAngle / 2f + angleBetweenProjectiles * i) * direction;
                SpawnAndSet(newDirection);
            }
        }

        public void AttackRanged()
        {
            AudioSystem.Instance?.PlaySound(attackSound, 1f, SoundType.Hit);

            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)((worldMousePos - caster.transform.position));
            direction.Normalize();

            if ((int)caster.Stats[Stat.Projectiles].Current == 1)
            {
                SpawnAndSet(direction);
            }
            else if ((int)caster.Stats[Stat.Projectiles].Current > 1)
            {
                CalcAngleAndSpawn(direction);
            }
        }

        public List<DamageData> GenerateDamage()
        {
            List<DamageData> damageDatas = new List<DamageData>();
            foreach (var damageStat in StatParser.GetDamageTypes())
            {
                if (caster.Stats[damageStat].Current > 0)
                {
                    damageDatas.Add(new DamageData(StatParser.StatToDamage(damageStat), (int)caster.Stats[damageStat].Current));
                }
            }
            return damageDatas;
        }

        public EffectInstance[] GenerateStatusEffects()
        {
            EffectInstance[] effectInstances = new EffectInstance[statusEffects.Length];
            for (int i = 0; i < statusEffects.Length; i++)
            {
                effectInstances[i] = caster.MakeOutgoingEffect(statusEffects[i]);
            }
            return effectInstances;
        }

        private void SpawnAndSet(Vector2 direction)
        {
            Projectile projectile =
            Instantiate(projectilePrefab, caster.transform.position + (Vector3)(direction * 0.5f), Quaternion.LookRotation(Vector3.forward, direction));

            ProjectileData data = new ProjectileData
            (
                GenerateDamage(),
                caster.MakeOutgoingEffect(effect),
                GenerateStatusEffects(),
                (int)caster.Stats[Stat.Pierce].Current,
                (int)caster.Stats[Stat.Chain].Current,
                caster.Stats[Stat.AreaOfEffect].Current,
                30f / (int)caster.Stats[Stat.ProjectileSpeed].Current,
                caster.Stats[Stat.ProjectileSpeed].Current,
                projectileSprite,
                direction * caster.Stats[Stat.ProjectileSpeed].Current
            );

            projectile.SetProjectile(data);
        }
        public override bool CheckGameplayTags()
        {
            return true;
        }

        protected override IEnumerator PreActivate()
        {
            yield return null;
        }
    }
}