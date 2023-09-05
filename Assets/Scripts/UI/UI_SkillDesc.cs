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
public class UI_SkillDesc : MonoBehaviour, IPointerUpHandler
{
    // 스킬 메뉴 내에서 스킬의 설명을 담당하는 UI 입니다.
    [Header("Skill Info")]
    public SkillData skillData;         // public 으로 스킬데이터를 세팅합니다.
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;

    Outline outline;
    Button selectButton;

    public SkillSelectData selectData;
    public Action<UI_SkillDesc> OnSelect;
    public Action<SkillName> OnEnableSkill;

    [Header("Buy Skill")]
    public GameObject disableImage;
    public TextMeshProUGUI cost;
    bool canUse = false;
    private void Awake()
    {
        outline = GetComponent<Outline>();
        selectData.select = false;
        selectData.skillData = skillData;
        outline.enabled = selectData.select;
        selectButton = GetComponent<Button>();
    }
    private void Start()
    {
        if (skillData)
        {
            skillIcon.enabled = true;
            skillIcon.sprite = skillData.skillIcon;
            skillName.text = skillData.skillNameString;
            skillDesc.text = skillData.slillDesc;
            cost.text = skillData.goldCost.ToString() + " 골드로 구매 가능";
        }
        else
        {
            skillIcon.sprite = null;
            skillName.text = "업데이트 예정";
            skillDesc.text = "";
            selectButton.enabled = false;
            disableImage.SetActive(false);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(canUse)
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
        else
        {
            // 현재 선택된 스킬이 사용이 불가능한 상태일 때,
            // 게임매니저에서 스킬을 배우는데 필요한 골드를 보유했는지 확인한 후
            // 스킬을 사용가능하게 바꿉니다.
            if(GameManager.Instance.GetCurGold() >= skillData.goldCost)
            {
                GameManager.Instance.ModifyGold(-skillData.goldCost);
                EnableSkill();
            }
        }
    }
    public void EnableSkill()
    {
        canUse = true;
        OnEnableSkill?.Invoke(skillData.skillName);
        disableImage.SetActive(false);
    }
    public void SetSelect(bool select)
    {
        selectData.select = select;
        outline.enabled = selectData.select;
    }
    public SkillData GetSelectData()
    {
        return selectData.skillData;
    }
}
