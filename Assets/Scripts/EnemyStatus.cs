using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStatus : MonoBehaviour
{
    public int enemyID;
    private string charName;

    public float maxHp;
    public float maxPoise;
    public float damage;
    public float defence;
    public float stagger;
    public float evade;
    public float hitrate;
    public float moveSpeed;

    private float curHp;
    private float curPoise;


    private void Awake()
    {
        SetData(DataManager.Instance.GetEnemyData(enemyID));
    }
    void SetData(EnemyData data)
    {
        this.charName   = data.charName;
        this.maxHp      = data.maxHp;
        this.maxPoise   = data.maxPoise;
        this.damage     = data.damage;
        this.defence    = data.defence;
        this.stagger    = data.stagger;
        this.evade      = data.evade;
        this.hitrate    = data.hitrate;
        this.moveSpeed  = data.moveSpeed;

        this.curHp = maxHp;
        this.curPoise = maxPoise;
    }
}
