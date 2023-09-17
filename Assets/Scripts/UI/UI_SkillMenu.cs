using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_SkillMenu : UI_WindowMenu
{
    public Transform skillDescsParent;
    List<UI_SkillDesc> skillDescs;

    public override void ActiveMenu()
    {
        activeMenu = !activeMenu;
        menuPanel.SetActive(activeMenu);
        SkillManager.Instance.curSelectSkillindex = -1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.SkillMenu]))
            KeyInputAtiveMenu();
    }
    public override void KeyInputAtiveMenu()
    {
        ActiveMenu();
    }
    private void Start()
    {
        skillDescs = new List<UI_SkillDesc>();
        UI_SkillDesc[] uI_SkillDescs = skillDescsParent.GetComponentsInChildren<UI_SkillDesc>();
        skillDescs.AddRange(uI_SkillDescs);
        for(int i = 0; i < DataManager.Instance.skillDatas.Length;++i)
        {
            skillDescs[i].SetData(DataManager.Instance.GetSkillData(i));
            skillDescs[i].descindex = i;
        }
        SkillManager.Instance.OnActivateSkill += ActiveSkill;
        SkillManager.Instance.OnClickDesc += ActivatedSkillClick;
        ActiveSkill();
        SkillManager.Instance.OnSetUsedSkill += OutlineOff;
    }
    void ActiveSkill()
    {
        List<bool> activatedSkills = SkillManager.Instance.activatedSkills;
        for(int i = 0; i < activatedSkills.Count;++i)
        {
            if (activatedSkills[i] == true)
                skillDescs[i].ActiveSkill();
        }
    }
    public void ActivatedSkillClick(int index)
    {
        foreach(UI_SkillDesc ui in skillDescs)
        {
            ui.outline.enabled = false;
        }
        skillDescs[index].outline.enabled = true;
    }
    public void OutlineOff()
    {
        foreach (UI_SkillDesc ui in skillDescs)
        {
            ui.outline.enabled = false;
        }
    }
}
