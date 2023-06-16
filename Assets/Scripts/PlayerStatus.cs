using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public delegate void StatsCalculatedDelegate();
    public event StatsCalculatedDelegate OnStatsCalculated;
    #region Player Attribute Allocation
    public int vitality;    // ü�� : MaxHp�� ����
    public int endurance;   // ������ : MaxStamina
    public int strength;    // �ٷ� : ���ݷ°� ������
    public int dexterity;   // �ⷮ : ���ݼӵ��� ȸ����
    public int luck;        // �� : ġ��ŸȮ���� ������ �����
    #endregion 
    #region Player Status
    private float maxHp;
    private float curHp;
    private float maxStamina;
    private float curStamina;
    private float defence;
    private float damage;
    private float stagger;
    private float minAttackSpeed;
    private float maxAttackSpeed;
    private float attackSpeed;
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    private float dodge;
    private float minMoveSpeed;
    private float maxMoveSpeed;
    private float moveSpeed;
    private readonly float minCriticalChance = 0.1f;
    private readonly float maxCriticalChance = 1f;
    private float criticalChance;
    private readonly float minCriticalHitDamage = 1.5f;
    private readonly float criticalHitDamage;
    private float increasedItemFindingChance;
    #endregion
    private void Awake()
    {
    }
    public void SetStatus(PlayerData data)
    {
        vitality = data.vitality;
        endurance = data.endurance;
        strength = data.strength;
        dexterity = data.dexterity;
        luck = data.luck;
        minAttackSpeed = data.minAttackSpeed;
        maxAttackSpeed = data.maxAttackSpeed;
        attackSpeed = minAttackSpeed;

        CalculateStats();
    }    


    private void CalculateStats()
    {
        maxHp = vitality * 20f;
        maxStamina = endurance * 10f;
        defence = vitality;
        damage = strength * 1.5f;
        stagger = strength;
        //attackSpeed = dexterity * 0.5f;
        dodge = dexterity * 0.1f;
        moveSpeed = dexterity * 0.33f;
        criticalChance = luck * 0.5f;
        OnStatsCalculated?.Invoke();
    }
    /*
        HP = vit * 20 + �߰�HP
        ������ = end * 10 + �߰� ������
        def = vit + ��� def
        damage = str * 1.5 + ��� atk
        stag = str + �⺻ ���� ������
        asp = �⺻ ���� ���ݼӵ� + dex * 0.5 + �߰� ���ݼӵ�
        dodge = dex * 0.1 + �߰� ȸ��
        msp = �⺻ �̵��ӵ� + dex * 0.33
        cri = �⺻ ���� ġ��Ÿ Ȯ�� + luk * 0.5
        cridmg = �߰� ġ��Ÿ ������
        finding = �߰� ���ε� Ȯ��
     */

}
