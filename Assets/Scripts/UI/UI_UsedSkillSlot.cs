using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_UsedSkillSlot : MonoBehaviour
{
    public int slotindex;
    public Image skillImage;
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(UsedSkillSlotButton);
    }
    private void Start()
    {
        SetSkillData();
        SkillManager.Instance.OnSetUsedSkill += SetSkillData;
    }
    public void UsedSkillSlotButton()
    {
        SkillManager.Instance.UsedSlotButton(slotindex);
    }
    public void SetSkillData()
    {
        if (SkillManager.Instance.curUsedSkills[slotindex] == -1)
        { 
            skillImage.sprite = null;
            skillImage.enabled = false;
        }
        else
        {
            SkillData data = SkillManager.Instance.skilldatas[SkillManager.Instance.curUsedSkills[slotindex]];
            skillImage.enabled = true;
            skillImage.sprite = data.skillIcon;
        }
    }
}
