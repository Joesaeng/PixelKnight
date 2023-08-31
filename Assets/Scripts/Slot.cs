using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotNum;
    public Item item = null;
    public Image itemIcon;
    public Text itemCountText;

    bool onCount = false;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        if (item.itemType == ItemType.Consumable)
        {
            itemCountText.enabled = true;
            itemCountText.gameObject.SetActive(true);
            Inventory.Instance.onUpdateCount += UpdateCountText;
            onCount = true;
            UpdateCountText();
            if(item is Consumable con)
            {
                if(con.consumableType == ConsumableType.HpRecovery)
                {
                    Inventory.Instance.hpPotionSlot = slotNum;
                }
            }
        }
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
        if(onCount)
        {
            Inventory.Instance.onUpdateCount -= UpdateCountText;
            itemCountText.enabled = false;
            itemCountText.gameObject.SetActive(false);
            onCount = false;
        }
    }

    public void UpdateCountText()
    {
        itemCountText.text = Inventory.Instance.countItems[item].ToString();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (item == null) return;
        SelectItemUI.Instance.SetItem(slotNum);
    }
}
