using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_ButtonAddStatus : MonoBehaviour, IPointerUpHandler
{
    // �÷��̾� �������ͽ� â���� �Ҵ�� ������ ��½�Ű�� ��ư ��ũ��Ʈ�Դϴ�.
    public PlayerAttribute index;

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerStatus playerStatus = GameManager.Instance.player.playerStatus;
        if (playerStatus.remainingPoint < 1) return;
        playerStatus.AddAttribute(index, 1);
        playerStatus.ModifyRemainingPoint(1,false);
    }
}
