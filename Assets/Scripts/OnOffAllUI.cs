using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffAllUI : MonoBehaviour
{
    InventoryUI inventoryUI;
    StatusUI statusUI;
    private void Awake()
    {
        inventoryUI = GetComponent<InventoryUI>();
        statusUI = GetComponent<StatusUI>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(inventoryUI.activeInventory)
            {
                inventoryUI.ActiveInventory();
            }
            if(statusUI.activeStatusUI)
            {
                statusUI.ActiveStatusUI();
            }
        }
    }
}
