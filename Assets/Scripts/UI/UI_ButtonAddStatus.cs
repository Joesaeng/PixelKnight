using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_ButtonAddStatus : MonoBehaviour, IPointerUpHandler
{
    // 플레이어 스테이터스 창에서 할당된 스텟을 상승시키는 버튼 스크립트입니다.
    public PlayerAttribute index;

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerStatus playerStatus = GameManager.Instance.player.playerStatus;
        if (playerStatus.remainingPoint < 1) return;
        playerStatus.AddAttribute(index, 1);
        playerStatus.ModifyRemainingPoint(1,false);
    }
}
