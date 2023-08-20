using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardInBulkUI : MonoBehaviour
{
    public GameObject uiPanel;
    public Transform togglesParent;
    Toggle[] toggles;
    bool firstOpen = false;
    public void Init()
    {
        toggles = togglesParent.GetComponentsInChildren<Toggle>();
    }
    private void OnEnable()
    {
        if (!firstOpen) Init();
    }
    public void ActiveUI()
    {
        uiPanel.SetActive(true);
        if (SelectItemUI.Instance.selectItemPanel.activeSelf)
            SelectItemUI.Instance.selectItemPanel.SetActive(false);
    }
    public void Discard()
    {
        List<int> discardItemsIndexs = new();
        List<Item> discardItems = Inventory.Instance.GetItems();
        for(int i = 0; i < discardItems.Count; ++i)
        {
            if(toggles[(int)discardItems[i].itemLevel].isOn)
            {
                discardItemsIndexs.Add(i);
            }
        }
        if(discardItemsIndexs.Count > 0)
        {
            for (int i = discardItemsIndexs.Count - 1 ; i >= 0; --i)
            {
                int indexToRemove = discardItemsIndexs[i];
                Inventory.Instance.RemoveItem(indexToRemove);
            }
        }
        uiPanel.SetActive(false);
    }
    public void Cancel()
    {
        uiPanel.SetActive(false);
    }
}
