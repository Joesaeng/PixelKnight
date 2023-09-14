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
    HpRegen,
    StaminaRegen,
    PoiseRegen,
}
public enum DynamicStatusName
{
    CurHp,
    CurStamina,
    CurPoise,
    CurExp
}
public enum DamageOption
{
    Critical,
    Normal,
}
public class CalculatedDamage
{
    public DamageOption option;
    public float damage;
}

public class PlayerStatus : MonoBehaviour
{
    public Equipment equipment;
    public delegate void StatsCalculatedDelegate();
    public event StatsCalculatedDelegate OnStatsCalculated;

    #region Player Attribute Allocation
    public Dictionary<PlayerAttribute, int> dPlayerAttribute;
    public Dictionary<PlayerAttribute, int> dAddedAttribute;
    #endregion

    #region Player Status
    private PlayerData initialPlayerData;
    public Dictionary<FixedStatusName, float> dPlayerFixedStatus;
    public Dictionary<DynamicStatusName, float> dPlayerDynamicStatus;
    public float minAttackSpeed;
    public float maxAttackSpeed;

    public float AttackSpeed
    {
        get { return dPlayerFixedStatus[FixedStatusName.AttackSpeed]; }
    }
    public readonly float minMoveSpeed = 2.5f;
    public readonly float maxMoveSpeed = 3.5f;
    public readonly float minCriticalChance = 0.05f;
    public readonly float maxCriticalChance = 1f;
    public readonly float minCriticalHitDamage = 1.5f;

    #region 레벨 관련
    public int playerLv = 0;
    private float expRequirement;
    public float ExpRequirement
    {
        set => this.expRequirement = value;
        get { return expRequirement; }
    }
    private float expRequirementIncrese = 1.6f; // 레벨업 당 경험치필요량 증가량
    public int remainingPoint = 0;
    public int addedPoint;                      // 현재까지 총 받은 포인트
    public Action OnLevelUp;

    #endregion
    #endregion

    private float hpRegenTime = 0f;
    public Action OnPlayerDead;
    public Action OnPlayerHit;
    public Action OnUpdateRemaingPoint;


    private void Awake()
    {
        equipment = Equipment.Instance;
        equipment.OnChangeEquipment += UpdateStatus;
        dPlayerAttribute = new Dictionary<PlayerAttribute, int>();
        dAddedAttribute = new Dictionary<PlayerAttribute, int>();
        dPlayerFixedStatus = new Dictionary<FixedStatusName, float>();
        dPlayerDynamicStatus = new Dictionary<DynamicStatusName, float>();
        expRequirement = 50f;

        // PlayerAttribute 개수 계산
        string[] playerAttributeNames = Enum.GetNames(typeof(PlayerAttribute));
        int playerAttributeCount = playerAttributeNames.Length;

        // FixedStatusName 개수 계산
        string[] statusNames = Enum.GetNames(typeof(FixedStatusName));
        int statusNameCount = statusNames.Length;

        // DynamicStatusName 개수 계산
        string[] dynamicStatusNames = Enum.GetNames(typeof(DynamicStatusName));
        int dynamicStatusNameCount = dynamicStatusNames.Length;

        for (int i = 0; i < playerAttributeCount; ++i)
        {
            PlayerAttribute playerAttribute = (PlayerAttribute)Enum.Parse(typeof(PlayerAttribute), playerAttributeNames[i]);
            dPlayerAttribute.Add(playerAttribute, 0);
        }
        for (int i = 0; i < playerAttributeCount; ++i)
        {
            PlayerAttribute playerAttribute = (PlayerAttribute)Enum.Parse(typeof(PlayerAttribute), playerAttributeNames[i]);
            dAddedAttribute.Add(playerAttribute, 0);
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
    public SaveData SaveStatus()
    {
        SaveData saveData = new();
        saveData.name = SaveDataManager.Instance.saveData.name;
        saveData.level = playerLv;
        saveData.curExp = dPlayerDynamicStatus[DynamicStatusName.CurExp];
        saveData.expReq = ExpRequirement;
        saveData.remainingPoint = remainingPoint;
        saveData.addedPoint = addedPoint;

        saveData.AddedVit = dAddedAttribute[PlayerAttribute.Vitality];
        saveData.AddedEnd = dAddedAttribute[PlayerAttribute.Endurance];
        saveData.AddedStr = dAddedAttribute[PlayerAttribute.Strength];
        saveData.AddedDex = dAddedAttribute[PlayerAttribute.Dexterity];
        saveData.AddedLuk = dAddedAttribute[PlayerAttribute.Luck];

        saveData.curHp = dPlayerDynamicStatus[DynamicStatusName.CurHp];

        return saveData;
    }
    public void LoadStatus()
    {
        SaveData loadData = SaveDataManager.Instance.saveData;

        playerLv = loadData.level;
        dPlayerDynamicStatus[DynamicStatusName.CurExp] = loadData.curExp;
        dPlayerDynamicStatus[DynamicStatusName.CurHp] = loadData.curHp;
        ExpRequirement = loadData.expReq;
        remainingPoint = loadData.remainingPoint;
        addedPoint = loadData.addedPoint;
        dAddedAttribute[PlayerAttribute.Vitality] = loadData.AddedVit;
        dAddedAttribute[PlayerAttribute.Endurance] = loadData.AddedEnd;
        dAddedAttribute[PlayerAttribute.Strength] = loadData.AddedStr;
        dAddedAttribute[PlayerAttribute.Dexterity] = loadData.AddedDex;
        dAddedAttribute[PlayerAttribute.Luck] = loadData.AddedLuk;
    }
    private void Update()
    {
        RegeneratePoise();
        RegenerateStamina();
        hpRegenTime += Time.deltaTime;
        if (hpRegenTime >= 10f)
        {
            HpRegeneration();
            hpRegenTime = 0f;
        }
    }
    public void InitSetStatus(PlayerData data) // 초기 스테이터스 설정
    {
        initialPlayerData = data;
        LoadStatus();
        UpdateStatus();
        if(dPlayerDynamicStatus[DynamicStatusName.CurHp] == 0)
            dPlayerDynamicStatus[DynamicStatusName.CurHp] = dPlayerFixedStatus[FixedStatusName.MaxHp];
        dPlayerDynamicStatus[DynamicStatusName.CurStamina] = dPlayerFixedStatus[FixedStatusName.MaxStamina];
        dPlayerDynamicStatus[DynamicStatusName.CurPoise] = dPlayerFixedStatus[FixedStatusName.Poise];
    }
    public void ResetStatus()
    {
        ResetAddedAttribute();
        ModifyRemainingPoint(addedPoint);
        addedPoint = 0;
        UpdateStatus();
    }
    void ResetAddedAttribute()
    {
        for (int i = 0; i < dAddedAttribute.Count; ++i)
        {
            dAddedAttribute[(PlayerAttribute)i] = 0;
        }
    }
    public void UpdateStatus() // 스테이터스를 변경해야 할 때 호출되는 메서드
    {
        dPlayerAttribute[PlayerAttribute.Vitality] = initialPlayerData.vitality + dAddedAttribute[PlayerAttribute.Vitality];
        dPlayerAttribute[PlayerAttribute.Dexterity] = initialPlayerData.dexterity + dAddedAttribute[PlayerAttribute.Dexterity];
        dPlayerAttribute[PlayerAttribute.Endurance] = initialPlayerData.endurance + dAddedAttribute[PlayerAttribute.Endurance];
        dPlayerAttribute[PlayerAttribute.Strength] = initialPlayerData.strength + dAddedAttribute[PlayerAttribute.Strength];
        dPlayerAttribute[PlayerAttribute.Luck] = initialPlayerData.luck + dAddedAttribute[PlayerAttribute.Luck];
        minAttackSpeed = initialPlayerData.minAttackSpeed;
        maxAttackSpeed = initialPlayerData.maxAttackSpeed;
        for (int i = 0; i < dPlayerFixedStatus.Count; ++i)
        {
            dPlayerFixedStatus[(FixedStatusName)i] = 0f;
        }

        CalculateStats();
    }
    private void CalculateStats() // 스테이터스 계산
    {
        #region 현재 장비한 아이템들의 추가 Status 할당
        ApplyEquip(equipment.GetCurEquip());
        #endregion
        #region Attribute 값의 따른 Status 할당
        AttributeCalculate();
        #endregion
        #region 현재 적용받고있는 버프들의 추가 Status 할당
        ApplyBuffs();
        #endregion
        #region DynamicStatus가 FixedStatus를 넘어가는 경우를 배제
        DeleteOverStatus();
        #endregion
        OnStatsCalculated?.Invoke();
    }
    private void AttributeCalculate() // Attribute의 Value를 이용하여 현재 Player Status를 설정
    {
        dPlayerFixedStatus[FixedStatusName.MaxHp] += dPlayerAttribute[PlayerAttribute.Vitality] * 20f;
        dPlayerFixedStatus[FixedStatusName.MaxStamina] += dPlayerAttribute[PlayerAttribute.Endurance] * 10f;
        dPlayerFixedStatus[FixedStatusName.StaminaRegen] += dPlayerAttribute[PlayerAttribute.Endurance];
        dPlayerFixedStatus[FixedStatusName.PoiseRegen] += dPlayerAttribute[PlayerAttribute.Endurance];
        dPlayerFixedStatus[FixedStatusName.HpRegen] += dPlayerAttribute[PlayerAttribute.Vitality];
        dPlayerFixedStatus[FixedStatusName.Defence] += dPlayerAttribute[PlayerAttribute.Vitality];
        dPlayerFixedStatus[FixedStatusName.Poise] += dPlayerAttribute[PlayerAttribute.Endurance] * 10f;
        dPlayerFixedStatus[FixedStatusName.Damage] += dPlayerAttribute[PlayerAttribute.Strength] * 1.5f;
        dPlayerFixedStatus[FixedStatusName.Stagger] += dPlayerAttribute[PlayerAttribute.Strength];
        dPlayerFixedStatus[FixedStatusName.HitRate] += dPlayerAttribute[PlayerAttribute.Dexterity] + 20f;
        dPlayerFixedStatus[FixedStatusName.Evade] += dPlayerAttribute[PlayerAttribute.Dexterity] * 0.33f;
        dPlayerFixedStatus[FixedStatusName.AttackSpeed] += minAttackSpeed;
        dPlayerFixedStatus[FixedStatusName.MoveSpeed] += minMoveSpeed;
        dPlayerFixedStatus[FixedStatusName.CriticalChance] += dPlayerAttribute[PlayerAttribute.Luck] * 0.01f;
        if (dPlayerFixedStatus[FixedStatusName.CriticalChance] >= maxCriticalChance)
            dPlayerFixedStatus[FixedStatusName.CriticalChance] = maxCriticalChance;
        else if (dPlayerFixedStatus[FixedStatusName.CriticalChance] <= minCriticalChance)
            dPlayerFixedStatus[FixedStatusName.CriticalChance] = minCriticalChance;
        dPlayerFixedStatus[FixedStatusName.CriticalHitDamage] += minCriticalHitDamage;
        if (dPlayerFixedStatus[FixedStatusName.AttackSpeed] >= maxAttackSpeed)
            dPlayerFixedStatus[FixedStatusName.AttackSpeed] = maxAttackSpeed;
        else if (dPlayerFixedStatus[FixedStatusName.AttackSpeed] <= minAttackSpeed)
            dPlayerFixedStatus[FixedStatusName.AttackSpeed] = minAttackSpeed;
    }
    private void ApplyEquip(Equip[] _CurEquips) // 장비의 옵션을 스테이터스에 저장
    {
        foreach (Equip equip in _CurEquips)
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
        foreach (Equip equip in _CurEquips)
        {
            if (equip.additionalOptions == null) continue;
            foreach (var option in equip.additionalOptions)
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
                    case AdditionalOptions.HpRegen:
                        dPlayerFixedStatus[FixedStatusName.HpRegen] += option.Value;
                        break;
                    case AdditionalOptions.StaminaRegen:
                        dPlayerFixedStatus[FixedStatusName.StaminaRegen] += option.Value;
                        break;
                    case AdditionalOptions.PoiseRegen:
                        dPlayerFixedStatus[FixedStatusName.PoiseRegen] += option.Value;
                        break;
                    default:
                        Debug.LogError("PlayerStatus_ApplyEquip_AddtionalOptions");
                        break;
                }
            }
        }
    }
    private void ApplyBuffs()
    {

    }
    private void DeleteOverStatus()
    {
        if (dPlayerDynamicStatus[DynamicStatusName.CurHp] > dPlayerFixedStatus[FixedStatusName.MaxHp])
            dPlayerDynamicStatus[DynamicStatusName.CurHp] = dPlayerFixedStatus[FixedStatusName.MaxHp];
        if (dPlayerDynamicStatus[DynamicStatusName.CurStamina] > dPlayerFixedStatus[FixedStatusName.MaxStamina])
            dPlayerDynamicStatus[DynamicStatusName.CurStamina] = dPlayerFixedStatus[FixedStatusName.MaxStamina];
        if (dPlayerDynamicStatus[DynamicStatusName.CurPoise] > dPlayerFixedStatus[FixedStatusName.Poise])
            dPlayerDynamicStatus[DynamicStatusName.CurPoise] = dPlayerFixedStatus[FixedStatusName.Poise];
    }
    #region 각종 리젠
    void RegenerateStamina()
    {
        float staminaRegenAmount = dPlayerFixedStatus[FixedStatusName.StaminaRegen] * Time.deltaTime;
        if (dPlayerDynamicStatus[DynamicStatusName.CurStamina] < dPlayerFixedStatus[FixedStatusName.MaxStamina])
            ModifyStamina(staminaRegenAmount);
    }
    void RegeneratePoise()
    {
        float poiseRegenAmount = dPlayerFixedStatus[FixedStatusName.PoiseRegen] * Time.deltaTime;
        if (dPlayerDynamicStatus[DynamicStatusName.CurPoise] < dPlayerFixedStatus[FixedStatusName.Poise])
            ModifyPoise(poiseRegenAmount);
    }
    public void HpRegeneration()
    {
        if (dPlayerDynamicStatus[DynamicStatusName.CurHp] + dPlayerFixedStatus[FixedStatusName.HpRegen] // 이번에 코루틴에 회복하는 Hp가 MaxHp를 넘어설 경우
            >= dPlayerFixedStatus[FixedStatusName.MaxHp])
            dPlayerDynamicStatus[DynamicStatusName.CurHp] = dPlayerFixedStatus[FixedStatusName.MaxHp]; // 현재 HP를 MaxHp로
        else ModifyHp(dPlayerFixedStatus[FixedStatusName.HpRegen]);
    }
    #endregion
    public void ModifyAttribute(PlayerAttribute key, int value)
    {
        dAddedAttribute[key] += value;
        addedPoint += value;
        UpdateStatus();
    }
    #region Dynanimc 스테이터스들 수정 메서드
    public void ModifyHp(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurHp] += value;
        if (dPlayerDynamicStatus[DynamicStatusName.CurHp] <= 0f)
        {
            OnPlayerDead?.Invoke();
            dPlayerDynamicStatus[DynamicStatusName.CurHp] = 0f;
        }
        if (dPlayerDynamicStatus[DynamicStatusName.CurHp] > dPlayerFixedStatus[FixedStatusName.MaxHp])
        {
            dPlayerDynamicStatus[DynamicStatusName.CurHp] = dPlayerFixedStatus[FixedStatusName.MaxHp];
        }
    }
    public void ModifyStamina(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurStamina] += value;
    }
    public void ModifyPoise(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurPoise] += value;
        if (dPlayerDynamicStatus[DynamicStatusName.CurPoise] <= 0f)
        {
            OnPlayerHit?.Invoke();
            dPlayerDynamicStatus[DynamicStatusName.CurPoise] = 0f;
        }
    }
    public void ModifyExp(float value)
    {
        dPlayerDynamicStatus[DynamicStatusName.CurExp] += value;
        if (dPlayerDynamicStatus[DynamicStatusName.CurExp] >= expRequirement)
        {
            dPlayerDynamicStatus[DynamicStatusName.CurExp] -= expRequirement;
            expRequirement *= expRequirementIncrese;
            LevelUp();
        }
    }
    #endregion
    private void LevelUp()
    {
        playerLv += 1;
        ModifyRemainingPoint(5);
        OnLevelUp?.Invoke();
    }
    public void ModifyRemainingPoint(int value)
    {
        remainingPoint += value;
        OnUpdateRemaingPoint?.Invoke();
    }
    public void TakeDamage(EnemyStatus enemyStatus)
    {
        if (dPlayerFixedStatus[FixedStatusName.Defence] < enemyStatus.damage)
        {
            ModifyHp(dPlayerFixedStatus[FixedStatusName.Defence] - enemyStatus.damage);
        }
        else
        {
            ModifyHp(-1f);
        }
        ModifyPoise(-enemyStatus.stagger);
    }
    public void HpRecovery(float value)
    {
        ModifyHp(value);
    }
    public bool CalculatedHit(EnemyStatus enemyStatus) // 피격 계산
    {
        bool isHit = false;
        float hitRate = 0f;
        float hitDiff = enemyStatus.hitrate - dPlayerFixedStatus[FixedStatusName.Evade];
        if (hitDiff > 25)
        {
            isHit = true;
            TakeDamage(enemyStatus);
            Spawner.instance.ShowDamageEffect(3).transform.SetPositionAndRotation
                (transform.position, Quaternion.identity);
        }
        else if (hitDiff > 0)
        {
            hitRate = hitDiff * 0.04f;
        }
        if (UnityEngine.Random.value < hitRate)
        {
            isHit = true;
            TakeDamage(enemyStatus);
            Spawner.instance.ShowDamageEffect(3).transform.SetPositionAndRotation
                (transform.position, Quaternion.identity);
        }
        if (!isHit)
        {
            Spawner.instance.ShowDamageEffect(1).transform.SetPositionAndRotation
            (transform.position, Quaternion.identity);
        }
        return isHit;
    }
    public CalculatedDamage CalculateDamage() // 피격 데미지 계산
    {
        CalculatedDamage result = new();
        if (UnityEngine.Random.value < dPlayerFixedStatus[FixedStatusName.CriticalChance])
        {
            result.option = DamageOption.Critical;
            result.damage = dPlayerFixedStatus[FixedStatusName.Damage] * dPlayerFixedStatus[FixedStatusName.CriticalHitDamage];
        }
        else
        {
            result.option = DamageOption.Normal;
            result.damage = dPlayerFixedStatus[FixedStatusName.Damage];
        }

        return result;
    }
    public bool UseStamina(float value)
    {
        bool isUse = false;
        if (dPlayerDynamicStatus[DynamicStatusName.CurStamina] >= value)
        {
            ModifyStamina(-value);
            isUse = true;
        }

        return isUse;
    }
    
    public float GetHitDiff(float evade)
    {
        return dPlayerFixedStatus[FixedStatusName.HitRate] - evade;
    }
}
