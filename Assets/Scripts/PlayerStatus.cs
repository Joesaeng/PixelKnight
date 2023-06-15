using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    EquipmentManager equipmentManager;
    #region Player Attribute Allocation
    private int vitality;    // ü�� : MaxHp�� ����
    private int endurance;   // ������ : MaxStamina
    private int strength;    // �ٷ� : ���ݷ°� ������
    private int dexterity;   // �ⷮ : ���ݼӵ��� ȸ����
    private int luck;        // �� : ġ��ŸȮ���� ������ �����
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
