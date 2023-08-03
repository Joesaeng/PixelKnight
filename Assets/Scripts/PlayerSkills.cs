using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    Dictionary<int, SkillData> skillDatas = new Dictionary<int, SkillData>();
    Player player;
    SkillUI skillUI;
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
                break;
            default:
                break;
        }

        return isUse;
    }
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    public void InitUI()
    {
        skillUI = FindObjectOfType<SkillUI>();
        skillUI.OnChangedUsedSkill += SetUsedSkill;
        UsedSkills nullSkills = new
            UsedSkills
        { skill_1 = null, skill_2 = null, skill_3 = null, skill_4 = null, };
        SetUsedSkill(nullSkills);
    }
    public void EnableSkill(int index, SkillData data)
    {
        skillDatas.Add(index, data);
        if (data && data.skillName == SkillName.Judgement)
        {
            judgementRange.enabled = true;
        }
    }
    void SetUsedSkill(UsedSkills skills)
    {
        skillDatas = new Dictionary<int, SkillData>();
        EnableSkill(0, skills.skill_1);
        EnableSkill(1, skills.skill_2);
        EnableSkill(2, skills.skill_3);
        EnableSkill(3, skills.skill_4);
    }
    public SkillData GetData(int index)
    {
        return skillDatas[index];
    }
}
