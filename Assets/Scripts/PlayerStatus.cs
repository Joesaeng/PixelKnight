using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttribute
{
    Vitality,
    Endurance,
    Strength,
    Dexterity,
    Luck,
}

public enum StatusName
{
    MaxHp,
    CurHp,
    MaxStamina,
    CurStamina,
    Defence,
    Damage,
    Stagger,
    Poise,
    CurPoise,
    MinAttackSpeed,
    MaxAttackSpeed,
    AttackSpeed,
    Evade,
    HitRate,
    MinMoveSpeed,
    MaxMoveSpeed,
    MoveSpeed,
    MinCriticalChance,
    MaxCriticalChance,
    CriticalChance,
    MinCriticalHitDamage,
    CriticalHitDamage,
    IncreasedItemFindingChance,
}
public class PlayerStatus : MonoBehaviour
{
    public delegate void StatsCalculatedDelegate();
    public event StatsCalculatedDelegate OnStatsCalculated;
    #region Player Attribute Allocation
    Dictionary<PlayerAttribute, int> dPlayerAttribute;
    public int vitality;    // ü�� : MaxHp�� ����
    public int endurance;   // ������ : MaxStamina �� ���ε�
    public int strength;    // �ٷ� : ���ݷ°� ������
    public int dexterity;   // �ⷮ : ���߷��� ȸ����
    public int luck;        // �� : ġ��ŸȮ���� ������ �����
    #endregion 
    #region Player Status
    Dictionary<StatusName, float> dPlayerStatus;
    public float maxHp;
    public float curHp;
    public float maxStamina;
    public float curStamina;
    public float defence;
    public float damage;
    public float stagger;          // ������
    public float poise;            // ���ε�
    public float curPoise;
    public float minAttackSpeed;
    public float maxAttackSpeed;
    public float attackSpeed;
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    public float evade;
    public float hitRate;
    public readonly float minMoveSpeed = 2.5f;
    public readonly float maxMoveSpeed = 3.5f;
    public float moveSpeed;
    public readonly float minCriticalChance = 0.01f;
    public readonly float maxCriticalChance = 1f;
    public float criticalChance;
    public readonly float minCriticalHitDamage = 1.5f;
    public float criticalHitDamage;
    public float increasedItemFindingChance;
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
        moveSpeed = minMoveSpeed;

        CalculateStats();
    }    


    private void CalculateStats()
    {
        maxHp = vitality * 20f;                     // �ִ� ü��
        defence = vitality;                         // ����
        maxStamina = endurance * 10f;               // �ִ� ������
        poise = endurance;                          // ���ε�
        damage = strength * 1.5f;                   // ���ݷ�
        stagger = strength;                         // ������
        hitRate = dexterity * 0.2f;                 // ���߷�
        evade = dexterity * 0.1f;                   // ȸ����
        attackSpeed = minAttackSpeed;               // ���ݼӵ�
        moveSpeed = minMoveSpeed;                   // �̵��ӵ�
        criticalChance = Mathf.Max(luck * 0.01f,0.7f);
        if (criticalChance >= maxCriticalChance) 
            criticalChance = maxCriticalChance;
        else if (criticalChance <= minCriticalChance) 
            criticalChance = minCriticalChance;
        else criticalChance = luck * 0.01f;         // ġ��Ÿ Ȯ��
        criticalHitDamage = minCriticalHitDamage;   // ġ��Ÿ ������
        increasedItemFindingChance = 0f;            // ������ ��� �߰� Ȯ��
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
