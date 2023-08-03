using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
public struct SkillSelectData
{
    public bool select;
    public SkillData skillData;
}
public class SkillDescUI : MonoBehaviour, IPointerUpHandler
{
    public SkillData skillData;
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;
    Outline outline;
    public SkillSelectData selectData;
    public Action<SkillDescUI> OnSelect;
    private void Awake()
    {
        outline = GetComponent<Outline>();
        selectData.select = false;
        selectData.skillData = skillData;
        outline.enabled = selectData.select;
    }
    private void Start()
    {
        if (skillData)
        {
            skillIcon.enabled = true;
            skillIcon.sprite = skillData.skillIcon;
            skillName.text = skillData.skillNameString;
            skillDesc.text = skillData.slillDesc;
        }
        else
        {
            skillIcon.sprite = null;
            skillName.text = "업데이트 예정";
            skillDesc.text = "";
            GetComponent<Button>().enabled = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (selectData.select)
        {
            selectData.select = false;
            OnSelect?.Invoke(null);
        }
        else
        {
            selectData.select = true;
            OnSelect?.Invoke(this);

        }
        outline.enabled = selectData.select;

    }
    public void SetSelect(bool b)
    {
        selectData.select = b;
        outline.enabled = selectData.select;
    }
    public SkillData GetSelectData()
    {
        return selectData.skillData;
    }
}
