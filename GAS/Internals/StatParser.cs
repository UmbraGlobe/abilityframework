using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemStat
{
    FireDamageFlat,
    FireDamageIncreased,
    FireDamageMore,

    IceDamageFlat,
    IceDamageIncreased,
    IceDamageMore,

    LightningDamageFlat,
    LightningDamageIncreased,
    LightningDamageMore,

    PoisonDamageFlat,
    PoisonDamageIncreased,
    PoisonDamageMore,

    ChaosDamageFlat,
    ChaosDamageIncreased,
    ChaosDamageMore,

    PhysicalDamageFlat,
    PhysicalDamageIncreased,
    PhysicalDamageMore,

    HealthFlat,
    HealthIncreased,
    HealthMore,

    ManaFlat,
    ManaIncreased,
    ManaMore,

    ArmorFlat,
    ArmorIncreased,
    ArmorMore,

    EnergyShieldFlat,
    EnergyShieldIncreased,
    EnergyShieldMore,

    MovementSpeedIncreased,

    FireResistanceFlat,
    IceResistanceFlat,
    LightningResistanceFlat,
    PoisonResistanceFlat,
    ChaosResistanceFlat,
    PhysicalResistanceFlat,

    AttackSpeedIncreased,

    CritChanceIncreased,

    CritDamageIncreased,

    HealthLeechFlat,
    ManaLeechFlat,

    AreaOfEffectIncreased,
    AreaOfEffectMore,

    ProjectileSpeedIncreased,
    ProjectileSpeedMore,
    ProjectilesFlat,
    PierceFlat,
    ChainFlat,

    MagicFindFlat,
    ExperienceFindFlat,
    GoldFindFlat,

    KnockbackIncreased,
    DodgeFlat,

    HealthRegenerationFlat,
    HealthRegenerationIncreased,

    ManaRegenerationFlat,
    ManaRegenerationIncreased,

    EnergyShieldRegenerationFlat,
    EnergyShieldRegenerationIncreased,

    ReflectFlat,

    CooldownReductionFlat,
}
[System.Serializable]
public struct Tier
{
    public int tierNum;
    public int weight;
    public int minLevel;
    public float maxRoll;
    public float minRoll;

    public Tier(int _tier, int _weight, int _minLevel, float _minRoll, float _maxRoll)
    {
        tierNum = _tier;
        weight = _weight;
        minLevel = _minLevel;
        minRoll = _minRoll;
        maxRoll = _maxRoll;
    }
    public float Roll()
    {
        return UnityEngine.Random.Range(Mathf.FloorToInt(minRoll), Mathf.FloorToInt(maxRoll) + 1);
    }
}

public static class StatParser
{
    public static readonly Dictionary<DamageType, Stat> damageTypeToDamageStat = new Dictionary<DamageType, Stat>()
    {
        { DamageType.FireDamage, Stat.FireDamage },
        { DamageType.IceDamage, Stat.IceDamage },
        { DamageType.LightningDamage, Stat.LightningDamage },
        { DamageType.PoisonDamage, Stat.PoisonDamage },
        { DamageType.ChaosDamage, Stat.ChaosDamage },
        { DamageType.PhysicalDamage, Stat.PhysicalDamage },
    };

    public static readonly Dictionary<DamageType, Stat> damageTypeToResistanceStat = new Dictionary<DamageType, Stat>()
    {
        { DamageType.FireDamage, Stat.FireResistance },
        { DamageType.IceDamage, Stat.IceResistance },
        { DamageType.LightningDamage, Stat.LightningResistance },
        { DamageType.PoisonDamage, Stat.PoisonResistance },
        { DamageType.ChaosDamage, Stat.ChaosResistance },
        { DamageType.PhysicalDamage, Stat.PhysicalResistance },
    };

    public static readonly Dictionary<Stat, DamageType> statToDamageType = new Dictionary<Stat, DamageType>()
    {
        { Stat.FireDamage, DamageType.FireDamage },
        { Stat.IceDamage, DamageType.IceDamage },
        { Stat.LightningDamage, DamageType.LightningDamage },
        { Stat.PoisonDamage, DamageType.PoisonDamage },
        { Stat.ChaosDamage, DamageType.ChaosDamage },
        { Stat.PhysicalDamage, DamageType.PhysicalDamage },
    };

    public static readonly Dictionary<Stat, ItemStat[]> statToItemStat = new Dictionary<Stat, ItemStat[]>
    {
        {Stat.FireDamage, new ItemStat[] { ItemStat.FireDamageFlat, ItemStat.FireDamageIncreased, ItemStat.FireDamageMore } },
        {Stat.IceDamage, new ItemStat[] { ItemStat.IceDamageFlat, ItemStat.IceDamageIncreased, ItemStat.IceDamageMore } },
        {Stat.LightningDamage, new ItemStat[] { ItemStat.LightningDamageFlat, ItemStat.LightningDamageIncreased, ItemStat.LightningDamageMore } },
        {Stat.PoisonDamage, new ItemStat[] { ItemStat.PoisonDamageFlat, ItemStat.PoisonDamageIncreased, ItemStat.PoisonDamageMore } },
        {Stat.ChaosDamage, new ItemStat[] { ItemStat.ChaosDamageFlat, ItemStat.ChaosDamageIncreased, ItemStat.ChaosDamageMore } },
        {Stat.PhysicalDamage, new ItemStat[] { ItemStat.PhysicalDamageFlat, ItemStat.PhysicalDamageIncreased, ItemStat.PhysicalDamageMore } },
        {Stat.Health, new ItemStat[] { ItemStat.HealthFlat, ItemStat.HealthIncreased, ItemStat.HealthMore } },
        {Stat.Mana, new ItemStat[] { ItemStat.ManaFlat, ItemStat.ManaIncreased, ItemStat.ManaMore } },
        {Stat.Armor, new ItemStat[] { ItemStat.ArmorFlat, ItemStat.ArmorIncreased, ItemStat.ArmorMore } },
        {Stat.EnergyShield, new ItemStat[] { ItemStat.EnergyShieldFlat, ItemStat.EnergyShieldIncreased, ItemStat.EnergyShieldMore } },
        {Stat.MovementSpeed, new ItemStat[] { ItemStat.MovementSpeedIncreased } },
        {Stat.FireResistance, new ItemStat[] { ItemStat.FireResistanceFlat } },
        {Stat.IceResistance, new ItemStat[] { ItemStat.IceResistanceFlat } },
        {Stat.LightningResistance, new ItemStat[] { ItemStat.LightningResistanceFlat } },
        {Stat.PoisonResistance, new ItemStat[] { ItemStat.PoisonResistanceFlat } },
        {Stat.ChaosResistance, new ItemStat[] { ItemStat.ChaosResistanceFlat } },
        {Stat.PhysicalResistance, new ItemStat[] { ItemStat.PhysicalResistanceFlat } },
        {Stat.AttackSpeed, new ItemStat[] { ItemStat.AttackSpeedIncreased } },
        {Stat.CritChance, new ItemStat[] { ItemStat.CritChanceIncreased } },
        {Stat.CritDamage, new ItemStat[] { ItemStat.CritDamageIncreased } },
        {Stat.HealthLeech, new ItemStat[] { ItemStat.HealthLeechFlat } },
        {Stat.ManaLeech, new ItemStat[] { ItemStat.ManaLeechFlat } },
        {Stat.AreaOfEffect, new ItemStat[] { ItemStat.AreaOfEffectIncreased, ItemStat.AreaOfEffectMore } },
        {Stat.ProjectileSpeed, new ItemStat[] { ItemStat.ProjectileSpeedIncreased, ItemStat.ProjectileSpeedMore } },
        {Stat.Projectiles, new ItemStat[] { ItemStat.ProjectilesFlat } },
        {Stat.Pierce, new ItemStat[] { ItemStat.PierceFlat } },
        {Stat.Chain, new ItemStat[] { ItemStat.ChainFlat } },
        {Stat.MagicFind, new ItemStat[] { ItemStat.MagicFindFlat } },
        {Stat.ExperienceFind, new ItemStat[] { ItemStat.ExperienceFindFlat } },
        {Stat.GoldFind, new ItemStat[] { ItemStat.GoldFindFlat } },
        {Stat.Knockback, new ItemStat[] { ItemStat.KnockbackIncreased } },
        {Stat.Dodge, new ItemStat[] { ItemStat.DodgeFlat } },
        {Stat.HealthRegeneration, new ItemStat[] { ItemStat.HealthRegenerationFlat, ItemStat.HealthRegenerationIncreased } },
        {Stat.ManaRegeneration, new ItemStat[] { ItemStat.ManaRegenerationFlat, ItemStat.ManaRegenerationIncreased } },
        {Stat.EnergyShieldRegeneration, new ItemStat[] { ItemStat.EnergyShieldRegenerationFlat, ItemStat.EnergyShieldRegenerationIncreased } },
        {Stat.Reflect, new ItemStat[] { ItemStat.ReflectFlat } },
        {Stat.CooldownReduction, new ItemStat[] { ItemStat.CooldownReductionFlat } },
    };

    public static readonly Dictionary<ItemStat, EModifierOperator> statTypeDict = new Dictionary<ItemStat, EModifierOperator>
    {
        {ItemStat.FireDamageFlat, EModifierOperator.Add},
        {ItemStat.FireDamageIncreased, EModifierOperator.Increase},
        {ItemStat.FireDamageMore, EModifierOperator.More},
        {ItemStat.IceDamageFlat, EModifierOperator.Add},
        {ItemStat.IceDamageIncreased, EModifierOperator.Increase},
        {ItemStat.IceDamageMore, EModifierOperator.More},
        {ItemStat.LightningDamageFlat, EModifierOperator.Add},
        {ItemStat.LightningDamageIncreased, EModifierOperator.Increase},
        {ItemStat.LightningDamageMore, EModifierOperator.More},
        {ItemStat.PoisonDamageFlat, EModifierOperator.Add},
        {ItemStat.PoisonDamageIncreased, EModifierOperator.Increase},
        {ItemStat.PoisonDamageMore, EModifierOperator.More},
        {ItemStat.ChaosDamageFlat, EModifierOperator.Add},
        {ItemStat.ChaosDamageIncreased, EModifierOperator.Increase},
        {ItemStat.ChaosDamageMore, EModifierOperator.More},
        {ItemStat.PhysicalDamageFlat, EModifierOperator.Add},
        {ItemStat.PhysicalDamageIncreased, EModifierOperator.Increase},
        {ItemStat.PhysicalDamageMore, EModifierOperator.More},
        {ItemStat.HealthFlat, EModifierOperator.Add},
        {ItemStat.HealthIncreased, EModifierOperator.Increase},
        {ItemStat.HealthMore, EModifierOperator.More},
        {ItemStat.ManaFlat, EModifierOperator.Add},
        {ItemStat.ManaIncreased, EModifierOperator.Increase},
        {ItemStat.ManaMore, EModifierOperator.More},
        {ItemStat.ArmorFlat, EModifierOperator.Add},
        {ItemStat.ArmorIncreased, EModifierOperator.Increase},
        {ItemStat.ArmorMore, EModifierOperator.More},
        {ItemStat.EnergyShieldFlat, EModifierOperator.Add},
        {ItemStat.EnergyShieldIncreased, EModifierOperator.Increase},
        {ItemStat.EnergyShieldMore, EModifierOperator.More},
        {ItemStat.MovementSpeedIncreased, EModifierOperator.Increase},
        {ItemStat.FireResistanceFlat, EModifierOperator.Add},
        {ItemStat.IceResistanceFlat, EModifierOperator.Add},
        {ItemStat.LightningResistanceFlat, EModifierOperator.Add},
        {ItemStat.PoisonResistanceFlat, EModifierOperator.Add},
        {ItemStat.ChaosResistanceFlat, EModifierOperator.Add},
        {ItemStat.PhysicalResistanceFlat, EModifierOperator.Add},
        {ItemStat.AttackSpeedIncreased, EModifierOperator.Increase},
        {ItemStat.CritChanceIncreased, EModifierOperator.Increase},
        {ItemStat.CritDamageIncreased, EModifierOperator.Increase},
        {ItemStat.HealthLeechFlat, EModifierOperator.Add},
        {ItemStat.ManaLeechFlat, EModifierOperator.Add},
        {ItemStat.AreaOfEffectIncreased, EModifierOperator.Increase},
        {ItemStat.AreaOfEffectMore, EModifierOperator.More},
        {ItemStat.ProjectileSpeedIncreased, EModifierOperator.Increase},
        {ItemStat.ProjectileSpeedMore, EModifierOperator.More},
        {ItemStat.ProjectilesFlat, EModifierOperator.Add},
        {ItemStat.PierceFlat, EModifierOperator.Add},
        {ItemStat.ChainFlat, EModifierOperator.Add},
        {ItemStat.MagicFindFlat, EModifierOperator.Add},
        {ItemStat.ExperienceFindFlat, EModifierOperator.Add},
        {ItemStat.GoldFindFlat, EModifierOperator.Add},
        {ItemStat.KnockbackIncreased, EModifierOperator.Increase},
        {ItemStat.DodgeFlat, EModifierOperator.Add},
        {ItemStat.HealthRegenerationFlat, EModifierOperator.Add},
        {ItemStat.HealthRegenerationIncreased, EModifierOperator.Increase},
        {ItemStat.ManaRegenerationFlat, EModifierOperator.Add},
        {ItemStat.ManaRegenerationIncreased, EModifierOperator.Increase},
        {ItemStat.EnergyShieldRegenerationFlat, EModifierOperator.Add},
        {ItemStat.EnergyShieldRegenerationIncreased, EModifierOperator.Increase},
        {ItemStat.ReflectFlat, EModifierOperator.Add},
        {ItemStat.CooldownReductionFlat, EModifierOperator.Add},
    };

    private static readonly ItemStat[] baseArmorStats = new ItemStat[] {
        ItemStat.FireDamageFlat,
        ItemStat.FireDamageIncreased,
        //ItemStat.FireDamageMore,

        ItemStat.IceDamageFlat,
        ItemStat.IceDamageIncreased,
        //ItemStat.IceDamageMore,

        ItemStat.LightningDamageFlat,
        ItemStat.LightningDamageIncreased,
        //ItemStat.LightningDamageMore,

        ItemStat.PoisonDamageFlat,
        ItemStat.PoisonDamageIncreased,
        //ItemStat.PoisonDamageMore,

        ItemStat.ChaosDamageFlat,
        ItemStat.ChaosDamageIncreased,
        //ItemStat.ChaosDamageMore,

        ItemStat.PhysicalDamageFlat,
        ItemStat.PhysicalDamageIncreased,
        //ItemStat.PhysicalDamageMore,

        ItemStat.HealthFlat,
        ItemStat.HealthIncreased,
        //ItemStat.HealthMore,

        ItemStat.ManaFlat,
        ItemStat.ManaIncreased,
        //ItemStat.ManaMore,

        ItemStat.ArmorFlat,
        ItemStat.ArmorIncreased,
        //ItemStat.ArmorMore,

        ItemStat.EnergyShieldFlat,
        ItemStat.EnergyShieldIncreased,
        //ItemStat.EnergyShieldMore,

        //ItemStat.MovementSpeedIncreased,

        ItemStat.FireResistanceFlat,
        ItemStat.IceResistanceFlat,
        ItemStat.LightningResistanceFlat,
        ItemStat.PoisonResistanceFlat,
        ItemStat.ChaosResistanceFlat,
        ItemStat.PhysicalResistanceFlat,

        ItemStat.AttackSpeedIncreased,

        //ItemStat.CritChanceIncreased,

        //ItemStat.CritDamageIncreased,

        //ItemStat.HealthLeechFlat,
        //ItemStat.ManaLeechFlat,

        ItemStat.AreaOfEffectIncreased,
        //ItemStat.AreaOfEffectMore,

        //ItemStat.ProjectileSpeedIncreased,
        //ItemStat.ProjectileSpeedMore,
        //ItemStat.ProjectilesFlat,
        //ItemStat.PierceFlat,
        //ItemStat.ChainFlat,

        ItemStat.MagicFindFlat,
        ItemStat.ExperienceFindFlat,
        ItemStat.GoldFindFlat,

        //ItemStat.KnockbackIncreased,
        //ItemStat.DodgeFlat,

        ItemStat.HealthRegenerationFlat,
        ItemStat.HealthRegenerationIncreased,

        ItemStat.ManaRegenerationFlat,
        ItemStat.ManaRegenerationIncreased,

        ItemStat.EnergyShieldRegenerationFlat,
        ItemStat.EnergyShieldRegenerationIncreased,

        ItemStat.ReflectFlat,

        ItemStat.CooldownReductionFlat,
    };
    private static readonly ItemStat[] baseWeaponStats = new ItemStat[]
    {
        ItemStat.FireDamageFlat,
        ItemStat.FireDamageIncreased,
        //ItemStat.FireDamageMore,

        ItemStat.IceDamageFlat,
        ItemStat.IceDamageIncreased,
        //ItemStat.IceDamageMore,

        ItemStat.LightningDamageFlat,
        ItemStat.LightningDamageIncreased,
        //ItemStat.LightningDamageMore,

        ItemStat.PoisonDamageFlat,
        ItemStat.PoisonDamageIncreased,
        //ItemStat.PoisonDamageMore,

        ItemStat.ChaosDamageFlat,
        ItemStat.ChaosDamageIncreased,
        //ItemStat.ChaosDamageMore,

        ItemStat.PhysicalDamageFlat,
        ItemStat.PhysicalDamageIncreased,
        //ItemStat.PhysicalDamageMore,

        //ItemStat.HealthFlat,
        //ItemStat.HealthIncreased,
        //ItemStat.HealthMore,

        ItemStat.ManaFlat,
        ItemStat.ManaIncreased,
        //ItemStat.ManaMore,

        //ItemStat.ArmorFlat,
        //ItemStat.ArmorIncreased,
        //ItemStat.ArmorMore,

        //ItemStat.EnergyShieldFlat,
        //ItemStat.EnergyShieldIncreased,
        //ItemStat.EnergyShieldMore,

        //ItemStat.MovementSpeedIncreased,

        //ItemStat.FireResistanceFlat,
        //ItemStat.IceResistanceFlat,
        //ItemStat.LightningResistanceFlat,
        //ItemStat.PoisonResistanceFlat,
        //ItemStat.ChaosResistanceFlat,
        //ItemStat.PhysicalResistanceFlat,

        ItemStat.AttackSpeedIncreased,

        ItemStat.CritChanceIncreased,

        ItemStat.CritDamageIncreased,

        ItemStat.HealthLeechFlat,
        ItemStat.ManaLeechFlat,

        ItemStat.AreaOfEffectIncreased,
        ItemStat.AreaOfEffectMore,

        //ItemStat.ProjectileSpeedIncreased,
        //ItemStat.ProjectileSpeedMore,
        //ItemStat.ProjectilesFlat,
        //ItemStat.PierceFlat,
        //ItemStat.ChainFlat,

        //ItemStat.MagicFindFlat,
        //ItemStat.ExperienceFindFlat,
        //ItemStat.GoldFindFlat,

        //ItemStat.KnockbackIncreased,
        //ItemStat.DodgeFlat,

        //ItemStat.HealthRegenerationFlat,
        //ItemStat.HealthRegenerationIncreased,

        //ItemStat.ManaRegenerationFlat,
        //ItemStat.ManaRegenerationIncreased,

        //ItemStat.EnergyShieldRegenerationFlat,
        //ItemStat.EnergyShieldRegenerationIncreased,

        //ItemStat.ReflectFlat,

        ItemStat.CooldownReductionFlat,
    };

    private static readonly ItemStat[] amuletStats = baseArmorStats;
    private static readonly ItemStat[] helmetStats = baseArmorStats;
    private static readonly ItemStat[] charmStats = baseArmorStats;
    private static readonly ItemStat[] meleeStats = baseWeaponStats;
    private static readonly ItemStat[] rangedStats = baseWeaponStats.Concat(new ItemStat[] { ItemStat.ProjectilesFlat, ItemStat.ProjectileSpeedIncreased, ItemStat.PierceFlat, ItemStat.ChainFlat }).ToArray();
    private static readonly ItemStat[] chestplateStats = baseArmorStats;
    private static readonly ItemStat[] offhandStats = baseArmorStats;
    private static readonly ItemStat[] glovesStats = baseArmorStats;
    private static readonly ItemStat[] leggingsStats = baseArmorStats;
    private static readonly ItemStat[] ringStats = baseArmorStats;
    private static readonly ItemStat[] bootStats = baseArmorStats.Concat(new ItemStat[] { ItemStat.MovementSpeedIncreased, ItemStat.DodgeFlat, }).ToArray();

    private static readonly Dictionary<EquipmentType, ItemStat[]> statDict = new Dictionary<EquipmentType, ItemStat[]>()
    {
        { EquipmentType.Amulet, amuletStats },
        { EquipmentType.Helmet, helmetStats },
        { EquipmentType.Charm, charmStats },
        { EquipmentType.Weapon, meleeStats },
        { EquipmentType.Chestplate, chestplateStats },
        { EquipmentType.Offhand, offhandStats },
        { EquipmentType.Gloves, glovesStats },
        { EquipmentType.Leggings, leggingsStats },
        { EquipmentType.Ring, ringStats },
        { EquipmentType.Boots, bootStats },
    };

    private static readonly Dictionary<Stat, string> statString = new Dictionary<Stat, string>()
    {
        { Stat.FireDamage, "Fire Damage" },
        { Stat.IceDamage, "Ice Damage" },
        { Stat.LightningDamage, "Lightning Damage" },
        { Stat.PoisonDamage, "Poison Damage" },
        { Stat.ChaosDamage, "Chaos Damage" },
        { Stat.PhysicalDamage, "Physical Damage" },
        { Stat.Health, "Health" },
        { Stat.Mana, "Mana" },
        { Stat.Armor, "Armor" },
        { Stat.MovementSpeed, "Movement Speed" },
        { Stat.FireResistance, "Fire Resistance" },
        { Stat.IceResistance, "Ice Resistance" },
        { Stat.LightningResistance, "Lightning Resistance" },
        { Stat.PoisonResistance, "Poison Resistance" },
        { Stat.ChaosResistance, "Chaos Resistance" },
        { Stat.PhysicalResistance, "Physical Resistance" },
        { Stat.AttackSpeed, "Attack Speed" },
        { Stat.CritChance, "Crit Chance" },
        { Stat.CritDamage, "Crit Damage" },
        { Stat.HealthLeech, "Leech" },
        { Stat.AreaOfEffect, "Area of Effect" },
        { Stat.ProjectileSpeed, "Projectile Speed" },
        { Stat.Projectiles, "Projectiles" },
        { Stat.Pierce, "Pierce" },
        { Stat.Chain, "Chain" },
        { Stat.MagicFind, "Magic Find" },
        { Stat.Knockback, "Knockback" },
        { Stat.Dodge, "Dodge" },
        { Stat.HealthRegeneration, "Health Regeneration" },
        { Stat.ManaRegeneration, "Mana Regeneration" },
        { Stat.EnergyShieldRegeneration, "Energy Shield Regeneration" },
        { Stat.EnergyShield, "Energy Shield" },
        { Stat.Reflect, "Reflect" },
        { Stat.ManaLeech, "Mana Leech" },
        { Stat.CooldownReduction, "Cooldown Reduction" },
        { Stat.ExperienceFind, "Experience Find" },
        { Stat.GoldFind, "Gold Find" },
    };

    private static readonly Dictionary<ItemStat, string> itemStatString = new Dictionary<ItemStat, string>()
    {
        {ItemStat.FireDamageFlat, "Fire Damage"},
        {ItemStat.FireDamageIncreased, "Increased Fire Damage"},
        {ItemStat.FireDamageMore, "More Fire Damage"},
        {ItemStat.IceDamageFlat, "Ice Damage"},
        {ItemStat.IceDamageIncreased, "Increased Ice Damage"},
        {ItemStat.IceDamageMore, "More Ice Damage"},
        {ItemStat.LightningDamageFlat, "Lightning Damage"},
        {ItemStat.LightningDamageIncreased, "Increased Lightning Damage"},
        {ItemStat.LightningDamageMore, "More Lightning Damage"},
        {ItemStat.PoisonDamageFlat, "Poison Damage"},
        {ItemStat.PoisonDamageIncreased, "Increased Poison Damage"},
        {ItemStat.PoisonDamageMore, "More Poison Damage"},
        {ItemStat.ChaosDamageFlat, "Chaos Damage"},
        {ItemStat.ChaosDamageIncreased, "Increased Chaos Damage"},
        {ItemStat.ChaosDamageMore, "More Chaos Damage"},
        {ItemStat.PhysicalDamageFlat, "Physical Damage"},
        {ItemStat.PhysicalDamageIncreased, "Increased Physical Damage"},
        {ItemStat.PhysicalDamageMore, "More Physical Damage"},
        {ItemStat.HealthFlat, "Health"},
        {ItemStat.HealthIncreased, "Increased Health"},
        {ItemStat.HealthMore, "More Health"},
        {ItemStat.ManaFlat, "Mana"},
        {ItemStat.ManaIncreased, "Increased Mana"},
        {ItemStat.ManaMore, "More Mana"},
        {ItemStat.ArmorFlat, "Armor"},
        {ItemStat.ArmorIncreased, "Increased Armor"},
        {ItemStat.ArmorMore, "More Armor"},
        {ItemStat.EnergyShieldFlat, "Energy Shield"},
        {ItemStat.EnergyShieldIncreased, "Increased Energy Shield"},
        {ItemStat.EnergyShieldMore, "More Energy Shield"},
        {ItemStat.MovementSpeedIncreased, "Increased Movement Speed"},
        {ItemStat.FireResistanceFlat, "Fire Resistance"},
        {ItemStat.IceResistanceFlat, "Ice Resistance"},
        {ItemStat.LightningResistanceFlat, "Lightning Resistance"},
        {ItemStat.PoisonResistanceFlat, "Poison Resistance"},
        {ItemStat.ChaosResistanceFlat, "Chaos Resistance"},
        {ItemStat.PhysicalResistanceFlat, "Physical Resistance"},
        {ItemStat.AttackSpeedIncreased, "Increased Attack Speed"},
        {ItemStat.CritChanceIncreased, "Increased Crit Chance"},
        {ItemStat.CritDamageIncreased, "Increased Crit Damage"},
        {ItemStat.HealthLeechFlat, "Health Leech"},
        {ItemStat.ManaLeechFlat, "Mana Leech"},
        {ItemStat.AreaOfEffectIncreased, "Increased Area of Effect"},
        {ItemStat.AreaOfEffectMore, "More Area of Effect"},
        {ItemStat.ProjectileSpeedIncreased, "Increased Projectile Speed"},
        {ItemStat.ProjectileSpeedMore, "More Projectile Speed"},
        {ItemStat.ProjectilesFlat, "Projectiles"},
        {ItemStat.PierceFlat, "Pierce"},
        {ItemStat.ChainFlat, "Chain"},
        {ItemStat.MagicFindFlat, "Magic Find"},
        {ItemStat.ExperienceFindFlat, "Experience Find"},
        {ItemStat.GoldFindFlat, "Gold Find"},
        {ItemStat.KnockbackIncreased, "Increased Knockback"},
        {ItemStat.DodgeFlat, "Dodge"},
        {ItemStat.HealthRegenerationFlat, "Health Regeneration"},
        {ItemStat.HealthRegenerationIncreased, "Increased Health Regeneration"},
        {ItemStat.ManaRegenerationFlat, "Mana Regeneration"},
        {ItemStat.ManaRegenerationIncreased, "Increased Mana Regeneration"},
        {ItemStat.EnergyShieldRegenerationFlat, "Energy Shield Regeneration"},
        {ItemStat.EnergyShieldRegenerationIncreased, "Increased Energy Shield Regeneration"},
        {ItemStat.ReflectFlat, "Reflect"},
        {ItemStat.CooldownReductionFlat, "Cooldown Reduction"},

    };

    private static readonly Dictionary<ItemStat, List<Tier>> statTiers = new Dictionary<ItemStat, List<Tier>>()
    {
        { ItemStat.FireDamageFlat, new List<Tier>() {
            new Tier(0, 1, 80, 37, 40),
            new Tier(1, 5, 70, 33, 36),
            new Tier(2, 10, 60, 29, 32),
            new Tier(3, 15, 50, 25, 28),
            new Tier(4, 20, 40, 21, 24),
            new Tier(5, 25, 30, 17, 20),
            new Tier(6, 30, 20, 13, 16),
            new Tier(7, 35, 10, 9, 12),
            new Tier(8, 40, 0, 5, 8),
            new Tier(9, 100, 0, 1, 4) } },
        { ItemStat.FireDamageIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 90, 100),
            new Tier(1, 5, 70, 80, 89),
            new Tier(2, 10, 60, 70, 79),
            new Tier(3, 15, 50, 60, 69),
            new Tier(4, 20, 40, 50, 59),
            new Tier(5, 25, 30, 40, 49),
            new Tier(6, 30, 20, 30, 39),
            new Tier(7, 35, 10, 20, 29),
            new Tier(8, 40, 0, 10, 19),
            new Tier(9, 100, 0, 1, 9) } },
        { ItemStat.FireDamageMore, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.IceDamageFlat, new List<Tier>() {
            new Tier(0, 1, 80, 37, 40),
            new Tier(1, 5, 70, 33, 36),
            new Tier(2, 10, 60, 29, 32),
            new Tier(3, 15, 50, 25, 28),
            new Tier(4, 20, 40, 21, 24),
            new Tier(5, 25, 30, 17, 20),
            new Tier(6, 30, 20, 13, 16),
            new Tier(7, 35, 10, 9, 12),
            new Tier(8, 40, 0, 5, 8),
            new Tier(9, 100, 0, 1, 4) } },
        { ItemStat.IceDamageIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 90, 100),
            new Tier(1, 5, 70, 80, 89),
            new Tier(2, 10, 60, 70, 79),
            new Tier(3, 15, 50, 60, 69),
            new Tier(4, 20, 40, 50, 59),
            new Tier(5, 25, 30, 40, 49),
            new Tier(6, 30, 20, 30, 39),
            new Tier(7, 35, 10, 20, 29),
            new Tier(8, 40, 0, 10, 19),
            new Tier(9, 100, 0, 1, 9) } },
        { ItemStat.IceDamageMore, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.LightningDamageFlat, new List<Tier>() {
            new Tier(0, 1, 80, 37, 40),
            new Tier(1, 5, 70, 33, 36),
            new Tier(2, 10, 60, 29, 32),
            new Tier(3, 15, 50, 25, 28),
            new Tier(4, 20, 40, 21, 24),
            new Tier(5, 25, 30, 17, 20),
            new Tier(6, 30, 20, 13, 16),
            new Tier(7, 35, 10, 9, 12),
            new Tier(8, 40, 0, 5, 8),
            new Tier(9, 100, 0, 1, 4) } },
        { ItemStat.LightningDamageIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 90, 100),
            new Tier(1, 5, 70, 80, 89),
            new Tier(2, 10, 60, 70, 79),
            new Tier(3, 15, 50, 60, 69),
            new Tier(4, 20, 40, 50, 59),
            new Tier(5, 25, 30, 40, 49),
            new Tier(6, 30, 20, 30, 39),
            new Tier(7, 35, 10, 20, 29),
            new Tier(8, 40, 0, 10, 19),
            new Tier(9, 100, 0, 1, 9) } },
        { ItemStat.LightningDamageMore, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.PoisonDamageFlat, new List<Tier>() {
            new Tier(0, 1, 80, 37, 40),
            new Tier(1, 5, 70, 33, 36),
            new Tier(2, 10, 60, 29, 32),
            new Tier(3, 15, 50, 25, 28),
            new Tier(4, 20, 40, 21, 24),
            new Tier(5, 25, 30, 17, 20),
            new Tier(6, 30, 20, 13, 16),
            new Tier(7, 35, 10, 9, 12),
            new Tier(8, 40, 0, 5, 8),
            new Tier(9, 100, 0, 1, 4) } },
        { ItemStat.PoisonDamageIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 90, 100),
            new Tier(1, 5, 70, 80, 89),
            new Tier(2, 10, 60, 70, 79),
            new Tier(3, 15, 50, 60, 69),
            new Tier(4, 20, 40, 50, 59),
            new Tier(5, 25, 30, 40, 49),
            new Tier(6, 30, 20, 30, 39),
            new Tier(7, 35, 10, 20, 29),
            new Tier(8, 40, 0, 10, 19),
            new Tier(9, 100, 0, 1, 9) } },
        { ItemStat.PoisonDamageMore, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.ChaosDamageFlat, new List<Tier>() {
            new Tier(0, 1, 80, 37, 40),
            new Tier(1, 5, 70, 33, 36),
            new Tier(2, 10, 60, 29, 32),
            new Tier(3, 15, 50, 25, 28),
            new Tier(4, 20, 40, 21, 24),
            new Tier(5, 25, 30, 17, 20),
            new Tier(6, 30, 20, 13, 16),
            new Tier(7, 35, 10, 9, 12),
            new Tier(8, 40, 0, 5, 8),
            new Tier(9, 100, 0, 1, 4) } },
        { ItemStat.ChaosDamageIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 90, 100),
            new Tier(1, 5, 70, 80, 89),
            new Tier(2, 10, 60, 70, 79),
            new Tier(3, 15, 50, 60, 69),
            new Tier(4, 20, 40, 50, 59),
            new Tier(5, 25, 30, 40, 49),
            new Tier(6, 30, 20, 30, 39),
            new Tier(7, 35, 10, 20, 29),
            new Tier(8, 40, 0, 10, 19),
            new Tier(9, 100, 0, 1, 9) } },
        { ItemStat.ChaosDamageMore, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.PhysicalDamageFlat, new List<Tier>() {
            new Tier(0, 1, 80, 37, 40),
            new Tier(1, 5, 70, 33, 36),
            new Tier(2, 10, 60, 29, 32),
            new Tier(3, 15, 50, 25, 28),
            new Tier(4, 20, 40, 21, 24),
            new Tier(5, 25, 30, 17, 20),
            new Tier(6, 30, 20, 13, 16),
            new Tier(7, 35, 10, 9, 12),
            new Tier(8, 40, 0, 5, 8),
            new Tier(9, 100, 0, 1, 4) } },
        { ItemStat.PhysicalDamageIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 90, 100),
            new Tier(1, 5, 70, 80, 89),
            new Tier(2, 10, 60, 70, 79),
            new Tier(3, 15, 50, 60, 69),
            new Tier(4, 20, 40, 50, 59),
            new Tier(5, 25, 30, 40, 49),
            new Tier(6, 30, 20, 30, 39),
            new Tier(7, 35, 10, 20, 29),
            new Tier(8, 40, 0, 10, 19),
            new Tier(9, 100, 0, 1, 9) } },
        { ItemStat.PhysicalDamageMore, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.HealthFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.HealthIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.HealthMore, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.ManaFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.ManaIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.ManaMore, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.ArmorFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.ArmorIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.ArmorMore, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.EnergyShieldFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.EnergyShieldIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.EnergyShieldMore, new List<Tier>() {
            new Tier(0, 1, 80, 8, 10),
            new Tier(1, 5, 50, 5, 7),
            new Tier(2, 10, 10, 2, 4) } },
        { ItemStat.MovementSpeedIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.FireResistanceFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.IceResistanceFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.LightningResistanceFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.PoisonResistanceFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.ChaosResistanceFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.PhysicalResistanceFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.AttackSpeedIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.CritChanceIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 19, 20),
            new Tier(1, 5, 70, 17, 18),
            new Tier(2, 10, 60, 15, 16),
            new Tier(3, 15, 50, 13, 14),
            new Tier(4, 20, 40, 11, 12),
            new Tier(5, 25, 30, 9, 10),
            new Tier(6, 30, 20, 7, 8),
            new Tier(7, 35, 10, 5, 6),
            new Tier(8, 40, 0, 3, 4),
            new Tier(9, 100, 0, 1, 2) } },
        { ItemStat.CritDamageIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.HealthLeechFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.ManaLeechFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.AreaOfEffectIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 19, 20),
            new Tier(1, 5, 70, 17, 18),
            new Tier(2, 10, 60, 15, 16),
            new Tier(3, 15, 50, 13, 14),
            new Tier(4, 20, 40, 11, 12),
            new Tier(5, 25, 30, 9, 10),
            new Tier(6, 30, 20, 7, 8),
            new Tier(7, 35, 10, 5, 6),
            new Tier(8, 40, 0, 3, 4),
            new Tier(9, 100, 0, 1, 2) } },
        { ItemStat.AreaOfEffectMore, new List<Tier>() {
            new Tier(0, 1, 80, 19, 20),
            new Tier(1, 5, 70, 17, 18),
            new Tier(2, 10, 60, 15, 16),
            new Tier(3, 15, 50, 13, 14),
            new Tier(4, 20, 40, 11, 12),
            new Tier(5, 25, 30, 9, 10),
            new Tier(6, 30, 20, 7, 8),
            new Tier(7, 35, 10, 5, 6),
            new Tier(8, 40, 0, 3, 4),
            new Tier(9, 100, 0, 1, 2) } },
        { ItemStat.ProjectileSpeedIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 19, 20),
            new Tier(1, 5, 70, 17, 18),
            new Tier(2, 10, 60, 15, 16),
            new Tier(3, 15, 50, 13, 14),
            new Tier(4, 20, 40, 11, 12),
            new Tier(5, 25, 30, 9, 10),
            new Tier(6, 30, 20, 7, 8),
            new Tier(7, 35, 10, 5, 6),
            new Tier(8, 40, 0, 3, 4),
            new Tier(9, 100, 0, 1, 2) } },
        { ItemStat.ProjectilesFlat, new List<Tier>() {
            new Tier(0, 1, 80, 4, 5),
            new Tier(1, 20, 30, 3, 4),
            new Tier(3, 100, 0, 1, 2) } },
        { ItemStat.PierceFlat, new List<Tier>() {
            new Tier(0, 1, 80, 4, 5),
            new Tier(1, 20, 30, 3, 4),
            new Tier(2, 100, 0, 1, 2) } },
        { ItemStat.ChainFlat, new List<Tier>() {
            new Tier(0, 1, 80, 4, 5),
            new Tier(1, 20, 30, 3, 4),
            new Tier(2, 100, 0, 1, 2) } },
        { ItemStat.MagicFindFlat, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.ExperienceFindFlat, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.GoldFindFlat, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.DodgeFlat, new List<Tier>() {
            new Tier(0, 1, 80, 19, 20),
            new Tier(1, 5, 70, 17, 18),
            new Tier(2, 10, 60, 15, 16),
            new Tier(3, 15, 50, 13, 14),
            new Tier(4, 20, 40, 11, 12),
            new Tier(5, 25, 30, 9, 10),
            new Tier(6, 30, 20, 7, 8),
            new Tier(7, 35, 10, 5, 6),
            new Tier(8, 40, 0, 3, 4),
            new Tier(9, 100, 0, 1, 2) } },
        { ItemStat.HealthRegenerationFlat, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
         { ItemStat.HealthRegenerationIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.ManaRegenerationFlat, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.ManaRegenerationIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.EnergyShieldRegenerationFlat, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.EnergyShieldRegenerationIncreased, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        { ItemStat.ReflectFlat, new List<Tier>() {
            new Tier(0, 1, 80, 46, 50),
            new Tier(1, 5, 70, 41, 45),
            new Tier(2, 10, 60, 36, 40),
            new Tier(3, 15, 50, 31, 35),
            new Tier(4, 20, 40, 26, 30),
            new Tier(5, 25, 30, 21, 25),
            new Tier(6, 30, 20, 16, 20),
            new Tier(7, 35, 10, 11, 15),
            new Tier(8, 40, 0, 6, 10),
            new Tier(9, 100, 0, 1, 5) } },
        { ItemStat.CooldownReductionFlat, new List<Tier>() {
            new Tier(0, 1, 80, 28, 30),
            new Tier(1, 5, 70, 25, 27),
            new Tier(2, 10, 60, 22, 24),
            new Tier(3, 15, 50, 19, 21),
            new Tier(4, 20, 40, 16, 18),
            new Tier(5, 25, 30, 13, 15),
            new Tier(6, 30, 20, 10, 12),
            new Tier(7, 35, 10, 7, 9),
            new Tier(8, 40, 0, 4, 6),
            new Tier(9, 100, 0, 1, 3) } },
        };


    public static Tier GetTier(ItemStat stat, float value)
    {
        Tier tier = new Tier(0, 0, 0, 0, 0);
        foreach (Tier t in statTiers[stat])
        {
            if (value >= t.minRoll && value <= t.maxRoll)
            {
                tier = t;
            }
        }
        return tier;
    }

    //Gets a tier for the stat based on the item level, higher item level = higher tier, highest tiers have low weight
    public static Tier GetWeightedTier(ItemStat stat, int itemLevel)
    {
        int highestTier = statTiers[stat].Count - 1; // Default to the lowest tier [9]
        foreach (Tier tier in statTiers[stat])
        {
            if (tier.tierNum < highestTier) //"lower" tiers are better so <
            {
                if (Helpers.PercentChance(tier.weight) && itemLevel >= tier.minLevel)
                {
                    highestTier = tier.tierNum;

                }
            }
        }
        return statTiers[stat][highestTier];
    }

    public static Tier GetTierZero(ItemStat stat)
    {
        return statTiers[stat][0];
    }



    public static ItemStat[] GetStatsForEquipmentType(EquipmentType equipmentType)
    {
        if (statDict.ContainsKey(equipmentType))
        {
            return statDict[equipmentType];
        }
        else
        {
            return new ItemStat[0]; // Return an empty array if equipment type not found
        }
    }

    public static string GetStatString(Stat stat)
    {
        return statString[stat];
    }
    public static string GetItemStatString(ItemStat stat)
    {
        return itemStatString[stat];
    }

    public static DamageType StatToDamage(Stat stat)
    {
        return statToDamageType[stat];
    }

    public static Stat DamageToResistance(DamageType type)
    {
        return damageTypeToResistanceStat[type];
    }

    public static Stat DamageToStat(DamageType type)
    {
        return damageTypeToDamageStat[type];
    }

    public static List<Stat> GetDamageTypes()
    {
        return new List<Stat> { Stat.FireDamage, Stat.IceDamage, Stat.LightningDamage, Stat.PoisonDamage, Stat.ChaosDamage, Stat.PhysicalDamage };
    }

    public static EModifierOperator GetItemStatModifierType(ItemStat itemStat)
    {
        return statTypeDict[itemStat];
    }

    public static ItemStat[] StatToItemStats(Stat stat)
    {
        return statToItemStat[stat];
    }
}
