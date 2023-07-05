using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;

    private void Awake()
    {
        item = new Item();
    }
    public void SetItem(Item _item)
    {
        item = _item;
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;
        item.itemLevel = _item.itemLevel;
        item.efts = _item.efts;
        image.sprite = item.itemImage;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (Inventory.Instance.AddItem(item))
            {
                DestroyItem();
            }
        }
    }
    public Item GetItem()
    {
        return item;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
