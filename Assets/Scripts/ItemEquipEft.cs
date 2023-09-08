using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipEft : ItemEffect
{
    // 장비아이템의 장착을 위한 클래스입니다.
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
