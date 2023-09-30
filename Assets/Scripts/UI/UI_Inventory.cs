using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_WindowMenu
{
    Inventory inventory;
    public GameObject discardUIPanel;
    public List<UI_InventorySlot> slots;
    public UI_EquipmentSlot[] equipmentSlots;
    public Transform slotParent;
    public Transform equipmentUI;

    private void Start()
    {
        inventory = Inventory.Instance;
        UI_InventorySlot[] tSlots = slotParent.GetComponentsInChildren<UI_InventorySlot>();
        slots.AddRange(tSlots);
        equipmentSlots = equipmentUI.GetComponentsInChildren<UI_EquipmentSlot>();
        foreach (UI_EquipmentSlot eqSlot in equipmentSlots)
        {
            eqSlot.Init();
        }
        inventory.onSlotCountChange += SlotChange;
        inventory.onChangeItem += RedrawSlotUI;
        SlotChange(inventory.SlotCnt);
        RedrawSlotUI();
        menuPanel.SetActive(activeMenu);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.InventoryMenu]))
            KeyInputAtiveMenu();
    }
    private void SlotChange(int val) // val == �κ��丮�� Ȱ��ȭ�� ������ ����
    {
        if(slots.Count < val) // Load �κ��丮 �����͸� ���� ���ǹ�
        {
            while(slots.Count < val) // ���� �κ��丮UI�� ������ ������ �κ��丮�� Ȱ��ȭ�� ������ �������� ���ٸ�
                                     // ������ �����ؼ� �κ��丮 UI�� �߰��Ѵ�.
            {
                GameObject newSlot = PoolManager.Instance.Get(PoolType.Slot);
                newSlot.transform.SetParent(slotParent);
                newSlot.transform.localScale = new Vector3(1f, 1f, 1f);
                slots.Add(newSlot.GetComponent<UI_InventorySlot>());
            }
        }
        for (int i = 0; i < slots.Count; ++i) // ���� ������ ������ŭ ���鼭
        {
            slots[i].slotNum = i;
            if (i < val) // �κ��丮�� Ȱ��ȭ�� ������ ������ŭ ������ ��ư�� Ȱ��ȭ��Ŵ
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public override void ActiveMenu()
    {
        activeMenu = !activeMenu;
        menuPanel.SetActive(activeMenu);
        GameObject selectItemUI = UI_SelectItemDesc.Instance.selectItemPanel;
        if (selectItemUI && selectItemUI.activeSelf)
            selectItemUI.SetActive(false);
        if (discardUIPanel && discardUIPanel.activeSelf)
            discardUIPanel.SetActive(false);
    }
    public override void KeyInputAtiveMenu()
    {
        ActiveMenu();
    }
    public void AddSlot()
    {
        inventory.SlotCnt++;
        if (slots.Count < inventory.SlotCnt)
        {
            for (int i = 0; i < 4; ++i)
            {
                GameObject newSlot = PoolManager.Instance.Get(PoolType.Slot);
                newSlot.transform.SetParent(slotParent);
                newSlot.transform.localScale = new Vector3(1f, 1f, 1f);
                slots.Add(newSlot.GetComponent<UI_InventorySlot>());
            }
            SlotChange(inventory.SlotCnt);
            RedrawSlotUI();
        }
    }

    void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];
            slots[i].UpdateSlotUI();
        }
    }
}
