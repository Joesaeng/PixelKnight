using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusValueUI : MonoBehaviour
{
    PlayerStatus playerStatus;
    public enum StatusName
    {
        vit,end,str,dex,luk,stats
    }

    public TextMeshProUGUI[] textValue;
    public void InitStatusUI()
    {
        playerStatus = GameManager.Instance.player.playerStatus;
        UpdatePlayerStatsUI();
        playerStatus.OnStatsCalculated += UpdatePlayerStatsUI;
    }
    void UpdatePlayerStatsUI()
    {
        textValue[(int)StatusName.vit].text = string.Format("{0:F0}", playerStatus.vitality);
        textValue[(int)StatusName.end].text = string.Format("{0:F0}", playerStatus.endurance);
        textValue[(int)StatusName.str].text = string.Format("{0:F0}", playerStatus.strength);
        textValue[(int)StatusName.dex].text = string.Format("{0:F0}", playerStatus.dexterity);
        textValue[(int)StatusName.luk].text = string.Format("{0:F0}", playerStatus.luck);
        textValue[(int)StatusName.stats].text = string.Format(
            "HP  : {0:F0} \n" +
            "DEF : {1:F0}\n" +
            "STM : {2:F0}\n" +
            "POI : {3:F0}\n" +
            "DMG : {4:F0}\n" +
            "STG : {5:F0}\n" +
            "HIT : {6:F0}\n" +
            "EVA : {7:F0}\n" +
            "ASP : {8:F0}%\n" +
            "MSP : {9:F0}%\n" +
            "CRI : {10:F0}%\n" +
            "CDM : {11:F0}%\n" +
            "FIND: {12:F0}%",playerStatus.maxHp, playerStatus.defence, playerStatus.maxStamina, playerStatus.poise
            , playerStatus.damage, playerStatus.stagger, playerStatus.hitRate, playerStatus.evade,
            playerStatus.attackSpeed*100f, playerStatus.moveSpeed*100f/playerStatus.minMoveSpeed, 
            playerStatus.criticalChance*100f
            , playerStatus.criticalHitDamage*100f, playerStatus.increasedItemFindingChance*100f);


        Debug.Log("스탯UI 업데이트");
    }
}
