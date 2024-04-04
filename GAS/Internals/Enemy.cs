using System.Collections.Generic;
using UnityEngine;

public class Enemy : AbilityCharacter
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EventInt experienceDropped;
    [SerializeField] protected EventNumberTextData eventNumberTextData;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private LootBag lootBag;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D cc2d;
    [SerializeField] private EnemyDifficulty enemyDifficulty;

    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip[] death;

    private float lastAttackTime;
    private bool hasBeenHit = false;
    private HashSet<Collider2D> projectilesThatHitEnemy = new HashSet<Collider2D>();

    private Transform self;
    private Transform targetTransform;
    private AbilityCharacter target;

    private int monsterLevel = 1;

    public int GoldDropped => (int)(enemyData.GoldDropped * (1 + stats[Stat.GoldFind].Current / 100f));
    public int Experience => (int)(enemyData.Experience * (1 + stats[Stat.ExperienceFind].Current / 100f));
    public int LootBonus => (int)((int)enemyData.EnemyGrade * (1 + (stats[Stat.MagicFind].Current / 100f)));
    public EnemyGrade EnemyGrade => enemyData.EnemyGrade;
    public float GoldDropChance => enemyData.GoldDropChance;
    public int MonsterLevel => monsterLevel;

    protected override void StatCurrentChange(Stat stat, float value)
    {
        if (stat == Stat.Health)
        {
            TakeDamage();
        }
    }

    protected override void StatMaxChange(Stat stat, float value)
    {
    }

    public void SetEnemy(EnemyData _enemyData, Transform _player, PlayerStats _playerStats)
    {
        enemyData = _enemyData;
        Init(_player, _playerStats);
        ResetEnemy();
    }
    public void SetEnemyDifficulty(DifficultyBoost difficultyBoost)
    {
        stats.ResetAll();
        Stats.CombineAll(stats, enemyData.BaseStats, false);

        monsterLevel = 1 + enemyDifficulty.Difficulty * 7;
        stats.AddModifierValue(EStatChange.Max, Stat.Health, difficultyBoost.health, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.PhysicalDamage, difficultyBoost.damage, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.FireDamage, difficultyBoost.damage, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.IceDamage, difficultyBoost.damage, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.LightningDamage, difficultyBoost.damage, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.PoisonDamage, difficultyBoost.damage, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.ChaosDamage, difficultyBoost.damage, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.FireResistance, difficultyBoost.resistance, EModifierOperator.Add, false);
        stats.AddModifierValue(EStatChange.Max, Stat.IceResistance, difficultyBoost.resistance, EModifierOperator.Add, false);
        stats.AddModifierValue(EStatChange.Max, Stat.LightningResistance, difficultyBoost.resistance, EModifierOperator.Add, false);
        stats.AddModifierValue(EStatChange.Max, Stat.PoisonResistance, difficultyBoost.resistance, EModifierOperator.Add, false);
        stats.AddModifierValue(EStatChange.Max, Stat.ChaosResistance, difficultyBoost.resistance, EModifierOperator.Add, false);
        stats.AddModifierValue(EStatChange.Max, Stat.PhysicalResistance, difficultyBoost.resistance, EModifierOperator.Add, false);
        stats.AddModifierValue(EStatChange.Max, Stat.GoldFind, difficultyBoost.gold, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max, Stat.MagicFind, difficultyBoost.loot, EModifierOperator.Increase, false);
        stats.AddModifierValue(EStatChange.Max,  Stat.ExperienceFind, difficultyBoost.experience, EModifierOperator.Increase, false);
    }

    public void ResetEnemy()
    {
        StopAllCoroutines();
        sprite.color = Color.white;
        healthBar.SetMax();
        healthBar.HideHealthBar();
        isDead = false;
        hasBeenHit = false;
        projectilesThatHitEnemy.Clear();
        rb.velocity = Vector2.zero;
        stats.ResetAllCurrentToMax(evaluate: true, sendEvent: false);
    }

    private void Init(Transform player, PlayerStats playerStats)
    {
        SetEnemyDifficulty(enemyDifficulty.GetDifficultyBoost());

        self = transform;
        targetTransform = player;
        target = playerStats;

        healthBar.transform.position = spriteRenderer.bounds.center + new Vector3(0, spriteRenderer.bounds.extents.y * 2 + 1, 0);
        spriteRenderer.sprite = enemyData.EnemySprite;
        name = enemyData.EnemyName;
        cc2d.radius = (spriteRenderer.size.x + spriteRenderer.size.y) / 5f;

        lastAttackTime = -stats[Stat.AttackSpeed].Max;
        stats.ResetAllCurrentToMax(evaluate: false, sendEvent: false);
    }

    public override void TakeDamage()
    {
        //if (projectilesThatHitEnemy.Contains(attacker)) return 0; //Enable to make enemies invulnerable to the same projectile
        if (isDead) return;
        hasBeenHit = true;
        healthBar.ShowHealthBar();
        StartCoroutine(Flash());

        healthBar.UpdateHealth((int)stats[Stat.Health].Current, (int)stats[Stat.Health].Max);

        if (stats[Stat.Health].Current <= 0)
        {
            isDead = true;
            EnemySpawner.Instance.KillEnemy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        projectilesThatHitEnemy.Add(collision);
    }


    private void MoveIfNearPlayer()
    {
        if (Vector2.Distance(self.position, targetTransform.position) < enemyData.AggroRange || hasBeenHit)
        {
            hasBeenHit = true;
            rb.velocity = (target.transform.position - self.position).normalized * stats[Stat.MovementSpeed].Current;
        }
    }


    private bool TryAttack()
    {
        if (Vector2.Distance(self.position, targetTransform.position) < enemyData.AttackRange)
        {
            rb.velocity = Vector2.zero;
            if (Time.time - lastAttackTime >= stats[Stat.AttackSpeed].Current)
            {
                //target.TakeDamage(enemyDamage);
                lastAttackTime = Time.time; // Update the last attack time.
            }
            return true;
        }
        return false;
    }

    protected void FixedUpdate()
    {
        if (target == null) return;
        if (isDead) return;

        if (!TryAttack())
        {
            MoveIfNearPlayer();
        }
    }

    public override void Die()
    {
        AudioSystem.Instance?.PlaySound(death, 0.5f, SoundType.Hit);
        if (target == null) return;
        lootBag.DropLoot(this, target);
        lootBag.DropGold(this, target);
        experienceDropped.Raise(Experience);
        //MemorySaver.Instance?.Unregister(gameObject);   
    }

    /*public void OnLoad()
    {
        gameObject.SetActive(true);
    }

    public void OnUnload()
    {
        gameObject.SetActive(false);
    }

    public void Register()
    {
        //MemorySaver.Instance.Register(gameObject, this);
    }
    private void OnDestroy()
    {
        //MemorySaver.Instance?.Unregister(gameObject);
    }

    private void OnDisable()
    {
        //MemorySaver.Instance?.Unregister(gameObject);
    }*/
}
