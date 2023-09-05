using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DiscardInBulk : MonoBehaviour
{
    // �κ��丮�� ������ �ϰ� �ı��ϴ� UI �Դϴ�.
    public GameObject uiPanel;
    public Transform togglesParent;

    Toggle[] toggles;

    bool firstOpen = false;
    public void Init()
    {
        toggles = togglesParent.GetComponentsInChildren<Toggle>();
        firstOpen = true;
    }
    private void OnEnable()
    {
        if (!firstOpen) Init();
    }
    public void ActiveUI()
    {
        uiPanel.SetActive(true);
        if (UI_SelectItemDesc.Instance.selectItemPanel.activeSelf)
            UI_SelectItemDesc.Instance.selectItemPanel.SetActive(false);
    }
    public void DiscardInBulk()
    {
        List<int> discardItemsIndexs = new();
        List<Item> allInventoryItems = Inventory.Instance.GetItems();
        for(int i = 0; i < allInventoryItems.Count; ++i)
        {
            if(toggles[(int)allInventoryItems[i].itemLevel].isOn)   
            {
                discardItemsIndexs.Add(i); // Toggles���� itemLevel�� �Ҵ�Ǿ��ִ� üũ�ڽ���
                                           // On�Ǿ��ִ��� Ȯ���Ͽ� discardItemsIndexs�� �ֽ��ϴ�.
            }
        }
        if(discardItemsIndexs.Count > 0)    // ������ �������� �ִ� ���
        {
            for (int i = discardItemsIndexs.Count - 1 ; i >= 0; --i) 
            {
                int indexToRemove = discardItemsIndexs[i];
                Inventory.Instance.RemoveItem(indexToRemove); // �κ��丮�� �ڿ������� �����Ͽ� ������ �����մϴ�.
            }
        }
        uiPanel.SetActive(false);
    }
    public void Cancel()
    {
        uiPanel.SetActive(false);
    }
}
