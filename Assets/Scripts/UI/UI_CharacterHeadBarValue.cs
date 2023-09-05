using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_CharacterHeadBarValue : MonoBehaviour
{
    // 캐릭터 머리를 따라다니는 Bar의 값을 설정하는 스크립트입니다.
    [SerializeField]
    private Slider slider;
    [SerializeField]
    Image fill;
    Color fillColor = new Color(142f/255f,0f,0f);

    private EnemyStatus enemyStatus;
    private PlayerStatus playerStatus;
    public void InitEnemyStatus(EnemyStatus _enemyStatus)
    {
        enemyStatus = _enemyStatus;
        fill.color = fillColor;
    }
    public void InitPlayerStatus()
    {
        playerStatus = GameManager.Instance.player.playerStatus;
        fill.color = new Color(130f / 255f, 130f / 255f, 130f / 255f);
    }
    private void Update()
    {
        if (slider != null)
        {
            if(enemyStatus != null)
            {
                slider.value = Utils.Percent(enemyStatus.CurHp, enemyStatus.maxHp);
                if (slider.gameObject.activeSelf && enemyStatus.CurHp <= 0f) slider.gameObject.SetActive(false);
                else if (!slider.gameObject.activeSelf && enemyStatus.CurHp >0f) slider.gameObject.SetActive(true);
            }
            if(playerStatus != null)
            {
                slider.value = Utils.Percent(playerStatus.dPlayerDynamicStatus[DynamicStatusName.CurPoise], playerStatus.dPlayerFixedStatus[FixedStatusName.Poise]);
            }
        }

    }

}
