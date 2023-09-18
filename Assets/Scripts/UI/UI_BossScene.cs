using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossScene : MonoBehaviour
{
    [SerializeField]
    Slider bossHpBar;
    [SerializeField]
    Slider bossPoiseBar;
    [SerializeField]
    Text bossHpText;
    [SerializeField]
    Text bossNameText;
    [SerializeField]
    BossStatus bossStatus;

    public void Init(EnemyData data)
    {
        bossNameText.text = data.charName;
    }
    private void Update()
    {
        if (bossStatus == null) return;
        bossHpBar.value = Utils.Percent(bossStatus.CurHp, bossStatus.maxHp);
        bossHpText.text = string.Format("{0:F0} / {1:F0}",bossStatus.CurHp,bossStatus.maxHp);
        bossPoiseBar.value = Utils.Percent(bossStatus.CurPoise,bossStatus.maxPoise);
    }
}
