using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipSlot
{
    Weapon,
    Head,
    Body,
    Hands,
    Foots
}
public enum BaseOption
{
    Damage,
    Defence,
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
    AttackSpeed,
    Evade,
    HitRate,
    MoveSpeed,
    CriticalChance,
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
    public Equip (string _name, string _iconPath, ItemType _type, string _effectPath, ItemLevel _level,
        EquipSlot _slot, BaseOption _baseOption, float _baseOptionValue)
    {
        this.itemName = _name;
        this.itemImage = Resources.Load<Sprite>(_iconPath);
        this.itemType = _type;
        this.efts.Add(Resources.Load<ItemEffect>(_effectPath));
        this.itemLevel = _level;
        this.equipSlot = _slot;
        this.baseOption = _baseOption;
        this.baseOptionValue = _baseOptionValue;
    }
    public Equip(string _name, string _iconPath, ItemType _type, string _effectPath, ItemLevel _level,
        EquipSlot _slot, BaseOption _baseOption, float _baseOptionValue, AdditionalOptions _addtionalOption,
        float _addtionalOptionValue)
    {
        this.itemName = _name;
        this.itemImage = Resources.Load<Sprite>(_iconPath);
        this.itemType = _type;
        this.efts.Add(Resources.Load<ItemEffect>(_effectPath));
        this.itemLevel = _level;
        this.equipSlot = _slot;
        this.baseOption = _baseOption;
        this.baseOptionValue = _baseOptionValue;
        this.additionalOptions.Add(_addtionalOption, _addtionalOptionValue);
    }
}
