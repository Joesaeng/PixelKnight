using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
    IncreasedItemFindingChance,
    HpRegen,
    StaminaRegen,
    PoiseRegen,
}
[System.Serializable]
public class Equip : Item
{
    public EquipSlot equipSlot;
    public BaseOption baseOption;
    public Dictionary<AdditionalOptions, float> additionalOptions = new();
    public float baseOptionValue;
    public Equip()
    { }
    
    public Equip (string _name, string _iconAddress, ItemType _type, ItemLevel _level,
        EquipSlot _slot, BaseOption _baseOption, float _baseOptionValue)
    {
        this.itemName = _name;
        this.iconAddress = _iconAddress;
        LoadEquipResourcesAsync(iconAddress);
        ItemEquipEft itemEquipEft = new();
        SetItemEffect(itemEquipEft);
        this.itemType = _type;
        this.itemLevel = _level;
        this.equipSlot = _slot;
        this.baseOption = _baseOption;
        this.baseOptionValue = _baseOptionValue;
    }
    public Equip(string _name, string _iconAddress, ItemType _type, ItemLevel _level,
        EquipSlot _slot, BaseOption _baseOption, float _baseOptionValue, AdditionalOptions _addtionalOption,
        float _addtionalOptionValue)
    {
        this.itemName = _name;
        this.iconAddress = _iconAddress;
        LoadEquipResourcesAsync(iconAddress);
        ItemEquipEft itemEquipEft = new();
        SetItemEffect(itemEquipEft);
        this.itemType = _type;
        this.itemLevel = _level;
        this.equipSlot = _slot;
        this.baseOption = _baseOption;
        this.baseOptionValue = _baseOptionValue;
        this.additionalOptions.Add(_addtionalOption, _addtionalOptionValue);
    }

    public void LoadEquipResourcesAsync(string _iconAddress)
    {
        Addressables.LoadAssetAsync<Sprite>(_iconAddress).Completed += OnLoadIcon;
    }
    private void OnLoadIcon(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> obj)
    {
        this.itemImage = obj.Result;
    }
    private void SetItemEffect(ItemEffect itemEffect)
    {
        if (itemEffect is ItemEquipEft equipEft)
        {
            equipEft.SetEquipInfo(this);
            this.efts.Add(equipEft);
        }
    }
    public void SetItemData(Equip equip)
    {
        this.itemName = equip.itemName;
        LoadEquipResourcesAsync(equip.iconAddress);
        ItemEquipEft itemEquipEft = new();
        SetItemEffect(itemEquipEft);
        this.itemType = equip.itemType;
        this.itemLevel = equip.itemLevel;
        this.equipSlot = equip.equipSlot;
        this.baseOption = equip.baseOption;
        this.baseOptionValue = equip.baseOptionValue;
        if(equip.additionalOptions.Count>0)
        {
            foreach(var option in equip.additionalOptions)
            {
                this.additionalOptions.Add(option.Key, option.Value);
            }
        }
    }

}
