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
public class Item : ScriptableObject
{
    public string itemName; // ������ �̸�
    public Sprite itemImage;
    public List<ItemEffect> efts;
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
