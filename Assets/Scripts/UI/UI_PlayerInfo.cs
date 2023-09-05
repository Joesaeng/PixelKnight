using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private Slider sliderHp;
    [SerializeField]
    private Slider sliderStm;
    [SerializeField]
    private Slider sliderExp;
    [SerializeField]
    private Text lvText;
    private PlayerStatus playerStatus;
    public void InitLevelText()
    {
        UpdateLvText();
        GameManager.Instance.player.playerStatus.OnLevelUp += UpdateLvText;
    }
    void UpdateLvText()
    {
        lvText.text = string.Format("Lv " + (GameManager.Instance.player.playerStatus.playerLv + 1));
    }
    public void InitPlayerInfo()
    {
        playerStatus = GameManager.Instance.player.playerStatus;
        UpdateLvText();
        playerStatus.OnLevelUp += UpdateLvText;
    }

    private void Update()
    {
        if (sliderHp != null)
        {
            if (playerStatus != null)
            {
                sliderHp.value = Utils.Percent(playerStatus.dPlayerDynamicStatus[DynamicStatusName.CurHp], playerStatus.dPlayerFixedStatus[FixedStatusName.MaxHp]);
                sliderStm.value = Utils.Percent(playerStatus.dPlayerDynamicStatus[DynamicStatusName.CurStamina], playerStatus.dPlayerFixedStatus[FixedStatusName.MaxStamina]);
                sliderExp.value = Utils.Percent(playerStatus.dPlayerDynamicStatus[DynamicStatusName.CurExp], playerStatus.ExpRequirement);
            }
        }

    }
}
