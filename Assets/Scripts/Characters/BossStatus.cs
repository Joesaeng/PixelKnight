using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : EnemyStatus
{
    Boss boss;
    protected override void Awake()
    {
        boss = GetComponent<Boss>();
    }
    protected override void Start()
    {
        base.Start();
        SetData(2);
    }
    protected override void OnDisable()
    {
        if (boss.IsDead())
        {
            GameManager.Instance.player.playerStatus.ModifyExp(expReward,true);
            GameManager.Instance.ModifyGold(goldReward,true);
        }
    }
    public override void SetData(int _enemyID)
    {
        EnemyData tempdata = DataManager.Instance.GetEnemyData(_enemyID);
        if (tempdata is BossData data)
        {
            this.enemyID = _enemyID;
            this.charName = data.charName;
            this.expReward = data.expReward;
            this.goldReward = data.goldReward;
            this.attackDelay = data.attackDelay;
            this.maxHp = data.maxHp;
            this.maxPoise = data.maxPoise;
            this.damage = data.damage;
            this.defence = data.defence;
            this.stagger = data.stagger;
            this.evade = data.evade;
            this.hitrate = data.hitrate;
            this.moveSpeed = data.moveSpeed;
            this.bulletName = data.bulletName;

            this.curHp = maxHp;
            this.curPoise = maxPoise;

            boss.SetData(_enemyID);
        }
        else
            Debug.LogError("보스 데이터에 잘못된 값 할당");
    }
    public override void ModifyPoise(float value)
    {
        if (curPoise <= 0) return;
        curPoise -= value;

        if (curPoise <= 0f)
        {
            curPoise = 0f;
            boss.Stun();
        }
    }

}

