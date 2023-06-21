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
        // Equipment ������Ʈ�� �̺�Ʈ�� �����մϴ�.
        Equipment equipment = GetComponentInParent<Equipment>();
        if (equipment != null)
        {
            equipment.OnEquiped += HandleItemEquipped;
            equipment.OnUnEquiped += HandleItemUnequipped;
        }
    }

    private void OnDisable()
    {
        // Equipment ������Ʈ�� �̺�Ʈ ������ �����մϴ�.
        Equipment equipment = GetComponentInParent<Equipment>();
        if (equipment != null)
        {
            equipment.OnEquiped -= HandleItemEquipped;
            equipment.OnUnEquiped -= HandleItemUnequipped;
        }
    }

    private void HandleItemEquipped(EquipSlot slot, Equip item)
    {
        // �ش� ���Կ� ��� �����Ǿ��� �� UI�� �����մϴ�.
        if (slot == slotType)
        {
            // ������, �̸� ���� ������Ʈ�մϴ�.
            iconImage.sprite = item.itemImage;
            nameLabel.text = item.itemName;
        }
    }

    private void HandleItemUnequipped(EquipSlot slot)
    {
        // �ش� ������ ��� �����Ǿ��� �� UI�� �����մϴ�.
        if (slot == slotType)
        {
            // ������, �̸� ���� �ʱ�ȭ�մϴ�.
            iconImage.sprite = null;
            nameLabel.text = "";
        }
    }
}
