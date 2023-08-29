using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLevelText : MonoBehaviour
{
    public Text lvText;
    public void InitLevelText()
    {
        UpdateLvText();
        GameManager.Instance.player.playerStatus.OnLevelUp += UpdateLvText;
    }
    void UpdateLvText()
    {
        lvText.text = string.Format("Lv " + (GameManager.Instance.player.playerStatus.playerLv + 1));
    }
}
