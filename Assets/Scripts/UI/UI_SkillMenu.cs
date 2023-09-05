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

public class UI_SkillMenu : UI_WindowMenu
{
    public Transform descPanel;
    public Transform usedPanel;
    public UI_SkillDesc[] skillDescs;
    public UI_UsedSkillSlot[] usedSkillSlots;
    public SkillData selectSkillData;
    public Action<UsedSkills> OnChangedUsedSkill;
    public List<SkillName> enableSkills = new();
    private void Start()
    {
        menuPanel.SetActive(activeMenu);
        skillDescs = descPanel.GetComponentsInChildren<UI_SkillDesc>();
        usedSkillSlots = usedPanel.GetComponentsInChildren<UI_UsedSkillSlot>();
        for (int i = 0; i < skillDescs.Length; ++i)
        {
            skillDescs[i].OnSelect += SelectSkill;
            skillDescs[i].OnEnableSkill += EnableSkillData;
        }
        for (int i = 0; i < usedSkillSlots.Length; ++i)
        {
            usedSkillSlots[i].OnChangeUsedSkill += SetUsedSkill;
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
    void SelectSkill(UI_SkillDesc descUI)
    {
        // UI에서 스킬을 선택했을 때 호출되는 메서드입니다.
        for (int i = 0; i < skillDescs.Length; ++i)
        {
            if (skillDescs[i] == descUI) continue;
            skillDescs[i].SetSelect(false);
            // 현재 선택한 스킬을 제외한 모든 스킬들의 선택 상태를 false로 갱신합니다.
        }
        if (descUI)
            selectSkillData = descUI.GetSelectData();
        else selectSkillData = null;
    }
    void SetUsedSkill(UI_UsedSkillSlot usedSkillSlot)
    {
        // 사용할 스킬을 세팅했을때 호출되는 메서드입니다.
        // UsedSkillSlot을 클릭했을 때 사용하는 스킬을 추가/제거 하거나 변경합니다.
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
        // 사용하는 스킬이 변경되면 플레이어의 스킬 스크립트에
        // 현재 사용중인 스킬을 이벤트로 전달합니다.
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
