using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurUsedSkill
{
    public int num;
    public SkillName skillName;
    public CurUsedSkill(int num, SkillName skillName)
    {
        this.num = num;
        this.skillName = skillName;
    }
}
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
                    if (target.GetComponent<Enemy>().IsDead()) continue;
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
    private void Start()
    {
        player = GetComponent<Player>();
        float[] t = { 0f, 0f, 0f, 0f };
        skillCoolTimes.AddRange(t);
    }
    public void LoadEnableSkills()
    {
        enableSkills = SaveDataManager.Instance.saveData.skills;
    }
    public void LoadUsedSkills()
    {
        List<CurUsedSkill> list = SaveDataManager.Instance.saveData.curSkills;
        for(int i = 0; i < list.Count;++i)
        {
            SetUsedSkill(list[i].num, DataManager.Instance.GetSkillData(list[i].skillName));
            Debug.Log(list[i].skillName + " Load1");
        }
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
        if (curUsedSkills.ContainsKey(index))
            return curUsedSkills[index].skillCoolTime;
        else return 0f;
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
    public void InitUI(UI_SkillMenu uI_SkillMenu)
    {
        skillUI = uI_SkillMenu;
        skillUI.OnChangedUsedSkill += SetUsedSkills;
        skillUI.LoadEnableSkills();
        skillUI.LoadUsedSkills(curUsedSkills);
        UsedSkills nullSkills = new
            UsedSkills
        { skill_1 = null, skill_2 = null, skill_3 = null, skill_4 = null, };
        SetUsedSkills(nullSkills);
    }
    public void SetUsedSkill(int index, SkillData data)
    {
        if (data == null) return;
        curUsedSkills.Add(index, data);
        if (data && data.skillName == SkillName.Judgement)
        {
            judgementRange.enabled = true;
        }
    }
    void SetUsedSkills(UsedSkills skills)
    {
        judgementRange.enabled = false;
        curUsedSkills = new Dictionary<int, SkillData>();
        float[] t = { 0f, 0f, 0f, 0f };
        skillCoolTimes.AddRange(t);
        SetUsedSkill(0, skills.skill_1);
        SetUsedSkill(1, skills.skill_2);
        SetUsedSkill(2, skills.skill_3);
        SetUsedSkill(3, skills.skill_4);
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
    public List<CurUsedSkill> GetCurUsedSkills()
    {
        List<CurUsedSkill> list = new List<CurUsedSkill>();
        for(int i = 0; i < 4; ++i)
        { 
            if(curUsedSkills.ContainsKey(i))
            {
                CurUsedSkill t = new CurUsedSkill(i, curUsedSkills[i].skillName);
                Debug.Log(t.skillName);
                list.Add(t);
            }    
        }
        return list;
    }
}
