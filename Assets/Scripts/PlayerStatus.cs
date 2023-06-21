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
    #region 구버전
    //public int vitality;    // 체력 : MaxHp와 방어력
    //public int endurance;   // 지구력 : MaxStamina 와 강인도
    //public int strength;    // 근력 : 공격력과 경직도
    //public int dexterity;   // 기량 : 명중률과 회피율
    //public int luck;        // 운 : 치명타확률과 아이템 드랍률
    #endregion
    #endregion
    #region Player Status
    public Dictionary<StatusName, float> dPlayerStatus;
    public float AttackSpeed
    {
        get { return dPlayerStatus[StatusName.AttackSpeed]; }
    }
    #region 구버전
    //public float maxHp;
    //public float curHp;
    //public float maxStamina;
    //public float curStamina;
    //public float defence;
    //public float damage;
    //public float stagger;          // 경직도
    //public float poise;            // 강인도
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

        // PlayerAttribute 개수 계산
        string[] playerAttributeNames = Enum.GetNames(typeof(PlayerAttribute));
        int playerAttributeCount = playerAttributeNames.Length;

        // StatusName 개수 계산
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
        else Debug.Log("PlayerStatus 초기화 에러");
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
        #region 구버전
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

        #region 구버전
        //maxHp = vitality * 20f;                     // 최대 체력
        //defence = vitality;                         // 방어력
        //maxStamina = endurance * 10f;               // 최대 지구력
        //poise = endurance;                          // 강인도
        //damage = strength * 1.5f;                   // 공격력
        //stagger = strength;                         // 경직도
        //hitRate = dexterity * 0.2f;                 // 명중률
        //evade = dexterity * 0.1f;                   // 회피율
        //attackSpeed = minAttackSpeed;               // 공격속도
        //moveSpeed = minMoveSpeed;                   // 이동속도
        //criticalChance = Mathf.Max(luck * 0.01f,0.7f);
        //if (criticalChance >= maxCriticalChance) 
        //    criticalChance = maxCriticalChance;
        //else if (criticalChance <= minCriticalChance) 
        //    criticalChance = minCriticalChance;
        //else criticalChance = luck * 0.01f;         // 치명타 확률
        //criticalHitDamage = minCriticalHitDamage;   // 치명타 데미지
        //increasedItemFindingChance = 0f;            // 아이템 드랍 추가 확률
        #endregion
        OnStatsCalculated?.Invoke();
    }
    /*
        HP = vit * 20 + 추가HP
        지구력 = end * 10 + 추가 지구력
        def = vit + 장비 def
        damage = str * 1.5 + 장비 atk
        stag = str + 기본 무기 경직도
        asp = 기본 무기 공격속도 + dex * 0.5 + 추가 공격속도
        dodge = dex * 0.1 + 추가 회피
        msp = 기본 이동속도 + dex * 0.33
        cri = 기본 무기 치명타 확률 + luk * 0.5
        cridmg = 추가 치명타 데미지
        finding = 추가 파인딩 확률
     */

}
