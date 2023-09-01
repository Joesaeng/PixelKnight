using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MenuUI
{
    Inventory inventory;
    public GameObject discardUIPanel;
    public List<Slot> slots;
    public EquipmentSlotUI[] equipmentSlots;
    public Transform slotHolder;
    public Transform equipmentUI;

    private void Start()
    {
        inventory = Inventory.Instance;
        Slot[] tSlots = slotHolder.GetComponentsInChildren<Slot>();
        slots.AddRange(tSlots);
        equipmentSlots = equipmentUI.GetComponentsInChildren<EquipmentSlotUI>();
        foreach (EquipmentSlotUI eqSlot in equipmentSlots)
        {
            eqSlot.Init();
        }
        inventory.onSlotCountChange += SlotChange;
        inventory.onChangeItem += RedrawSlotUI;
        SlotChange(inventory.SlotCnt);
        RedrawSlotUI();
        menuPanel.SetActive(activeMenu);
        InputSystem.Instance.OnInventoryMenu += KeyInputAtiveMenu;
    }

    private void SlotChange(int val) // val == 인벤토리의 활성화된 슬롯의 개수
    {
        if(slots.Count < val) // Load 인벤토리 데이터를 위한 조건문
        {
            while(slots.Count < val) // 현재 인벤토리UI의 슬롯의 개수가 인벤토리의 활성화된 슬롯의 개수보다 작다면
                                     // 슬롯을 생성해서 인벤토리 UI에 추가한다.
            {
                GameObject newSlot = PoolManager.Instance.Get(PoolType.Slot);
                newSlot.transform.SetParent(slotHolder);
                newSlot.transform.localScale = new Vector3(1f, 1f, 1f);
                slots.Add(newSlot.GetComponent<Slot>());
            }
        }
        for (int i = 0; i < slots.Count; ++i) // 현재 슬롯의 개수만큼 돌면서
        {
            slots[i].slotNum = i;
            if (i < val) // 인벤토리의 활성화된 슬롯의 개수만큼 슬롯의 버튼을 활성화시킴
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
        GameObject selectItemUI = SelectItemUI.Instance.selectItemPanel;
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
                newSlot.transform.SetParent(slotHolder);
                newSlot.transform.localScale = new Vector3(1f, 1f, 1f);
                slots.Add(newSlot.GetComponent<Slot>());
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
