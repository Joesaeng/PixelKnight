using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Equipment : MonoBehaviour
{
    private Dictionary<EquipSlot, Equip> equippedItems = new Dictionary<EquipSlot, Equip>();
    public event Action<EquipSlot, Equip> OnEquiped;
    public event Action<EquipSlot> OnUnEquiped;

    public void EquipItem(EquipSlot slot, Equip item)
    {
        if (equippedItems.ContainsKey(slot))
        {
            // �̹� �ش� ���Կ� ��� �ִ� ���, ��� ��ü�մϴ�.
            equippedItems[slot] = item;
        }
        else
        {
            // �ش� ���Կ� ��� ���� ���, ���� �����մϴ�.
            equippedItems.Add(slot, item);
        }

        OnEquiped.Invoke(slot, item);
        // ��� ������ �Ŀ� �ʿ��� �߰� ó���� �����մϴ�.
        // ��: ���� ������Ʈ, �ð����� ��ȭ ��
        // ...
    }

    public void UnequipItem(EquipSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            equippedItems.Remove(slot);

            OnUnEquiped.Invoke(slot);
            // ��� ������ �Ŀ� �ʿ��� �߰� ó���� �����մϴ�.
            // ��: ���� ������Ʈ, �ð����� ��ȭ ��
            // ...
        }
    }
}
