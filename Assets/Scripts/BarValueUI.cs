using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarValueUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private EnemyStatus enemyStatus;
    public void InitEnmeyUI(EnemyStatus _enemyStatus)
    {
        enemyStatus = _enemyStatus;
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
        }

    }

}
