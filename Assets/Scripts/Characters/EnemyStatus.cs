using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyStatus : MonoBehaviour
{
    /*
     * Enemy�� �������� �������ͽ��� ����ϴ� Ŭ�����Դϴ�.
     */
    public Transform effectPoint;
    Enemy enemy;
    private string charName;
    public float expReward;
    public int goldReward;
    public int enemyID;

    public float attackDelay;
    public float maxHp;
    public float maxPoise;
    public float damage;
    public float defence;
    public float stagger;
    public float evade;
    public float hitrate;
    public float moveSpeed;
    public BulletName bulletName;

    private float curHp;
    private float curPoise;

    public Action OnEnemyDead;

    Spawner spawner;
    public float CurHp
    {
        get => curHp;
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        spawner = Spawner.instance;
    }
    public void SetData(int _enemyID) 
        // PoolManager���� Enemy�� ������ ȣ��Ǿ� �����մϴ�.
    {
        EnemyData data = DataManager.Instance.GetEnemyData(_enemyID);
        this.enemyID = _enemyID;
        this.charName   = data.charName;
        this.expReward = data.expReward;
        this.goldReward = data.goldReward;
        this.attackDelay = data.attackDelay;
        this.maxHp      = data.maxHp;
        this.maxPoise   = data.maxPoise;
        this.damage     = data.damage;
        this.defence    = data.defence;
        this.stagger    = data.stagger;
        this.evade      = data.evade;
        this.hitrate    = data.hitrate;
        this.moveSpeed  = data.moveSpeed;
        this.bulletName = data.bulletName;

        this.curHp = maxHp;
        this.curPoise = maxPoise;
        
        enemy.SetData(_enemyID);
    }
    public void ResetHpPoise()
    {
        this.curHp = maxHp;
        this.curPoise = maxPoise;
    }    
    public void ModifyHp(float value)
    {
        curHp += value;
        if(curHp <= 0)
        {
            OnEnemyDead?.Invoke();
            curHp = 0;
            GameManager.Instance.player.playerStatus.ModifyExp(expReward);
            GameManager.Instance.ModifyGold(goldReward);
        }
    }
    void ModifyPoise(float value)
    {
        curPoise += value;
    }
    public bool CalculatedHit(PlayerStatus playerStatus)
        // �ǰ��� ����ϴ� �޼����Դϴ�.
    {
        CalculatedDamage caldmg = playerStatus.CalculateDamage();
        float hitDiff = playerStatus.GetHitDiff(evade);
        return FinalCalculatedHit(caldmg.damage, hitDiff, caldmg.option == DamageOption.Critical ? true : false);
    }
    public bool CalculatedHit(PlayerStatus playerStatus, SkillData skillData)
        // ��ų �ǰ��� ����ϴ� �޼����Դϴ�.
    {
        CalculatedDamage caldmg = playerStatus.CalculateDamage();
        float hitDiff = playerStatus.GetHitDiff(evade);
        return FinalCalculatedHit(caldmg.damage * skillData.damageRatio, hitDiff,caldmg.option == DamageOption.Critical ? true : false);
    }
    public bool FinalCalculatedHit(float damage, float hitDiff, bool isCritical)
    {
        bool isHit = false;
        float hitRate = 0f;

        if (hitDiff > 25)
        {
            isHit = true;
            if (isCritical) // ũ��Ƽ�ÿ��η� ũ��Ƽ�� ����Ʈ�� ǥ�õ��� �ȵ��� �Ǵ��մϴ�.
            {
                spawner.ShowDamageEffect(0).transform.SetPositionAndRotation
                (effectPoint.position, Quaternion.identity);
            }
            spawner.ShowDamageEffect(2).transform.SetPositionAndRotation
                (transform.position, Quaternion.identity);
            TakeDamage(damage);
        }
        else if (hitDiff > 0) // ���߷��� ����մϴ�.
        {
            hitRate = hitDiff * 0.04f;
        }

        if (UnityEngine.Random.value < hitRate)
        {
            isHit = true;
            TakeDamage(damage);
            if (isCritical)
            {
                spawner.ShowDamageEffect(0).transform.SetPositionAndRotation
                (effectPoint.position, Quaternion.identity);
            }
            spawner.ShowDamageEffect(2).transform.SetPositionAndRotation
                (transform.position, Quaternion.identity);
        }
        if (!isHit)
        {
            spawner.ShowDamageEffect(1).transform.SetPositionAndRotation
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
        hpUI.GetComponent<UI_CharacterHeadBarValue>().InitEnemyStatus(this);
    }
}
