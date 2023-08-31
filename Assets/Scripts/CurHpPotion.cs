using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurHpPotion : MonoBehaviour
{
    public Image hideImage;
    public Text count;
    Item potion;
    Inventory inventory;
    private void Start()
    {
        count.text = "0";
        inventory = Inventory.Instance;
        inventory.onCurPotionUI += OnCount;
        inventory.offCurPotionUI += OffCount;
        SetPotion();
    }
    private void Update()
    {
        CoolTimeUpdate();
    }
    void CoolTimeUpdate()
    {
        hideImage.fillAmount = GameManager.Instance.GetHpPotionCooltime();
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
        potion = item;
        UpdateCount();
        inventory.onUpdateCount += UpdateCount;
    }
    private void OffCount()
    {
        potion = null;
        count.text = "0";
        inventory.onUpdateCount -= UpdateCount;
    }
}
