using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Equipment : MonoBehaviour
{
    // �÷��̾��� ���� ��� ����ϴ� Ŭ�����Դϴ�.
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
    // ��� �����ɶ� ȣ��Ǵ� �̺�Ʈ

    public delegate void DelegateUnEquiped(EquipSlot slot);
    public event DelegateUnEquiped OnUnEquiped;
    // ��� �����ɶ� ȣ��Ǵ� �̺�Ʈ

    public delegate void ChangeEquipment();
    public event ChangeEquipment OnChangeEquipment;
    // �����̵� ������ ���� ��� ����� �� ȣ��Ǵ� �̺�Ʈ

    public Equip[] GetCurEquip()
    {
        Equip[] curEquips = equippedItems.Values.ToArray();

        return curEquips;
    }
    public void LoadEquip()
    {
        // ����� �����Ϳ��� ���� ��� �ܾ�ɴϴ�.
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
            // �̹� �ش� ���Կ� ��� �ִ� ���, ��� ��ü�մϴ�.
            Inventory.Instance.AddItem(equippedItems[slot]);
            UnequipItem(slot);
            equippedItems[slot] = item;
        }
        else
        {
            // �ش� ���Կ� ��� ���� ���, ���� �����մϴ�.
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
