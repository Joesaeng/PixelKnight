using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumEft : ItemEffect
{
    // 소모성 아이템의 사용효과의 클래스입니다.
    public Consumable consumable;
    public void SetConsumableInfo(Consumable Consumable)
    {
        this.consumable = Consumable;
    }
    public override bool ExecuteRole()
    {
        switch (this.consumable.consumableType)
        {
            case ConsumableType.HpRecovery:
                {
                    GameManager.Instance.playerStatus.HpRecovery(consumable.value);
                    GameManager.Instance.HpPotionUse();
                }
                break;
            case ConsumableType.PowerEnforce:
                break;
            case ConsumableType.SpeedEnforce:
                break;
        }
        return true;
    }
}
