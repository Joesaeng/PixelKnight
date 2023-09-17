using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurUsedSkills : MonoBehaviour
{
    // 현재 사용중인 스킬의 이미지를 활성화 및 쿨타임을 업데이트하는 UI입니다.
    public Transform slotheader;
    SkillManager mgr;
    UI_CurSkillSlot[] slots;

    private void Start()
    {
        slots = slotheader.GetComponentsInChildren<UI_CurSkillSlot>();
        mgr = SkillManager.Instance;
        mgr.OnSetUsedSkill += SetSkillData;
        SetSkillData();
    }
    public void SetSkillData()
    {
        foreach (UI_CurSkillSlot ui in slots)
        {
            if (mgr.curUsedSkills[ui.index] == -1)
            {
                ui.image.sprite = null;
                ui.image.enabled = false;
                ui.hideImage.fillAmount = 0;
            }
            else
            {
                SkillData data = mgr.skilldatas[mgr.curUsedSkills[ui.index]];
                ui.image.sprite = data.skillIcon;
                ui.image.enabled = true;
                ui.hideImage.fillAmount =
                Utils.Percent(mgr.curCooltimes[mgr.curUsedSkills[ui.index]], data.skillCoolTime);
            }
        }
    }
    private void Update()
    {
        foreach (UI_CurSkillSlot ui in slots)
        {
            if (mgr.curUsedSkills[ui.index] == -1) continue;
            float cooltime = mgr.skilldatas[mgr.curUsedSkills[ui.index]].skillCoolTime;
            ui.hideImage.fillAmount =
                Utils.Percent(mgr.curCooltimes[mgr.curUsedSkills[ui.index]], cooltime);
        }
    }
}
