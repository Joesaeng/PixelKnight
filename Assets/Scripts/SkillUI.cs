using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct UsedSkills
{
    public SkillData skill_1;
    public SkillData skill_2;
    public SkillData skill_3;
    public SkillData skill_4;
}

public class SkillUI : MenuUI
{
    public Transform descPanel;
    public Transform usedPanel;
    public SkillDescUI[] skillDescs;
    public UsedSkillSlot[] usedSkillSlots;
    public SkillData selectSkillData;
    public Action<UsedSkills> OnChangedUsedSkill;
    public List<SkillName> enableSkills = new();
    private void Start()
    {
        menuPanel.SetActive(activeMenu);
        skillDescs = descPanel.GetComponentsInChildren<SkillDescUI>();
        usedSkillSlots = usedPanel.GetComponentsInChildren<UsedSkillSlot>();
        for (int i = 0; i < skillDescs.Length; ++i)
        {
            skillDescs[i].OnSelect += SelectSkill;
            skillDescs[i].OnEnableSkill += EnableSkillData;
        }
        for (int i = 0; i < usedSkillSlots.Length; ++i)
        {
            usedSkillSlots[i].OnUsedSkill += SetUsedSkill;
        }
        InputSystem.Instance.OnSkillMenu += KeyInputAtiveMenu;
    }
    public override void ActiveMenu()
    {
        activeMenu = !activeMenu;
        menuPanel.SetActive(activeMenu);
    }
    public override void KeyInputAtiveMenu()
    {
        ActiveMenu();
    }
    void EnableSkillData(SkillName skillName)
    {
        if (enableSkills.Contains(skillName)) return;
        enableSkills.Add(skillName);
    }
    public List<SkillName> GetEnableSkills()
    {
        return enableSkills;
    }
    public void LoadEnableSkills()
    {
        enableSkills = GameManager.Instance.player.skills.enableSkills;
        for (int i = 0; i < skillDescs.Length; ++i)
        {
            for(int j = 0; j < enableSkills.Count; ++j)
            {
                if(skillDescs[i].skillData != null &&
                    skillDescs[i].skillData.skillName == enableSkills[j])
                {
                    skillDescs[i].EnableSkill();
                    break;
                }
            }
        }
    }
    void SelectSkill(SkillDescUI descUI)
    {
        for (int i = 0; i < skillDescs.Length; ++i)
        {
            if (skillDescs[i] == descUI) continue;
            skillDescs[i].SetSelect(false);
        }
        if (descUI)
            selectSkillData = descUI.GetSelectData();
        else selectSkillData = null;
    }
    void SetUsedSkill(UsedSkillSlot usedSkillSlot)
    {
        for (int i = 0; i < usedSkillSlots.Length; ++i)
        {
            if (usedSkillSlots[i] == usedSkillSlot) continue;
            if(usedSkillSlots[i].GetSkillData() == usedSkillSlot.GetSkillData())
            {
                usedSkillSlots[i].SetSkillData(null);
            }
        }
        for (int i = 0; i < skillDescs.Length; ++i)
        {
            skillDescs[i].SetSelect(false);
            selectSkillData = null;
        }
        OnChangedUsedSkill?.Invoke(GetUsedSkill());
    }
    public SkillData GetSelectSkillData()
    {
        return selectSkillData;
    }
    UsedSkills GetUsedSkill()
    {
        UsedSkills usedSkills = new UsedSkills();
        usedSkills.skill_1 = usedSkillSlots[0].GetSkillData();
        usedSkills.skill_2 = usedSkillSlots[1].GetSkillData();
        usedSkills.skill_3 = usedSkillSlots[2].GetSkillData();
        usedSkills.skill_4 = usedSkillSlots[3].GetSkillData();
        return usedSkills;
    }
}
