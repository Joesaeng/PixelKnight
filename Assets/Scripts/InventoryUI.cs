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
        RedrawSlotUI();
        menuPanel.SetActive(activeMenu);
        InputSystem.Instance.OnInventoryMenu += KeyInputAtiveMenu;
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            slots[i].slotNum = i;
            if (i < inventory.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
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
