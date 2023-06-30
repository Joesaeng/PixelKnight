using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipEft : ItemEffect
{
    public Equip equip;
    public void SetEquipInfo(Equip _equip)
    {
        equip = _equip;
        
    }
    public override bool ExecuteRole()
    {
        Equipment.Instance.EquipItem(equip.equipSlot, equip);
        return true;
    }
}
