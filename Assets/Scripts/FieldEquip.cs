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
    public Equip GetItem()
    {
        return equip;
    }
    public void DestroyEquip()
    {
        Destroy(gameObject);
    }
}
