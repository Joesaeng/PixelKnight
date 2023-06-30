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
        int test = 0;
        while(test == itemDB.Count)
        {
            foreach(Item item in itemDB)
            {
                if (item.itemImage == null)
                    continue;
                test++;
            }
        }
        //CreateFieldItem();
    }

    public void CreateFieldItem() // юс╫ц
    {
        for (int i = 0; i < pos.Length; ++i)
        {
            GameObject go;
            if (itemDB[i] is Equip equip)
            {
                go = Instantiate(fieldEquipPrefab, pos[i], Quaternion.identity);
                go.GetComponent<FieldEquip>().SetEquip(equip);
            }
            else
            {
                go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
                go.GetComponent<FieldItems>().SetItem(itemDB[i]);
            }
        }
    }
    private void SetItemDataBase()
    {
        Equip[] equips = 
            {
            new Equip("CommonHead", "Equip[Equip_0]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Head, BaseOption.Defence, 3f),
            new Equip("CommonBody", "Equip[Equip_4]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Body, BaseOption.Defence, 5f),
            new Equip("CommonHands", "Equip[Equip_8]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Hands, BaseOption.Defence, 2f),
            new Equip("CommonFoots", "Equip[Equip_12]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Foots, BaseOption.Defence, 2f),
            new Equip("CommonWeapon", "Equip[Equip_16]", ItemType.Equipment, ItemLevel.Common, EquipSlot.Weapon, BaseOption.Damage, 15f),
            new Equip("AdvancedHead", "Equip[Equip_1]", ItemType.Equipment, ItemLevel.Advanced, EquipSlot.Head, BaseOption.Defence, 5f
            ,AdditionalOptions.CriticalChance,0.05f),
            new Equip("AdvancedBody", "Equip[Equip_5]", ItemType.Equipment, ItemLevel.Advanced, EquipSlot.Body, BaseOption.Defence, 7f
            ,AdditionalOptions.Vitality,5f),
            new Equip("AdvancedHands", "Equip[Equip_9]", ItemType.Equipment, ItemLevel.Advanced, EquipSlot.Hands, BaseOption.Defence, 3f
            ,AdditionalOptions.AttackSpeed,0.2f),
            new Equip("AdvancedFoots", "Equip[Equip_13]", ItemType.Equipment, ItemLevel.Advanced, EquipSlot.Foots, BaseOption.Defence, 3f
            ,AdditionalOptions.MoveSpeed,0.2f),
            new Equip("AdvancedWeapon", "Equip[Equip_17]", ItemType.Equipment, ItemLevel.Advanced, EquipSlot.Weapon, BaseOption.Damage, 22f
            ,AdditionalOptions.HitRate,10f),
            };


        itemDB.AddRange(equips);
    }
}
