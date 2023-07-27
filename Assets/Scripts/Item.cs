using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    Etc,
}
public enum ItemLevel
{
    Common,
    Advanced,
    Rare,
    Unique,
}

[System.Serializable]
public class Item 
{
    public string itemName; // ������ �̸�
    public Sprite itemImage;
    public string iconAddress;
    public List<ItemEffect> efts = new List<ItemEffect>();
    public ItemType itemType;
    public ItemLevel itemLevel; // ������ ����

    public bool Use()
    {
        bool isUsed = false;
        foreach (ItemEffect effect in efts)
        {
            isUsed = effect.ExecuteRole();
        }

        return isUsed;
    }
}
