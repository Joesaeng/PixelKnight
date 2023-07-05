using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEquip : MonoBehaviour
{
    public Equip equip;
    public SpriteRenderer image;
    private void Awake()
    {
        equip = new Equip();
    }
    public void SetEquip(Equip equip)
    {
        this.equip = equip;
        image.sprite = equip.itemImage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Inventory.Instance.AddItem(equip))
            {
                DestroyEquip();
            }
        }
    }
    public Equip GetItem()
    {
        return equip;
    }
    public void DestroyEquip()
    {
        Destroy(gameObject);
    }
}
