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
    // ��ų �޴� ������ ��ų�� ������ ����ϴ� UI �Դϴ�.
    [Header("Skill Info")]
    public SkillData skillData;         // public ���� ��ų�����͸� �����մϴ�.
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
            cost.text = skillData.goldCost.ToString() + " ���� ���� ����";
        }
        else
        {
            skillIcon.sprite = null;
            skillName.text = "������Ʈ ����";
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
            // ���� ���õ� ��ų�� ����� �Ұ����� ������ ��,
            // ���ӸŴ������� ��ų�� ���µ� �ʿ��� ��带 �����ߴ��� Ȯ���� ��
            // ��ų�� ��밡���ϰ� �ٲߴϴ�.
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
