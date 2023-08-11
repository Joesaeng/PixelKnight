using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : Singleton<ItemDataBase>
{

    public List<Item> itemDB = new List<Item>();

    public GameObject fieldItemPrefab;
    public GameObject fieldEquipPrefab;
    public Vector2[] pos;
    
    
    private void Start()
    {
        SetItemDataBase();
    }

    private void SetItemDataBase()
    {
        Equip[] equips = 
            {
            new Equip("Head", "Equip[Equip_0]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Head, BaseOption.Defence, 3f),
            new Equip("Body", "Equip[Equip_4]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Body, BaseOption.Defence, 5f),
            new Equip("Hands", "Equip[Equip_8]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Hands, BaseOption.Defence, 2f),
            new Equip("Foots", "Equip[Equip_12]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Foots, BaseOption.Defence, 2f),
            new Equip("Weapon", "Equip[Equip_16]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Weapon, BaseOption.Damage, 15f),
            };


        itemDB.AddRange(equips);
    }
    public Item GetItemData(int index)
    {
        return itemDB[index];
    }
    public Equip GetEquipData(int index)
    {
        if (itemDB[index] is Equip equip)
        {
            return equip;
        }
        else return null;
    }
}
