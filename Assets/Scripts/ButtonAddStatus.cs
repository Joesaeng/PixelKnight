using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonAddStatus : MonoBehaviour, IPointerUpHandler
{
    public PlayerAttribute index;

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerStatus playerStatus = GameManager.Instance.player.playerStatus;
        if (playerStatus.remainingPoint < 1) return;
        playerStatus.ModifyAttribute(index, 1);
        playerStatus.ModifyRemainingPoint(-1);
    }
}
