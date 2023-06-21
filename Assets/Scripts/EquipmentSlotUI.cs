using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    public EquipSlot slotType;
    public Image iconImage;
    public Text nameLabel;

    private void OnEnable()
    {
        // Equipment 컴포넌트의 이벤트에 구독합니다.
        Equipment equipment = GetComponentInParent<Equipment>();
        if (equipment != null)
        {
            equipment.OnEquiped += HandleItemEquipped;
            equipment.OnUnEquiped += HandleItemUnequipped;
        }
    }

    private void OnDisable()
    {
        // Equipment 컴포넌트의 이벤트 구독을 해제합니다.
        Equipment equipment = GetComponentInParent<Equipment>();
        if (equipment != null)
        {
            equipment.OnEquiped -= HandleItemEquipped;
            equipment.OnUnEquiped -= HandleItemUnequipped;
        }
    }

    private void HandleItemEquipped(EquipSlot slot, Equip item)
    {
        // 해당 슬롯에 장비가 장착되었을 때 UI를 갱신합니다.
        if (slot == slotType)
        {
            // 아이콘, 이름 등을 업데이트합니다.
            iconImage.sprite = item.itemImage;
            nameLabel.text = item.itemName;
        }
    }

    private void HandleItemUnequipped(EquipSlot slot)
    {
        // 해당 슬롯의 장비가 해제되었을 때 UI를 갱신합니다.
        if (slot == slotType)
        {
            // 아이콘, 이름 등을 초기화합니다.
            iconImage.sprite = null;
            nameLabel.text = "";
        }
    }
}
