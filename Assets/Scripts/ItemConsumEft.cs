using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumEft : ItemEffect
{
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
