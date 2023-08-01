using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    List<SkillData> skillDatas = new List<SkillData>();
    Player player;
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
                foreach(var target in player.GetJudgetTarget())
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
        EnableSkill(SkillName.Dash);
        EnableSkill(SkillName.Judgement);
    }
    public void EnableSkill(SkillName name)
    {
        skillDatas.Add(DataManager.Instance.GetSkillData(name));
        if (name == SkillName.Judgement)
        {
            judgementRange.enabled = true;
        }
    }
    public SkillData GetData(int index)
    {
        return skillDatas[index];
    }
}
