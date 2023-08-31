using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEquip : FieldItems
{
    public Equip equip;
    public override void Awake()
    {
        base.Awake();
        equip = new Equip();
    }
    public void SetEquip(Equip equip)
    {
        this.equip = equip;
        SetItem();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Inventory.Instance.AddItem(equip))
            {
                DestroyItem();
            }
        }
    }
    public override void SetImage()
    {
        image.sprite = equip.itemImage;
        StartCoroutine(FadeIn());
    }
    public Equip GetItem()
    {
        return equip;
    }
}
