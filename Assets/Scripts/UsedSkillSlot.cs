using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class UsedSkillSlot : MonoBehaviour , IPointerUpHandler
{
    public Image skillIcon;
    SkillData skillData;
    SkillUI skillUI;
    public Action<UsedSkillSlot> OnUsedSkill;
    private void Awake()
    {
        skillUI = GetComponentInParent<SkillUI>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GameManager.Instance.player.skills.CanChangeSkill())
        {
            FindAnyObjectByType<CenterPopupText>().SetPopupText
                ("스킬 쿨타임 중에는 변경할 수 없습니다.");
            return;
        }
        SetSkillData(skillUI.GetSelectSkillData());
        OnUsedSkill?.Invoke(this);
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
