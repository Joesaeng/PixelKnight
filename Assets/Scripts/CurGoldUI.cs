using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurGoldUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Start()
    {
        UpdateGold();
        GameManager.Instance.OnChangedGold += UpdateGold;
    }

    private void UpdateGold()
    {
        text.text = GameManager.Instance.GetCurGold().ToString();
    }
}
