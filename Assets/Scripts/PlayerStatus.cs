using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    EquipmentManager equipmentManager;
    #region Player Attribute Allocation
    private int vitality;    // 체력 : MaxHp와 방어력
    private int endurance;   // 지구력 : MaxStamina
    private int strength;    // 근력 : 공격력과 경직도
    private int dexterity;   // 기량 : 공격속도와 회피율
    private int luck;        // 운 : 치명타확률과 아이템 드랍률
    #endregion 
    #region Player Status
    private float maxHp;
    private float curHp;
    private float maxStamina;
    private float curStamina;
    private float defence;
    private float damage;
    private float stagger;
    private readonly float minAttackSpeed = 0.5f;
    private readonly float maxAttackSpeed = 2.5f;
    private float attackSpeed;
    private float dodge;
    private readonly float minMoveSpeed = 0.7f;
    private readonly float maxMoveSpeed = 1.5f;
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
        equipmentManager = GameManager.instance.equipmentManager;
        CalculateStats();
    }


    private void CalculateStats()
    {
        maxHp = vitality * 20f;
        maxStamina = endurance * 10f;
        defence = vitality;
        damage = strength * 1.5f;
        stagger = strength;
        attackSpeed = dexterity * 0.5f;
        dodge = dexterity * 0.1f;
        moveSpeed = dexterity * 0.33f;
        criticalChance = luck * 0.5f;

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
