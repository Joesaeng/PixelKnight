using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipSlot
{
    Weapon,
    Head,
    Body,
    Hands,
    Foot
}
public enum BaseOption
{
    AttackDamage,
    Defence,
    AttackSpeed,
    CriticalChance,
    MoveSpeed
}
public enum AdditionalOptions
{
    Vitality,
    Endurance,
    Strength,
    Dexterity,
    Luck,
    MaxHp,
    MaxStamina,
    Stagger,
    Poise,
    Evade,
    HitRate,
    CriticalHitDamage,
    IncreasedItemFindingChance
}
[System.Serializable]
[CreateAssetMenu(menuName = "ItemDB/Item/Equipment")]
public class Equip : Item
{
    public EquipSlot equipSlot;
    public BaseOption baseOption;
    public Dictionary<AdditionalOptions, float> additionalOptions;
    public float baseOptionValue;
}
