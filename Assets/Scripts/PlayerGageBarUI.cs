using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerGageBarUI : MonoBehaviour
{
    [SerializeField]
    private Slider sliderHp;
    [SerializeField]
    private Slider sliderStm;
    [SerializeField]
    private Slider sliderExp;
    private PlayerStatus playerStatus;
    public void InitGageBarUI()
    {
        playerStatus = GameManager.Instance.player.playerStatus;
    }

    private void Update()
    {
        if (sliderHp != null)
        {
            if (playerStatus != null)
            {
                sliderHp.value = Utils.Percent(playerStatus.dPlayerDynamicStatus[DynamicStatusName.CurHp], playerStatus.dPlayerFixedStatus[FixedStatusName.MaxHp]);
                sliderStm.value = Utils.Percent(playerStatus.dPlayerDynamicStatus[DynamicStatusName.CurStamina], playerStatus.dPlayerFixedStatus[FixedStatusName.MaxStamina]);
                //sliderExp.value = Utils.Percent(playerStatus.dPlayerDynamicStatus[DynamicStatusName.CurExp], playerStatus.dPlayerFixedStatus[FixedStatusName.MaxHp]);
            }
        }

    }
}
