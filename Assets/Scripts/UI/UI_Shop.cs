using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
    public void BuyPotion()
    {
        if (GameManager.Instance.curGold >= 150)
        {
            if (Inventory.Instance.AddItem(ItemDataBase.Instance.GetConsumableData(5)))
            {
                GameManager.Instance.ModifyGold(150, false);
            }
            else
            {
                UI_CenterPopupText.instance.SetPopupText
                    ("인벤토리 슬롯이 부족합니다.");
            }
        }
        else
        {
            UI_CenterPopupText.instance.SetPopupText
                    ("소지한 골드가 부족합니다.");
        }
    }
    public void BuyEquip()
    {
        if (GameManager.Instance.curGold >= 250)
        {
            int randomIndex = Random.Range(0, 4);
            Equip equip = new Equip();
            equip.SetItemData(ItemDataBase.Instance.GetEquipData(randomIndex));
            float ran = UnityEngine.Random.value;

            if (ran <= 0.05f)
                equip.LevelUpItem(ItemLevel.Unique);
            else if (ran <= 0.15f)
                equip.LevelUpItem(ItemLevel.Rare);
            else if (ran <= 0.35f)
                equip.LevelUpItem(ItemLevel.Advanced);
            else
                equip.LevelUpItem(ItemLevel.Common);

            if(Inventory.Instance.AddItem(equip))
            {
                GameManager.Instance.ModifyGold(250, false);
            }
            else
            {
                UI_CenterPopupText.instance.SetPopupText
                    ("인벤토리 슬롯이 부족합니다.");
            }
        }
        else
        {
            UI_CenterPopupText.instance.SetPopupText
                    ("소지한 골드가 부족합니다.");
        }
    }
}
