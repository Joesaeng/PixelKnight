using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentSlot : MonoBehaviour, IPointerUpHandler
{
    public EquipSlot slotType;                  // 지금 이 스크립트를 가지고 있는 오브젝트가 담당하는 장비 슬롯
    public Equip equip;                         // 슬롯에 담겨있는 장비
    public Image iconImage;
    private bool isEquipEventSunscribed = false;    // 플레이어의 Equipment의 이벤트를 구독 했는지 여부

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if (Equipment.Instance != null && !isEquipEventSunscribed)
        {
            Equipment.Instance.OnEquiped += ItemEquiped;
            Equipment.Instance.OnUnEquiped += ItemUnEquiped;
            isEquipEventSunscribed = true;
            LoadEquip();
        }
    }
    void LoadEquip()
    {
        if (!Equipment.Instance.equippedItems.ContainsKey(slotType)) // 로드한 데이터에 장비가 담겨져 있는지 여부
            return;                                                  // 게임을 새로 시작했더라도 비어있으면 오류가 나지 않게 만들기 위함

        Equip _equip = Equipment.Instance.equippedItems[slotType];

        iconImage.sprite = _equip.itemImage;
        iconImage.enabled = true;
        equip = _equip;
    }

    private void ItemEquiped(EquipSlot slot, Equip _equip)
    {
        if (slot == slotType)
        {
            iconImage.sprite = _equip.itemImage;
            equip = _equip;
            iconImage.enabled = true;
        }
    }

    private void ItemUnEquiped(EquipSlot slot)
    {
        if (slot == slotType)
        {
            iconImage.sprite = null;
            equip = null;
            iconImage.enabled = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // 현재 슬롯에 장비가 되어있는지를 확인하고
        if (!iconImage.enabled)
        {
            // 장비가 안되어있다면 오류방지를 위해 null로 밀어줍니다.
            iconImage.sprite = null;
            equip = null;
            return;
        }

        Inventory.Instance.AddItem(equip);          // 인벤토리에 현재 장착되있는 아이템을 넣고
        Equipment.Instance.UnequipItem(slotType);   // 장비를 해제합니다
    }
}
