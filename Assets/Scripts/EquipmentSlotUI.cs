using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerUpHandler
{
    public EquipSlot slotType;
    public Equip equip;
    public Image iconImage;
    private bool isEquipEventSunscribed = false;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if (Equipment.Instance != null && !isEquipEventSunscribed)
        {
            Equipment.Instance.OnEquiped += HandleItemEquipped;
            Equipment.Instance.OnUnEquiped += HandleItemUnequipped;
            isEquipEventSunscribed = true;
            LoadEquip();
        }
    }
    void LoadEquip()
    {
        if (!Equipment.Instance.equippedItems.ContainsKey(slotType)) return;
        Equip _equip = Equipment.Instance.equippedItems[slotType];
        iconImage.sprite = _equip.itemImage;
        equip = _equip;
        iconImage.enabled = true;
    }

    private void HandleItemEquipped(EquipSlot slot, Equip _equip)
    {
        // �ش� ���Կ� ��� �����Ǿ��� �� UI�� �����մϴ�.
        if (slot == slotType)
        {
            // ������, �̸� ���� ������Ʈ�մϴ�.
            iconImage.sprite = _equip.itemImage;
            equip = _equip;
            iconImage.enabled = true;
        }
    }

    private void HandleItemUnequipped(EquipSlot slot)
    {
        // �ش� ������ ��� �����Ǿ��� �� UI�� �����մϴ�.
        if (slot == slotType)
        {
            // ������, �̸� ���� �ʱ�ȭ�մϴ�.
            iconImage.sprite = null;
            equip = null;
            iconImage.enabled = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!iconImage.enabled)
        {
            iconImage.sprite = null;
            equip = null;
            return;
        }
        Inventory.Instance.AddItem(equip);
        Equipment.Instance.UnequipItem(slotType);
    }
}
