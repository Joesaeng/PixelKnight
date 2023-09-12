using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Equipment : MonoBehaviour
{
    // 플레이어의 현재 장비를 담당하는 클래스입니다.
    #region SINGLETON
    private static Equipment instance;
    public static Equipment Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public Dictionary<EquipSlot, Equip> equippedItems = new();

    public delegate void DelegateEquiped(EquipSlot slot,Equip equip);
    public event DelegateEquiped OnEquiped;
    // 장비가 장착될때 호출되는 이벤트

    public delegate void DelegateUnEquiped(EquipSlot slot);
    public event DelegateUnEquiped OnUnEquiped;
    // 장비가 해제될때 호출되는 이벤트

    public delegate void ChangeEquipment();
    public event ChangeEquipment OnChangeEquipment;
    // 장착이든 해제든 현재 장비가 변경될 때 호출되는 이벤트

    public Equip[] GetCurEquip()
    {
        Equip[] curEquips = equippedItems.Values.ToArray();

        return curEquips;
    }
    public void LoadEquip()
    {
        // 저장된 데이터에서 현재 장비를 긁어옵니다.
        List<Equip> curEquips = SaveDataManager.Instance.saveData.curEquips;
        for(int i = 0; i < curEquips.Count; ++i)
        {
            if(curEquips[i] is Equip equip)
            {
                Equip newequ = new Equip(equip);
                ItemEquipEft itemEquipEft = new();
                newequ.SetItemEffect(itemEquipEft);
                EquipItem(newequ.equipSlot, newequ);
            }
        }
    }
    public void EquipItem(EquipSlot slot, Equip item)
    {
        if (item == null) Debug.Log("item is null");
        if (equippedItems.ContainsKey(slot))
        {
            // 이미 해당 슬롯에 장비가 있는 경우, 장비를 교체합니다.
            Inventory.Instance.AddItem(equippedItems[slot]);
            UnequipItem(slot);
            equippedItems[slot] = item;
        }
        else
        {
            // 해당 슬롯에 장비가 없는 경우, 새로 장착합니다.
            equippedItems.Add(slot, item);
        }
        OnEquiped?.Invoke(slot, item);
        OnChangeEquipment?.Invoke();
    }

    public void UnequipItem(EquipSlot slot)
    {
        if (equippedItems.ContainsKey(slot) &&  Inventory.Instance.items.Count < Inventory.Instance.SlotCnt)
        {
            equippedItems.Remove(slot);

            OnUnEquiped?.Invoke(slot);
            OnChangeEquipment.Invoke();
        }
    }
}
