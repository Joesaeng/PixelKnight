using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_SkillDesc : MonoBehaviour
{
    // ��ų �޴� ������ ��ų�� ������ ����ϴ� UI �Դϴ�.
    public int descindex;
    public SkillData skillData;
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;

    public Outline outline;
    public Button button;

    public GameObject disableImage;
    public TextMeshProUGUI cost;
    bool isActive;
    private void Awake()
    {
        outline = GetComponent<Outline>();
        button = GetComponent<Button>();
        
    }
    private void OnEnable()
    {
        outline.enabled = false;
        button.enabled = skillData != null;
        disableImage.SetActive(!isActive);
        cost.enabled = !isActive;
    }
    public void SetData(SkillData data)
    {
        skillData = data;
        skillIcon.sprite = data.skillIcon;
        skillIcon.enabled = true;
        skillName.text = data.skillNameString;
        skillDesc.text = data.skillDesc;
        cost.text = data.goldCost.ToString() + "���� ���� ����";
        button.enabled = true;
        button.onClick.AddListener(SkillDescButton);
    }
    public void ActiveSkill()
    {
        isActive = true;
    }
    public void SkillDescButton()
    {
        SkillManager.Instance.SkillDescButton(descindex);
    }
}
