using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Equipment : MonoBehaviour
{
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

    public delegate void DelegateUnEquiped(EquipSlot slot);
    public event DelegateUnEquiped OnUnEquiped;

    public delegate void ChangeEquipment();
    public event ChangeEquipment OnChangeEquipment;

    public Equip[] GetCurEquip()
    {
        Equip[] curEquips = equippedItems.Values.ToArray();

        return curEquips;
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
        if (OnEquiped == null) Debug.Log("OnEquiped is null");
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
