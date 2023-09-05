using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DiscardInBulk : MonoBehaviour
{
    // 인벤토리의 장비들을 일괄 파괴하는 UI 입니다.
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
                discardItemsIndexs.Add(i); // Toggles에서 itemLevel이 할당되어있는 체크박스가
                                           // On되어있는지 확인하여 discardItemsIndexs에 넣습니다.
            }
        }
        if(discardItemsIndexs.Count > 0)    // 삭제할 아이템이 있는 경우
        {
            for (int i = discardItemsIndexs.Count - 1 ; i >= 0; --i) 
            {
                int indexToRemove = discardItemsIndexs[i];
                Inventory.Instance.RemoveItem(indexToRemove); // 인벤토리의 뒤에서부터 삭제하여 오류를 방지합니다.
            }
        }
        uiPanel.SetActive(false);
    }
    public void Cancel()
    {
        uiPanel.SetActive(false);
    }
}
