using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpc : NPC
{
    [SerializeField]
    GameObject shopUI;
    protected override void Interaction()
    {
        shopUI.SetActive(true);
        FindObjectOfType<UI_Inventory>(true).ActiveMenu();
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteraction = false;
            npcNameObj.SetActive(false);
            shopUI.SetActive(false);
        }
    }
}
