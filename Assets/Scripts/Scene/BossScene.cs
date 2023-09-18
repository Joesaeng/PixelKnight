using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : PlayScene
{
    [SerializeField]
    UI_BossScene ui;
    [SerializeField]
    int bossId;
    protected override void Awake()
    {
        base.Awake();
        EnemyData data = DataManager.Instance.GetEnemyData(bossId);
        ui.Init(data);
    }
}
