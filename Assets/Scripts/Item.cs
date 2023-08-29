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
    public string itemName; // 아이템 이름
    public Sprite itemImage;
    public string iconAddress;
    public ItemEffect eft;
    public ItemType itemType;
    public ItemLevel itemLevel; // 아이템 레벨

    public bool Use()
    {
        bool isUsed = eft.ExecuteRole();

        return isUsed;
    }
}
