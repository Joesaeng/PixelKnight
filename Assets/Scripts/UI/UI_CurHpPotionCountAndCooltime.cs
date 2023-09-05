using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurHpPotionCountAndCooltime : MonoBehaviour
{
    // ���� HP������ ������ ��Ÿ���� ǥ���ϴ� UI �Դϴ�.
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
        // ������ ��Ÿ���� Gamemanager���� �����ϰ� Update���� �о�ͼ� UI�� �����մϴ�.
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
        // �κ��丮�� HP������ �������� UpdateCount �̺�Ʈ�� �����մϴ�.
        potion = item;
        UpdateCount();
        inventory.onUpdateCount += UpdateCount;
    }
    private void OffCount()
    {
        // ������ ������ 0�� ���� �� UpdateCount �̺�Ʈ�� ������ ����մϴ�.
        potion = null;
        count.text = "0";
        inventory.onUpdateCount -= UpdateCount;
    }
}
