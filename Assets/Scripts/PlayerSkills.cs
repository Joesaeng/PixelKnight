using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    List<SkillData> skillDatas = new List<SkillData>();
    public bool UseSkill(int index)
    {
        bool isUse = false;
        GameObject skill = PoolManager.Instance.GetSkill(index);
        if(skill != null)
        {
            skill.transform.position = transform.position;
            skill.transform.localScale = transform.localScale;
            isUse = true;
        }
        return isUse;
    }
    private void Awake()
    {
        skillDatas.Add(DataManager.Instance.GetSkillData(0));
    }
    public SkillData GetData(int index)
    {
        return skillDatas[index];
    }
}
