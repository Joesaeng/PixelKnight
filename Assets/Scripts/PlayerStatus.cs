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

public enum FixedStatusName
{
    MaxHp,
    MaxStamina,
    Defence,
    Damage,
    Stagger,
    Poise,
    AttackSpeed,
    Evade,
    HitRate,
    MoveSpeed,
    CriticalChance,
    CriticalHitDamage,
    IncreasedItemFindingChance,
}
public enum DynamicStatusName
{
    CurHp,
    CurStamina,
    CurPoise,
    CurExp
}
public class PlayerStatus : MonoBehaviour
{
    public Equipment equipment;
    public delegate void StatsCalculatedDelegate();
    public event StatsCalculatedDelegate OnStatsCalculated;

    #region Player Attribute Allocation
    public Dictionary<PlayerAttribute, int> dPlayerAttribute;
    #endregion

    #region Player Status
    [Header("Player Status")]
    private PlayerData initialPlayerData;
    public Dictionary<FixedStatusName, float> dPlayerFixedStatus;
    public Dictionary<DynamicStatusName, float> dPlayerDynamicStatus;
    public float minAttackSpeed;
    public float maxAttackSpeed;

    private float hpRegenerationPerTenSecond;
    private float staminaRegenerationPerSecond;
    private float poiseRegenerationPerSecond;

    public float AttackSpeed
    {
        get { return dPlayerFixedStatus[FixedStatusName.AttackSpeed]; }
    }
    public readonly float minMoveSpeed = 2.5f;
    public readonly float maxMoveSpeed = 3.5f;
    public readonly float minCriticalChance = 0.05f;
    public readonly float maxCriticalChance = 1f;
    public readonly float minCriticalHitDamage = 1.5f;

    [Header("Lv & Experience")]
    public int playerLv;
    public float firstExpRequirement;
    public float expRequirementIncrese;
    #endregion
    public float TestCurHp;
    private float hpRegenTime = 0f;
    public Action OnPlayerDead;
    private void Awake()
    {
        equipment = Equipment.Instance;
        equipment.OnChangeEquipment += UpdateStatus;
        dPlayerAttribute = new Dictionary<PlayerAttribute, int>();
        dPlayerFixedStatus = new Dictionary<FixedStatusName, float>();
        dPlayerDynamicStatus = new Dictionary<DynamicStatusName, float>();

        // PlayerAttribute ���� ���
        string[] playerAttributeNames = Enum.GetNames(typeof(PlayerAttribute));
        int playerAttributeCount = playerAttributeNames.Length;

        // FixedStatusName ���� ���
        string[] statusNames = Enum.GetNames(typeof(FixedStatusName));
        int statusNameCount = statusNames.Length;

        // DynamicStatusName ���� ���
        string[] dynamicStatusNames = Enum.GetNames(typeof(DynamicStatusName));
        int dynamicStatusNameCount = dynamicStatusNames.Length;

        for (int i = 0; i < playerAttributeCount; ++i)
        {
            PlayerAttribute playerAttribute = (PlayerAttribute)Enum.Parse(typeof(PlayerAttribute), playerAttributeNames[i]);
            dPlayerAttribute.Add(playerAttribute, 0);
        }
        for (int i = 0; i < statusNameCount; ++i)
        {
            FixedStatusName statusName = (FixedStatusName)Enum.Parse(typeof(FixedStatusName), statusNames[i]);
            dPlayerFixedStatus.Add(statusName, 0);
        }
        for (int i = 0; i < dynamicStatusNameCount; ++i)
        {
            DynamicStatusName dynamicStatusName = (DynamicStatusName)Enum.Parse(typeof(DynamicStatusName), dynamicStatusNames[i]);
            dPlayerDynamicStatus.Add(dynamicStatusName, 0);
        }
    }
    private void Start()
    {
        hpRegenerationPerTenSecond = 5f;
    }
    private void Update()
    {
        RegeneratePoise();
        RegenerateStamina();
        TestCurHp = dPlayerDynamicStatus[DynamicStatusName.CurHp];
        hpRegenTime += Time.deltaTime;
        if (hpRegenTime >= 10f)
        {
            HpRegeneration();
            hpRegenTime = 0f;
        }
    }
    public void InitSetStatus(PlayerData data) // �ʱ� �������ͽ� ����
    {
        initialPlayerData = data;
        UpdateStatus();
        dPlayerDynamicStatus[DynamicStatusName.CurHp] = dPlayerFixedStatus[FixedStatusName.MaxHp]; 
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
    }
    public void UpdateStatus() // �������ͽ��� �����ؾ� �� �� ȣ��Ǵ� �޼���
    {
        dPlayerAttribute[PlayerAttribute.Vitality] = initialPlayerData.vitality;
        dPlayerAttribute[PlayerAttribute.Dexterity] = initialPlayerData.dexterity;
        dPlayerAttribute[PlayerAttribute.Endurance] = initialPlayerData.endurance;
        dPlayerAttribute[PlayerAttribute.Strength] = initialPlayerData.strength;
        dPlayerAttribute[PlayerAttribute.Luck] = initialPlayerData.luck;
        minAttackSpeed = initialPlayerData.minAttackSpeed;
        maxAttackSpeed = initialPlayerData.maxAttackSpeed;
        for(int i = 0; i < dPlayerFixedStatus.Count;++i)
        {
            dPlayerFixedStatus[(FixedStatusName)i] = 0f;
        }
        //dPlayerFixedStatus[FixedStatusName.AttackSpeed] = initialPlayerData.minAttackSpeed;
        CalculateStats();
    }
    private void CalculateStats() // �������ͽ� ���
    {
        #region ���� ����� �����۵��� �߰� Status �Ҵ�
        ApplyEquip(equipment.GetCurEquip());
        #endregion
        #region Attribute ���� ���� Status �Ҵ�
        AttributeCalculate();
        #endregion
        #region ���� ������� ��ų���� �߰� Status �Ҵ�
        #endregion

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
    private void AttributeCalculate() // Attribute�� Value�� �̿��Ͽ� ���� Player Status�� ����
    {
        dPlayerFixedStatus[FixedStatusName.MaxHp] += dPlayerAttribute[PlayerAttribute.Vitality] * 20f;
        dPlayerFixedStatus[FixedStatusName.MaxStamina] += dPlayerAttribute[PlayerAttribute.Endurance] * 10f;
        dPlayerFixedStatus[FixedStatusName.Defence] += dPlayerAttribute[PlayerAttribute.Vitality];
        dPlayerFixedStatus[FixedStatusName.Poise] += dPlayerAttribute[PlayerAttribute.Endurance];
        dPlayerFixedStatus[FixedStatusName.Damage] += dPlayerAttribute[PlayerAttribute.Strength] * 1.5f;
        dPlayerFixedStatus[FixedStatusName.Stagger] += dPlayerAttribute[PlayerAttribute.Strength];
        dPlayerFixedStatus[FixedStatusName.HitRate] += dPlayerAttribute[PlayerAttribute.Dexterity];
        dPlayerFixedStatus[FixedStatusName.Evade] += dPlayerAttribute[PlayerAttribute.Dexterity] * 0.33f;
        dPlayerFixedStatus[FixedStatusName.AttackSpeed] += minAttackSpeed;
        dPlayerFixedStatus[FixedStatusName.MoveSpeed] += minMoveSpeed;
        dPlayerFixedStatus[FixedStatusName.CriticalChance] += dPlayerAttribute[PlayerAttribute.Luck] * 0.01f;
        if (dPlayerFixedStatus[FixedStatusName.CriticalChance] >= maxCriticalChance)
            dPlayerFixedStatus[FixedStatusName.CriticalChance] = maxCriticalChance;
        else if (dPlayerFixedStatus[FixedStatusName.CriticalChance] <= minCriticalChance)
            dPlayerFixedStatus[FixedStatusName.CriticalChance] = minCriticalChance;
        dPlayerFixedStatus[FixedStatusName.CriticalHitDamage] += minCriticalHitDamage;
    }
    private void ApplyEquip(Equip[] _CurEquips) // ����� �ɼ��� �������ͽ��� ����
    {
        foreach(Equip equip in _CurEquips)
        {
            switch (equip.baseOption)
            {
                case BaseOption.Damage:
                    dPlayerFixedStatus[FixedStatusName.Damage] += equip.baseOptionValue;
                    break;
                case BaseOption.Defence:
                    dPlayerFixedStatus[FixedStatusName.Defence] += equip.baseOptionValue;
                    break;
                default:
                    Debug.LogError("PlayerStatus ApplyEquip BaseOptions");
                    break;
            }
        }
        foreach(Equip equip in _CurEquips)
        {
            if (equip.additionalOptions == null) continue;
            foreach(var option in equip.additionalOptions)
            {
                switch (option.Key)
                {
                    case AdditionalOptions.Vitality:
                        dPlayerAttribute[PlayerAttribute.Vitality] += (int)option.Value;
                        break;
                    case AdditionalOptions.Endurance:
                        dPlayerAttribute[PlayerAttribute.Endurance] += (int)option.Value;
                        break;
                    case AdditionalOptions.Strength:
                        dPlayerAttribute[PlayerAttribute.Strength] += (int)option.Value;
                        break;
                    case AdditionalOptions.Dexterity:
                        dPlayerAttribute[PlayerAttribute.Dexterity] += (int)option.Value;
                        break;
                    case AdditionalOptions.Luck:
                        dPlayerAttribute[PlayerAttribute.Luck] += (int)option.Value;
                        break;
                    case AdditionalOptions.MaxHp:
                        dPlayerFixedStatus[FixedStatusName.MaxHp] += option.Value;
                        break;
                    case AdditionalOptions.MaxStamina:
                        dPlayerFixedStatus[FixedStatusName.MaxStamina] += option.Value;
                        break;
                    case AdditionalOptions.Stagger:
                        dPlayerFixedStatus[FixedStatusName.Stagger] += option.Value;
                        break;
                    case AdditionalOptions.Poise:
                        dPlayerFixedStatus[FixedStatusName.Poise] += option.Value;
                        break;
                    case AdditionalOptions.AttackSpeed:
                        dPlayerFixedStatus[FixedStatusName.AttackSpeed] += option.Value;
                        break;
                    case AdditionalOptions.Evade:
                        dPlayerFixedStatus[FixedStatusName.Evade] += option.Value;
                        break;
                    case AdditionalOptions.HitRate:
                        dPlayerFixedStatus[FixedStatusName.HitRate] += option.Value;
                        break;
                    case AdditionalOptions.MoveSpeed:
                        dPlayerFixedStatus[FixedStatusName.MoveSpeed] += option.Value;
                        break;
                    case AdditionalOptions.CriticalChance:
                        dPlayerFixedStatus[FixedStatusName.CriticalChance] += option.Value;
                        break;
                    case AdditionalOptions.CriticalHitDamage:
                        dPlayerFixedStatus[FixedStatusName.CriticalHitDamage] += option.Value;
                        break;
                    case AdditionalOptions.IncreasedItemFindingChance:
                        dPlayerFixedStatus[FixedStatusName.IncreasedItemFindingChance] += option.Value;
                        break;
                    default:
                        Debug.LogError("PlayerStatus_ApplyEquip_AddtionalOptions");
                        break;
                }
            }
        }
    }

    void RegenerateStamina()
    {
        float staminaRegenAmount = staminaRegenerationPerSecond * Time.deltaTime;
        if(dPlayerDynamicStatus[DynamicStatusName.CurStamina] < dPlayerFixedStatus[FixedStatusName.MaxStamina])
            ModifyStamina(staminaRegenAmount);
    }
    void RegeneratePoise()
    {
        float poiseRegenAmount = poiseRegenerationPerSecond * Time.deltaTime;
        if (dPlayerDynamicStatus[DynamicStatusName.CurPoise] < dPlayerFixedStatus[FixedStatusName.Poise])
            ModifyPoise(poiseRegenAmount);
    }
    public void HpRegeneration()
    {
        if (dPlayerDynamicStatus[DynamicStatusName.CurHp] + hpRegenerationPerTenSecond // �̹��� �ڷ�ƾ�� ȸ���ϴ� Hp�� MaxHp�� �Ѿ ���
            >= dPlayerFixedStatus[FixedStatusName.MaxHp])
            dPlayerDynamicStatus[DynamicStatusName.CurHp] = dPlayerFixedStatus[FixedStatusName.MaxHp]; // ���� HP�� MaxHp��
        else ModifyHp(hpRegenerationPerTenSecond);
    }
    #region Dynanimc �������ͽ��� ���� �޼���
    public void ModifyHp(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurHp] += value;
        if(dPlayerDynamicStatus[DynamicStatusName.CurHp] <= 0f)
        {
            OnPlayerDead?.Invoke();
            dPlayerDynamicStatus[DynamicStatusName.CurHp] = 0;
        }
    }
    public void ModifyStamina(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurStamina] += value;
    }
    public void ModifyPoise(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurPoise] += value;
    }
    public void ModifyExp(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurExp] += value;
    }
    #endregion
    /*
     HP ���� �޼���
     Player ���
     
     Stamina ���� �޼���
     Exp ���� �޼���
     Poise ���� �޼���
     DynamicStatus Update �޼���
     LvUP �޼���
     UI ����
     */

}
