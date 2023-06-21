using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public Dictionary<PlayerAttribute, int> dPlayerAttribute;
    #region ������
    //public int vitality;    // ü�� : MaxHp�� ����
    //public int endurance;   // ������ : MaxStamina �� ���ε�
    //public int strength;    // �ٷ� : ���ݷ°� ������
    //public int dexterity;   // �ⷮ : ���߷��� ȸ����
    //public int luck;        // �� : ġ��ŸȮ���� ������ �����
    #endregion
    #endregion
    #region Player Status
    public Dictionary<StatusName, float> dPlayerStatus;
    public float AttackSpeed
    {
        get { return dPlayerStatus[StatusName.AttackSpeed]; }
    }
    #region ������
    //public float maxHp;
    //public float curHp;
    //public float maxStamina;
    //public float curStamina;
    //public float defence;
    //public float damage;
    //public float stagger;          // ������
    //public float poise;            // ���ε�
    //public float curPoise;
    //public float minAttackSpeed;
    //public float maxAttackSpeed;
    //public float attackSpeed;
    //public float evade;
    //public float hitRate;
    //public readonly float minMoveSpeed = 2.5f;
    //public readonly float maxMoveSpeed = 3.5f;
    //public float moveSpeed;
    //public readonly float minCriticalChance = 0.01f;
    //public readonly float maxCriticalChance = 1f;
    //public float criticalChance;
    //public readonly float minCriticalHitDamage = 1.5f;
    //public float criticalHitDamage;
    //public float increasedItemFindingChance;
    #endregion
    #endregion
    private void Awake()
    {
        dPlayerAttribute = new Dictionary<PlayerAttribute, int>();
        dPlayerStatus = new Dictionary<StatusName, float>();

        // PlayerAttribute ���� ���
        string[] playerAttributeNames = Enum.GetNames(typeof(PlayerAttribute));
        int playerAttributeCount = playerAttributeNames.Length;

        // StatusName ���� ���
        string[] statusNames = Enum.GetNames(typeof(StatusName));
        int statusNameCount = statusNames.Length;

        for (int i = 0; i < playerAttributeCount; ++i)
        {
            PlayerAttribute playerAttribute = (PlayerAttribute)Enum.Parse(typeof(PlayerAttribute), playerAttributeNames[i]);
            dPlayerAttribute.Add(playerAttribute, 0);
        }
        for (int i = 0; i < statusNameCount; ++i)
        {
            StatusName statusName = (StatusName)Enum.Parse(typeof(StatusName), statusNames[i]);
            dPlayerStatus.Add(statusName, 0);
        }
        if (dPlayerStatus.ContainsKey(StatusName.MinMoveSpeed) &&
           dPlayerStatus.ContainsKey(StatusName.IncreasedItemFindingChance))
        {
            dPlayerStatus[StatusName.MinMoveSpeed] = 2.5f;
            dPlayerStatus[StatusName.MaxMoveSpeed] = 3.5f;
            dPlayerStatus[StatusName.MinCriticalChance] = 0.01f;
            dPlayerStatus[StatusName.MaxCriticalChance] = 1f;
            dPlayerStatus[StatusName.MinCriticalHitDamage] = 1.5f;

        }
        else Debug.Log("PlayerStatus �ʱ�ȭ ����");
    }
    public void SetStatus(PlayerData data)
    {
        dPlayerAttribute[PlayerAttribute.Vitality] = data.vitality;
        dPlayerAttribute[PlayerAttribute.Dexterity] = data.dexterity;
        dPlayerAttribute[PlayerAttribute.Endurance] = data.endurance;
        dPlayerAttribute[PlayerAttribute.Strength] = data.strength;
        dPlayerAttribute[PlayerAttribute.Luck] = data.luck;
        dPlayerStatus[StatusName.MinAttackSpeed] = data.minAttackSpeed;
        dPlayerStatus[StatusName.MaxAttackSpeed] = data.maxAttackSpeed;
        dPlayerStatus[StatusName.AttackSpeed] = data.minAttackSpeed;
        #region ������
        //vitality = data.vitality;
        //endurance = data.endurance;
        //strength = data.strength;
        //dexterity = data.dexterity;
        //luck = data.luck;
        //minAttackSpeed = data.minAttackSpeed;
        //maxAttackSpeed = data.maxAttackSpeed;
        //attackSpeed = minAttackSpeed;
        //moveSpeed = minMoveSpeed;
        #endregion

        CalculateStats();
    }    


    private void CalculateStats()
    {
        dPlayerStatus[StatusName.MaxHp] = dPlayerAttribute[PlayerAttribute.Vitality] * 20f;
        dPlayerStatus[StatusName.MaxStamina] = dPlayerAttribute[PlayerAttribute.Endurance] * 10f;
        dPlayerStatus[StatusName.Defence] = dPlayerAttribute[PlayerAttribute.Vitality];
        dPlayerStatus[StatusName.Poise] = dPlayerAttribute[PlayerAttribute.Endurance];
        dPlayerStatus[StatusName.Damage] = dPlayerAttribute[PlayerAttribute.Strength] * 1.5f;
        dPlayerStatus[StatusName.Stagger] = dPlayerAttribute[PlayerAttribute.Strength];
        dPlayerStatus[StatusName.HitRate] = dPlayerAttribute[PlayerAttribute.Dexterity];
        dPlayerStatus[StatusName.Evade] = dPlayerAttribute[PlayerAttribute.Dexterity] * 0.5f;
        dPlayerStatus[StatusName.AttackSpeed] = dPlayerStatus[StatusName.MinAttackSpeed];
        dPlayerStatus[StatusName.MoveSpeed] = dPlayerStatus[StatusName.MinMoveSpeed];
        dPlayerStatus[StatusName.CriticalChance] = Mathf.Max(dPlayerAttribute[PlayerAttribute.Luck] * 0.01f, 0.7f);
        if (dPlayerStatus[StatusName.CriticalChance] >= dPlayerStatus[StatusName.MaxCriticalChance])
            dPlayerStatus[StatusName.CriticalChance] = dPlayerStatus[StatusName.MaxCriticalChance];
        else if (dPlayerStatus[StatusName.CriticalChance] <= dPlayerStatus[StatusName.MinCriticalChance])
            dPlayerStatus[StatusName.CriticalChance] = dPlayerStatus[StatusName.MinCriticalChance];
        else
            dPlayerStatus[StatusName.CriticalChance] = dPlayerAttribute[PlayerAttribute.Luck] * 0.01f;
        dPlayerStatus[StatusName.CriticalHitDamage] = dPlayerStatus[StatusName.MinCriticalHitDamage];
        dPlayerStatus[StatusName.IncreasedItemFindingChance] = 0f;

        #region ������
        //maxHp = vitality * 20f;                     // �ִ� ü��
        //defence = vitality;                         // ����
        //maxStamina = endurance * 10f;               // �ִ� ������
        //poise = endurance;                          // ���ε�
        //damage = strength * 1.5f;                   // ���ݷ�
        //stagger = strength;                         // ������
        //hitRate = dexterity * 0.2f;                 // ���߷�
        //evade = dexterity * 0.1f;                   // ȸ����
        //attackSpeed = minAttackSpeed;               // ���ݼӵ�
        //moveSpeed = minMoveSpeed;                   // �̵��ӵ�
        //criticalChance = Mathf.Max(luck * 0.01f,0.7f);
        //if (criticalChance >= maxCriticalChance) 
        //    criticalChance = maxCriticalChance;
        //else if (criticalChance <= minCriticalChance) 
        //    criticalChance = minCriticalChance;
        //else criticalChance = luck * 0.01f;         // ġ��Ÿ Ȯ��
        //criticalHitDamage = minCriticalHitDamage;   // ġ��Ÿ ������
        //increasedItemFindingChance = 0f;            // ������ ��� �߰� Ȯ��
        #endregion
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
