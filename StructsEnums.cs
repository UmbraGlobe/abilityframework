using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#region Player
[System.Serializable]
public struct PlayerStateData
{
    public int currentHealth;
    public int maxHealth;

    public int currentMana;
    public int maxMana;

    public int experience;
    public int experienceToNextLevel;

    public int level;

    public int gold;

    public PlayerStateData(int _currentHealth, int _maxHealth, int _currentMana, int _maxMana, int _experience, int _experienceToNextLevel, int _level, int _gold)
    {
        currentHealth = _currentHealth;
        maxHealth = _maxHealth;
        currentMana = _currentMana;
        maxMana = _maxMana;
        experience = _experience;
        experienceToNextLevel = _experienceToNextLevel;
        level = _level;
        gold = _gold;
    }

    public PlayerStateData(int _experience, int _experienceToNextLevel, int _level, int _gold)
    {
        currentHealth = 0;
        maxHealth = 0;
        currentMana = 0;
        maxMana = 0;
        experience = _experience;
        experienceToNextLevel = _experienceToNextLevel;
        level = _level;
        gold = _gold;
    }
}
#endregion
#region Inventory
[System.Serializable]
public class ItemStack
{
    private ItemInstance item;
    private int quantity;

    public ItemInstance Item => item;
    public int Quantity => quantity;

    public ItemStack(ItemInstance item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public ItemStack()
    {
        this.item = null;
        this.quantity = 0;
    }

    public void AddAmount(int value)
    {
        quantity += value;
    }

    public void ChangeItem(ItemInstance _item)
    {
        item = _item;
    }
}

[System.Serializable]
public struct ItemSaveData
{
    public string GUID;
    public RarityType rarity;
    public int quantity;
    public GenericDictionary<ItemStat, float> affixes;
    public int itemLevel;
    public int sellPrice;

    public ItemSaveData(string _GUID, int _quantity, RarityType _rarity, GenericDictionary<ItemStat, float> _affixes, int _sellPrice, int _itemLevel)
    {
        GUID = _GUID;
        quantity = _quantity;
        rarity = _rarity;
        affixes = _affixes;
        sellPrice = _sellPrice;
        itemLevel = _itemLevel;
    }
    public ItemSaveData(string _GUID, int _quantity, RarityType _rarity)
    {
        GUID = _GUID;
        quantity = _quantity;
        rarity = _rarity;
        affixes = null;
        sellPrice = 0;
        itemLevel = 0;
    }
}

[System.Serializable]
public struct InventoryData
{
    public int index1;
    public int index2;
    public UIInventoryManager inventoryManager;

    public InventoryData(int one, int two, UIInventoryManager _parent)
    {
        index1 = one;
        index2 = two;
        inventoryManager = _parent;
    }
}

#endregion
#region Items

[System.Serializable]
public enum ItemType
{
    Equipment,
    Consumable,
    Material,
    Default
}

[System.Serializable]
public enum EquipmentType
{
    Amulet,
    Helmet,
    Charm,
    Weapon,
    Chestplate,
    Offhand,
    Gloves,
    Leggings,
    Ring,
    Boots,
}

[System.Serializable]
public enum RarityType
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5,
}

[System.Serializable]
public enum DamageType
{
    FireDamage,
    IceDamage,
    LightningDamage,
    PoisonDamage,
    ChaosDamage,
    PhysicalDamage,
}

[Flags]
public enum WeaponStyle
{
    None = 0,
    Melee = 1,
    Ranged = 2,
    Magic = 4,
    All = Melee | Ranged | Magic
}

[System.Serializable]
public enum WeaponType
{
    Sword,
    Axe,
    Bow,
    Staff,
    Wand,
    Dagger,
    Mace,
    Spear,
    Crossbow,
}

[System.Serializable]
public enum WieldingType
{
    OneHanded,
    TwoHanded,
}

#endregion
#region Map

[System.Serializable]
public struct MapData
{
    [Min(0)] public int width;
    [Min(0)] public int height;
    [Min(0)] public float scale;

    [Min(0)] public int octaves;
    [Min(0)] public float persistance;
    [Min(0)] public float lacunarity;
    [Min(0)] public Vector2 offset;

    public int seed;
}

[System.Serializable]
public struct TilePallete
{
    public TileBase grass;
    public TileBase water;
    public TileBase sand;
}

#endregion
#region Enemy
public enum EnemyGrade
{
    Normal = 100,
    Elite = 300,
    Special = 500,
    Boss = 600
}
#endregion
#region Stats
[System.Serializable]
public struct ProjectileData
{
    public List<DamageData> damages;
    public EffectInstance effect;
    public EffectInstance[] statusEffects;
    public int pierce;
    public int chain;
    public float despawnTime;
    public float areaOfEffect;  
    public float initialVelocity;   
    public Sprite projectileSprite;
    public Vector2 directionalVelocity;

    public ProjectileData(List<DamageData> _damages, EffectInstance _effect, EffectInstance[] _statusEffects, int _pierce, int _chain, float _despawnTime, float _areaOfEffect, float _initialVelocity, Sprite _projectileSprite, Vector2 _directionalVelocity)
    {
        damages = _damages;
        effect = _effect;
        statusEffects = _statusEffects;
        pierce = _pierce;
        chain = _chain;
        despawnTime = _despawnTime;
        areaOfEffect = _areaOfEffect;
        initialVelocity = _initialVelocity;
        projectileSprite = _projectileSprite;
        directionalVelocity = _directionalVelocity;
    }
}

[System.Serializable]
public struct DamageData
{
    public DamageType DamageType;
    public int Damage;

    public DamageData(DamageType _damageType, int _damage)
    {
        DamageType = _damageType;
        Damage = _damage;
    }
}
[System.Serializable]
public enum Stat
{
    FireDamage,
    IceDamage,
    LightningDamage,
    PoisonDamage,
    ChaosDamage,
    PhysicalDamage,

    Health,
    Mana,
    Armor,
    EnergyShield,

    MovementSpeed,

    FireResistance,
    IceResistance,
    LightningResistance,
    PoisonResistance,
    ChaosResistance,
    PhysicalResistance,

    AttackSpeed,
    CritChance,
    CritDamage,
    HealthLeech,
    ManaLeech,
    AreaOfEffect,

    ProjectileSpeed,
    Projectiles,
    Pierce,
    Chain,

    MagicFind,
    GoldFind,
    ExperienceFind,

    Knockback,
    Dodge,

    HealthRegeneration,
    ManaRegeneration,
    EnergyShieldRegeneration,

    Reflect,
    CooldownReduction,
}

[Serializable]
public struct EffectModifier
{
    public Stat Stat;
    public EModifierOperator ModifierOperator;
    public EStatChange StatChanged;
    public BaseMagnitudeSO ModifierMagnitude;
    public BaseCueSO EffectCue;
    public float Multiplier;
}

[System.Serializable]
public struct StatModifier
{ 
    public float Add;
    public float Increase;
    public float More;
    public float Override;

    public StatModifier(float magnitude, EModifierOperator type)
    {
        switch (type)
        {
            case EModifierOperator.Add:
                Add = magnitude;
                Increase = 0;
                More = 0;
                Override = 0;
                break;
            case EModifierOperator.Increase:
                Add = 0;
                Increase = magnitude;
                More = 0;
                Override = 0;
                break;
            case EModifierOperator.More:
                Add = 0;
                Increase = 0;
                More = magnitude;
                Override = 0;
                break;
            case EModifierOperator.Override:
                Add = 0;
                Increase = 0;
                More = 0;
                Override = magnitude;
                break;
            default:
                Add = 0;
                Increase = 0;
                More = 0;
                Override = 0;
                break;
        }
    }
    public static StatModifier Combine(StatModifier one, StatModifier two)
    {
        StatModifier other = new StatModifier();
        other.Add = one.Add + two.Add;
        other.Increase = one.Increase + two.Increase;
        other.More = one.More + two.More;
        other.Override = one.Override + two.Override;

        return other;
    }

    public static StatModifier Subtract(StatModifier one, StatModifier two)
    {
        StatModifier other = new StatModifier();
        other.Add = one.Add - two.Add;
        other.Increase = one.Increase - two.Increase;
        other.More = one.More - two.More;
        other.Override = one.Override - two.Override;

        return other;
    }
}


[System.Serializable]
public struct StatData
{
    public ItemStat Stat;
    public Tier Tier;

    public StatData(ItemStat _stat, Tier _tier)
    {
        Stat = _stat;
        Tier = _tier;
    }

    public StatData(ItemStat _stat)
    {
        Stat = _stat;
        Tier = new Tier(0, 0, 0, 0, 0);
    }

}
public struct DifficultyBoost
{
    public int health;
    public int resistance;
    public int damage;
    public int experience;
    public int gold;
    public int loot;
    public int playerResistance;
}
#endregion
#region Misc
[System.Serializable]
public enum SoundType
{
    Hit,
    Loot,
    Music
}
public struct NumberTextData
{
    public string Str;
    public Vector3 Position;
    public NumberType Type;

    public NumberTextData(string _str, Vector3 _position, NumberType _type)
    {
        Str = _str;
        Position = _position;
        Type = _type;
    }
}
#endregion



