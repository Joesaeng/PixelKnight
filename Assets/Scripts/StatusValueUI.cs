using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusValueUI : MonoBehaviour
{
    PlayerStatus playerStatus;
    public enum StatusNameUI
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
        textValue[(int)StatusNameUI.vit].text = string.Format("{0:F0}", playerStatus.dPlayerAttribute[PlayerAttribute.Vitality]);
        textValue[(int)StatusNameUI.end].text = string.Format("{0:F0}", playerStatus.dPlayerAttribute[PlayerAttribute.Endurance]);
        textValue[(int)StatusNameUI.str].text = string.Format("{0:F0}", playerStatus.dPlayerAttribute[PlayerAttribute.Strength]);
        textValue[(int)StatusNameUI.dex].text = string.Format("{0:F0}", playerStatus.dPlayerAttribute[PlayerAttribute.Dexterity]);
        textValue[(int)StatusNameUI.luk].text = string.Format("{0:F0}", playerStatus.dPlayerAttribute[PlayerAttribute.Luck]);
        textValue[(int)StatusNameUI.stats].text = string.Format(
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
            "FIND: {12:F0}%",
            playerStatus.dPlayerStatus[StatusName.MaxHp],
            playerStatus.dPlayerStatus[StatusName.Defence],
            playerStatus.dPlayerStatus[StatusName.MaxStamina],
            playerStatus.dPlayerStatus[StatusName.Poise],
            playerStatus.dPlayerStatus[StatusName.Damage],
            playerStatus.dPlayerStatus[StatusName.Stagger],
            playerStatus.dPlayerStatus[StatusName.HitRate],
            playerStatus.dPlayerStatus[StatusName.Evade],
            playerStatus.dPlayerStatus[StatusName.AttackSpeed] * 100f,
            playerStatus.dPlayerStatus[StatusName.MoveSpeed] * 100f / playerStatus.dPlayerStatus[StatusName.MinMoveSpeed],
            playerStatus.dPlayerStatus[StatusName.CriticalChance] * 100f,
            playerStatus.dPlayerStatus[StatusName.CriticalHitDamage] * 100f,
            playerStatus.dPlayerStatus[StatusName.IncreasedItemFindingChance] * 100f);


        Debug.Log("스탯UI 업데이트");
    }
}
