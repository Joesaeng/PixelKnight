using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldConsumable : FieldItems
{
    public Consumable consumable;

    public override void Awake()
    {
        base.Awake();
        consumable = new Consumable();
    }

    public void SetConsumable(Consumable consumable)
    {
        this.consumable = consumable;
        SetItem();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Inventory.Instance.AddItem(consumable))
            {
                DestroyItem();
            }
        }
    }
    public override void SetImage()
    {
        image.sprite = consumable.itemImage;
        StartCoroutine(FadeIn());
    }

    public Consumable GetItem()
    {
        return consumable;
    }
}
