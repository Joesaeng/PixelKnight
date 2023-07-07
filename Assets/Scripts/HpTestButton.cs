using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpTestButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HpTest);
    }

    void HpTest()
    {
        GameManager.Instance.player.playerStatus.ModifyHp(-10f);
    }
}
