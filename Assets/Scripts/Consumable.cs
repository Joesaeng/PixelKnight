using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public enum ConsumableType
{
    HpRecovery,
    PowerEnforce,
    SpeedEnforce
}

[System.Serializable]
public class Consumable : Item
{
    public ConsumableType consumableType;
    public float value;
    public float duration;

    public Consumable()
    { }
    public Consumable(string _name, /*string _iconAddress,*/string imagename, ItemType _type, ItemLevel _level,
        ConsumableType type, float value, float duration)
    {
        this.itemName = _name;
        this.imageName = imagename;
        this.itemImage = DataManager.Instance.GetImage(imageName);
        //this.iconAddress = _iconAddress;
        //LoadEquipResourcesAsync(iconAddress);
        ItemConsumEft itemConsumEft = new();
        SetItemEffect(itemConsumEft);
        this.itemType = _type;
        this.itemLevel = _level;
        this.consumableType = type;
        this.value = value;
        this.duration = duration;
    }
    public void SetItemEffect(ItemEffect itemEffect)
    {
        if(itemEffect is ItemConsumEft consumEft)
        {
            consumEft.SetConsumableInfo(this);
            this.eft = consumEft;
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
    
    public Consumable (Consumable other)
    {
        this.itemName = other.itemName;
        this.imageName = other.imageName;
        this.itemImage = DataManager.Instance.GetImage(imageName);
        //this.iconAddress = other.iconAddress;
        //this.itemImage = other.itemImage;
        ItemConsumEft itemConsumEft = new();
        SetItemEffect(itemConsumEft);
        this.itemType = other.itemType;
        this.itemLevel = other.itemLevel;
        this.consumableType = other.consumableType;
        this.value = other.value;
        this.duration = other.duration;
    }
}
