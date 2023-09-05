using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentSlot : MonoBehaviour, IPointerUpHandler
{
    public EquipSlot slotType;                  // ���� �� ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� ����ϴ� ��� ����
    public Equip equip;                         // ���Կ� ����ִ� ���
    public Image iconImage;
    private bool isEquipEventSunscribed = false;    // �÷��̾��� Equipment�� �̺�Ʈ�� ���� �ߴ��� ����

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
        if (!Equipment.Instance.equippedItems.ContainsKey(slotType)) // �ε��� �����Ϳ� ��� ����� �ִ��� ����
            return;                                                  // ������ ���� �����ߴ��� ��������� ������ ���� �ʰ� ����� ����

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
        // ���� ���Կ� ��� �Ǿ��ִ����� Ȯ���ϰ�
        if (!iconImage.enabled)
        {
            // ��� �ȵǾ��ִٸ� ���������� ���� null�� �о��ݴϴ�.
            iconImage.sprite = null;
            equip = null;
            return;
        }

        Inventory.Instance.AddItem(equip);          // �κ��丮�� ���� �������ִ� �������� �ְ�
        Equipment.Instance.UnequipItem(slotType);   // ��� �����մϴ�
    }
}
