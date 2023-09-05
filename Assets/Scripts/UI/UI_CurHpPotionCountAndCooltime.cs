using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurHpPotionCountAndCooltime : MonoBehaviour
{
    // 현재 HP물약의 개수와 쿨타임을 표시하는 UI 입니다.
    public Image hideImage;
    public Text count;

    Item potion;
    Inventory inventory;
    GameManager gameManager;
    private void Start()
    {
        count.text = "0";
        inventory = Inventory.Instance;
        inventory.onCurPotionUI += OnCount;
        inventory.offCurPotionUI += OffCount;
        gameManager = GameManager.Instance;
        SetPotion();
    }
    private void Update()
    {
        CoolTimeUpdate();
    }
    void CoolTimeUpdate()
    {
        // 물약의 쿨타임은 Gamemanager에서 관리하고 Update에서 읽어와서 UI를 갱신합니다.
        hideImage.fillAmount = gameManager.GetHpPotionCooltime();
    }
    void SetPotion()
    {
        if(inventory.GetHpPotion() != null)
        {
            OnCount(inventory.GetHpPotion());
        }
    }
    private void UpdateCount()
    {
        count.text = inventory.countItems[potion].ToString();
    }
    private void OnCount(Item item)
    {
        // 인벤토리에 HP물약이 들어왔을때 UpdateCount 이벤트를 구독합니다.
        potion = item;
        UpdateCount();
        inventory.onUpdateCount += UpdateCount;
    }
    private void OffCount()
    {
        // 포션의 개수가 0이 됬을 때 UpdateCount 이벤트의 구독을 취소합니다.
        potion = null;
        count.text = "0";
        inventory.onUpdateCount -= UpdateCount;
    }
}
