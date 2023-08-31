using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : Singleton<ItemDataBase>
{

    public List<Item> itemDB = new List<Item>();

    public GameObject fieldConsumablePrefab;
    public GameObject fieldEquipPrefab;
    
    
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

        Consumable[] consumables =
        {
            new Consumable("HpPotion","Potions[potions_0]",ItemType.Consumable,ItemLevel.Unique,ConsumableType.HpRecovery,50f,0f)
        };

        itemDB.AddRange(equips);
        itemDB.AddRange(consumables);
    }
    public Consumable GetConsumableData(int index)
    {
        if (itemDB[index] is Consumable consumable)
        {
            return consumable;
        }
        else return null;
    }
    public Consumable FindAndGetConsumable(string itemname)
    {
        for(int i = 5; i < itemDB.Count; ++i)
        {
            if (itemDB[i].itemName == itemname)
                if (itemDB[i] is Consumable consum)
                    return consum;
        }
        return null;
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
