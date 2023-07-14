using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyStatus : MonoBehaviour
{
    public Transform effectPoint;
    EnemyData data;
    public int enemyID;
    private string charName;
    public float expReward;
    GameObject hpBarUI;

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

    public Action OnEnemyDead;

    private bool init = false;
    public float CurHp
    {
        get => curHp;
    }

    private void Awake()
    {
        data = DataManager.Instance.GetEnemyData(enemyID);
    }
    private void OnEnable()
    {
        SetData();
        init = true;
    }
    public void SetData()
    {
        this.charName   = data.charName;
        this.expReward = data.expReward;
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
    void ModifyHp(float value)
    {
        curHp += value;
        if(curHp <= 0)
        {
            OnEnemyDead?.Invoke();
            curHp = 0;
            //hpBarUI.SetActive(false);
        }
    }
    void ModifyPoise(float value)
    {
        curPoise += value;
    }
    public bool CalculatedHit(PlayerStatus playerStatus)
    {
        bool isHit = false;
        float hitRate = 0f;
        CalculatedDamage caldmg = playerStatus.CalculateDamage();
        bool isCritical = caldmg.option == DamageOption.Critical ? true : false;
        float hitDiff = playerStatus.dPlayerFixedStatus[FixedStatusName.HitRate] - evade;
        if (hitDiff > 25)
        {
            isHit = true;
            if (isCritical)
            {
                Spawner.instance.ShowDamageEffect(0).transform.SetPositionAndRotation
                (effectPoint.position,Quaternion.identity);
            }

            TakeDamage(caldmg.damage);
        }
        else if (hitDiff > 0)
        {
            hitRate = hitDiff * 0.04f;
        }

        if(UnityEngine.Random.value < hitRate)
        {
            isHit = true;
            TakeDamage(caldmg.damage);
            if (isCritical)
            {
                Spawner.instance.ShowDamageEffect(0).transform.SetPositionAndRotation
                (effectPoint.position, Quaternion.identity);
            }
        }
        if (!isHit)
        {
            Spawner.instance.ShowDamageEffect(1).transform.SetPositionAndRotation
            (effectPoint.position, Quaternion.identity);
        }
        return isHit;
    }
    void TakeDamage(float _damage)
    {
        ModifyHp(defence - _damage);
    }
    public void SetHPUI(GameObject hpUI)
    {
        hpUI.GetComponent<BarValueUI>().InitEnmeyUI(this);
        hpBarUI = hpUI;
    }
}
