using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    // 플레이어 오브젝트에 컴포넌트로 부착되는 스킬 클래스 입니다.

    // 현재 사용중인 스킬들의 데이터
    Dictionary<int, SkillData> curUsedSkills = new Dictionary<int, SkillData>();
    List<float> skillCoolTimes = new List<float>();
    Player player;
    UI_SkillMenu skillUI;

    public List<SkillName> enableSkills = new();

    public Collider2D judgementRange;
    public bool UseSkill(int index) 
        // 스킬 키 입력시 Player에서 호출하는 메서드입니다.
    {
        bool isUse = false;

        switch (curUsedSkills[index].skillName)
        {
            case SkillName.Dash: // Dash
                GameObject skill = PoolManager.Instance.GetSkill(curUsedSkills[index].skillName);
                skill.transform.position = transform.position;
                skill.transform.localScale = transform.localScale;
                isUse = true;
                break;
            case SkillName.Judgement: // Judgement
                foreach (var target in player.GetJudgetTarget())
                {
                    if (target.GetComponent<Enemy>().IsDead) continue;
                    GameObject judge = PoolManager.Instance.GetSkill(curUsedSkills[index].skillName);
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
        FindAnyObjectByType<UI_CurUsedSkills>().Init(this);
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
            if (curUsedSkills.ContainsKey(i) && skillCoolTimes[i] > 0f)
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
        return curUsedSkills[index].skillCoolTime;
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
        skillUI = FindObjectOfType<UI_SkillMenu>();
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
        curUsedSkills.Add(index, data);
        if (data && data.skillName == SkillName.Judgement)
        {
            judgementRange.enabled = true;
        }
    }
    void SetUsedSkill(UsedSkills skills)
    {
        judgementRange.enabled = false;
        curUsedSkills = new Dictionary<int, SkillData>();
        float[] t = { 0f, 0f, 0f, 0f };
        skillCoolTimes.AddRange(t);
        EnableSkill(0, skills.skill_1);
        EnableSkill(1, skills.skill_2);
        EnableSkill(2, skills.skill_3);
        EnableSkill(3, skills.skill_4);
    }
    public SkillData GetData(int index)
    {
        if(curUsedSkills.ContainsKey(index))
            return curUsedSkills[index];
        return null;
    }

    public List<SkillName> GetEnableSkills()
    {
        return skillUI.GetEnableSkills();
    }
}
