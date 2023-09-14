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
    public override void SetData(int _enemyID)
    {
        EnemyData data = DataManager.Instance.GetEnemyData(_enemyID);
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
    public override void ModifyPoise(float value)
    {
        curPoise -= value;
        Debug.Log(curPoise);
        if (curPoise <= 0)
        {
            curPoise = maxPoise;
            boss.Stun();
        }
    }
    /*
     * 보스전 UI에 Hp와 Poise 넣어주기
     * 
     * 
     */
}
