using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    Dictionary<int, SkillData> skillDatas = new Dictionary<int, SkillData>();
    List<float> skillCoolTimes = new List<float>();
    Player player;
    SkillUI skillUI;

    public List<SkillName> enableSkills = new();

    public Collider2D judgementRange;
    public bool UseSkill(int index)
    {
        bool isUse = false;

        switch (skillDatas[index].skillName)
        {
            case SkillName.Dash: // Dash
                GameObject skill = PoolManager.Instance.GetSkill(skillDatas[index].skillName);
                skill.transform.position = transform.position;
                skill.transform.localScale = transform.localScale;
                isUse = true;
                break;
            case SkillName.Judgement: // Judgement
                foreach (var target in player.GetJudgetTarget())
                {
                    if (target.GetComponent<Enemy>().IsDead) continue;
                    GameObject judge = PoolManager.Instance.GetSkill(skillDatas[index].skillName);
                    judge.transform.position = target.transform.position;
                }
                isUse = true;
                break;
            default:
                break;
        }
        if(isUse) SetCurCoolTime(index,GetCoolTime(index));
        return isUse;
    }
    private void Awake()
    {
        player = GetComponent<Player>();
        float[] t = { 0f, 0f, 0f, 0f };
        skillCoolTimes.AddRange(t);
    }
    public void LoadEnableSkills()
    {
        enableSkills = SaveDataManager.Instance.saveData.skills;
    }
    private void Update()
    {
        CoolTimeUpdate();
    }
    void CoolTimeUpdate()
    {
        for(int i = 0; i < skillCoolTimes.Count; ++i)
        {
            if (skillDatas.ContainsKey(i) && skillCoolTimes[i] > 0f)
                skillCoolTimes[i] -= Time.deltaTime;
        }
    }
    public void SetCurCoolTime(int index,float value)
    {
        skillCoolTimes[index] += value;
    }
    public float GetCurCoolTime(int index)
    {
        return skillCoolTimes[index];
    }
    public float GetCoolTime(int index)
    {
        return skillDatas[index].skillCoolTime;
    }
    public bool CanChangeSkill()
    {
        bool b = true;
        for(int i = 0; i < skillCoolTimes.Count; ++i)
        {
            if (skillCoolTimes[i] > 0f)
                b = false;
        }
        return b;
    }
    public bool CanUseSkill(int index)
    {
        bool can = false;
        if (skillCoolTimes[index] <= 0f)
            can = true;
        return can;
    }
    public void InitUI()
    {
        skillUI = FindObjectOfType<SkillUI>();
        skillUI.OnChangedUsedSkill += SetUsedSkill;
        skillUI.LoadEnableSkills();
        UsedSkills nullSkills = new
            UsedSkills
        { skill_1 = null, skill_2 = null, skill_3 = null, skill_4 = null, };
        SetUsedSkill(nullSkills);
    }
    public void EnableSkill(int index, SkillData data)
    {
        if (data == null) return;
        skillDatas.Add(index, data);
        if (data && data.skillName == SkillName.Judgement)
        {
            judgementRange.enabled = true;
        }
    }
    void SetUsedSkill(UsedSkills skills)
    {
        skillDatas = new Dictionary<int, SkillData>();
        float[] t = { 0f, 0f, 0f, 0f };
        skillCoolTimes.AddRange(t);
        EnableSkill(0, skills.skill_1);
        EnableSkill(1, skills.skill_2);
        EnableSkill(2, skills.skill_3);
        EnableSkill(3, skills.skill_4);
    }
    public SkillData GetData(int index)
    {
        if(skillDatas.ContainsKey(index))
            return skillDatas[index];
        return null;
    }

    public List<SkillName> GetEnableSkills()
    {
        return skillUI.GetEnableSkills();
    }
}
