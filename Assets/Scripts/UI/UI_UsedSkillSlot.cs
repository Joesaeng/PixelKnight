using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class UI_UsedSkillSlot : MonoBehaviour , IPointerUpHandler
{
    public Image skillIcon;
    SkillData skillData;
    UI_SkillMenu skillUI;
    public Action<UI_UsedSkillSlot> OnChangeUsedSkill;
    private void Awake()
    {
        skillUI = GetComponentInParent<UI_SkillMenu>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GameManager.Instance.player.skills.CanChangeSkill())
        {
            FindAnyObjectByType<UI_CenterPopupText>().SetPopupText
                ("스킬 쿨타임 중에는 변경할 수 없습니다.");
            return;
        }
        SetSkillData(skillUI.GetSelectSkillData());
        OnChangeUsedSkill?.Invoke(this);
    }
    public SkillData GetSkillData()
    {
        return skillData;
    }
    public void SetSkillData(SkillData _skillData)
    {
        if (_skillData == null)
        {
            skillIcon.enabled = false;
            skillData = null;
            return;
        }
        skillData = _skillData;
        skillIcon.sprite = skillData.skillIcon;
        skillIcon.enabled = true;
    }
}
