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
public class AdditionalOption
{
    public AdditionalOptions option;
    public float value;
    
    public AdditionalOption(AdditionalOptions option,float value)
    {
        this.option = option;
        this.value = value;
    }
    public AdditionalOptions Key
    {
        get => option;
    }
    public float Value { get => value; }
}
[System.Serializable]
public class Equip : Item
{
    public EquipSlot equipSlot;
    public BaseOption baseOption;
    public List<AdditionalOption> additionalOptions = new();
    public float baseOptionValue;
    public Equip()
    { }
    public Equip(Equip other)
    {
        this.itemName = other.itemName;
        this.imageName = other.imageName;
        this.itemImage = DataManager.Instance.GetImage(imageName);
        //this.iconAddress = other.iconAddress;
        //LoadEquipResourcesAsync(iconAddress);
        ItemEquipEft itemEquipEft = new();
        SetItemEffect(itemEquipEft);
        this.itemType = other.itemType;
        this.itemLevel = other.itemLevel;
        this.equipSlot = other.equipSlot;
        this.baseOption = other.baseOption;
        this.baseOptionValue = other.baseOptionValue;
        this.additionalOptions = other.additionalOptions;
    }
    
    public Equip (string _name, /*string _iconAddress,*/string _imagename, ItemType _type, ItemLevel _level,
        EquipSlot _slot, BaseOption _baseOption, float _baseOptionValue)
    {
        this.itemName = _name;
        this.imageName = _imagename;
        this.itemImage = DataManager.Instance.GetImage(imageName);
        //this.iconAddress = _iconAddress;
        //LoadEquipResourcesAsync(iconAddress);
        ItemEquipEft itemEquipEft = new();
        SetItemEffect(itemEquipEft);
        this.itemType = _type;
        this.itemLevel = _level;
        this.equipSlot = _slot;
        this.baseOption = _baseOption;
        this.baseOptionValue = _baseOptionValue;
    }
    public void SetItemData(Equip equip)
    {
        this.itemName = equip.itemName;
        this.imageName = equip.imageName;
        this.itemImage = DataManager.Instance.GetImage(imageName);
        //this.iconAddress = equip.iconAddress;
        //LoadEquipResourcesAsync(equip.iconAddress);
        ItemEquipEft itemEquipEft = new();
        SetItemEffect(itemEquipEft);
        this.itemType = equip.itemType;
        this.itemLevel = equip.itemLevel;
        this.equipSlot = equip.equipSlot;
        this.baseOption = equip.baseOption;
        this.baseOptionValue = equip.baseOptionValue;
        if (equip.additionalOptions.Count > 0)
        {
            foreach (var option in equip.additionalOptions)
            {
                additionalOptions.Add(option);
            }
        }
    }

    public void LoadEquipResourcesAsync(string _iconAddress)
    {
        Addressables.LoadAssetAsync<Sprite>(_iconAddress).Completed += OnLoadIcon;
    }
    private void OnLoadIcon(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> obj)
    {
        this.itemImage = obj.Result;
    }
    public void SetItemEffect(ItemEffect itemEffect)
    {
        if (itemEffect is ItemEquipEft equipEft)
        {
            equipEft.SetEquipInfo(this);
            this.eft = equipEft;
        }
    }
    public void LevelUpItem(ItemLevel level)
    {
        string levelString = level.ToString();
        this.itemName = levelString + " " + this.itemName;
        this.itemLevel = level;
        switch (level)
        {
            case ItemLevel.Advanced:
                this.baseOptionValue *= 1.2f;
                switch(this.equipSlot)
                {
                    case EquipSlot.Weapon:
                        //LoadEquipResourcesAsync("Equip[Equip_17]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_17");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.HitRate, 10f));
                        break;
                    case EquipSlot.Head:
                        //LoadEquipResourcesAsync("Equip[Equip_1]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_1");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.CriticalChance, 0.05f));
                        break;
                    case EquipSlot.Body:
                        //LoadEquipResourcesAsync("Equip[Equip_5]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_5");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Vitality, 5f));
                        break;
                    case EquipSlot.Hands:
                        //LoadEquipResourcesAsync("Equip[Equip_9]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_9");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.AttackSpeed, 0.2f));
                        break;
                    case EquipSlot.Foots:
                        //LoadEquipResourcesAsync("Equip[Equip_13]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_13");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.MoveSpeed, 0.2f));
                        break;
                }
                break;
            case ItemLevel.Rare:
                this.baseOptionValue *= 1.6f;
                switch (this.equipSlot)
                {
                    case EquipSlot.Weapon:
                        //LoadEquipResourcesAsync("Equip[Equip_18]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_18");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.HitRate, 13f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.CriticalHitDamage, 0.10f));
                        break;
                    case EquipSlot.Head:
                        //LoadEquipResourcesAsync("Equip[Equip_2]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_2");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.CriticalChance, 0.10f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Strength, 10f));
                        break;
                    case EquipSlot.Body:
                        //LoadEquipResourcesAsync("Equip[Equip_6]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_6");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Vitality, 10f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Endurance, 10f));
                        break;
                    case EquipSlot.Hands:
                        //LoadEquipResourcesAsync("Equip[Equip_10]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_10");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.AttackSpeed, 0.3f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Evade, 7f));
                        break;
                    case EquipSlot.Foots:
                        //LoadEquipResourcesAsync("Equip[Equip_14]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_14");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.MoveSpeed, 0.25f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.MaxStamina, 50f));
                        break;
                }
                break;
            case ItemLevel.Unique:
                this.baseOptionValue *= 2f;
                switch (this.equipSlot)
                {
                    case EquipSlot.Weapon:
                        //LoadEquipResourcesAsync("Equip[Equip_19]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_19");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.HitRate, 16f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.CriticalHitDamage, 0.15f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Stagger, 15f));
                        break;
                    case EquipSlot.Head:
                        //LoadEquipResourcesAsync("Equip[Equip_3]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_3");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.CriticalChance, 0.2f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Strength, 18f));
                        break;
                    case EquipSlot.Body:
                        //LoadEquipResourcesAsync("Equip[Equip_7]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_7");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Vitality, 15f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Endurance, 15f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.HpRegen, 10f));
                        break;
                    case EquipSlot.Hands:
                        //LoadEquipResourcesAsync("Equip[Equip_11]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_11");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.AttackSpeed, 0.5f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.Evade, 10f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.CriticalChance, 0.15f));
                        break;
                    case EquipSlot.Foots:
                        //LoadEquipResourcesAsync("Equip[Equip_15]");
                        this.itemImage = DataManager.Instance.GetImage("Equip_15");
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.MoveSpeed, 0.35f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.MaxStamina, 70f));
                        this.additionalOptions.Add(new AdditionalOption(AdditionalOptions.StaminaRegen, 5f));
                        break;
                }
                break;
            default:
                break;
        }
        this.imageName = itemImage.name;
    }
}
