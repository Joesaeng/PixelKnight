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
        // 해당 슬롯에 장비가 장착되었을 때 UI를 갱신합니다.
        if (slot == slotType)
        {
            // 아이콘, 이름 등을 업데이트합니다.
            iconImage.sprite = _equip.itemImage;
            equip = _equip;
            iconImage.enabled = true;
        }
    }

    private void HandleItemUnequipped(EquipSlot slot)
    {
        // 해당 슬롯의 장비가 해제되었을 때 UI를 갱신합니다.
        if (slot == slotType)
        {
            // 아이콘, 이름 등을 초기화합니다.
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
