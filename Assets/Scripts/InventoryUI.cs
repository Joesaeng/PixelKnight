using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    public GameObject inventoryPanel;
    public bool activeInventory = false;

    public Slot[] slots;
    public EquipmentSlotUI[] equipmentSlots;
    public Transform slotHolder;
    public Transform equipmentUI;

    private void Start()
    {
        inventory = Inventory.Instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentUI.GetComponentsInChildren<EquipmentSlotUI>();
        foreach(EquipmentSlotUI eqSlot in equipmentSlots)
        {
            eqSlot.Init();
        }
        inventory.onSlotCountChange += SlotChange;
        inventory.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(activeInventory);
    }

    private void SlotChange(int val)
    {
        for(int i = 0; i < slots.Length; ++i)
        {
            slots[i].slotNum = i;
            if (i < inventory.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void ActiveInventory()
    {
        activeInventory = !activeInventory;
        inventoryPanel.SetActive(activeInventory);
        if (SelectItemUI.Instance.selectItemPanel.activeSelf)
            SelectItemUI.Instance.selectItemPanel.SetActive(false);
    }
    public void AddSlot()
    {
        inventory.SlotCnt++;
    }

    void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
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
