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
            // 이미 해당 슬롯에 장비가 있는 경우, 장비를 교체합니다.
            equippedItems[slot] = item;
        }
        else
        {
            // 해당 슬롯에 장비가 없는 경우, 새로 장착합니다.
            equippedItems.Add(slot, item);
        }

        OnEquiped.Invoke(slot, item);
        // 장비를 장착한 후에 필요한 추가 처리를 수행합니다.
        // 예: 스탯 업데이트, 시각적인 변화 등
        // ...
    }

    public void UnequipItem(EquipSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            equippedItems.Remove(slot);

            OnUnEquiped.Invoke(slot);
            // 장비를 해제한 후에 필요한 추가 처리를 수행합니다.
            // 예: 스탯 업데이트, 시각적인 변화 등
            // ...
        }
    }
}
