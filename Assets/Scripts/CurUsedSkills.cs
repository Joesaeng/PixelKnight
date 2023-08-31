using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurUsedSkills : MonoBehaviour
{
    public Image[] images;
    public Image[] hideImage;
    SkillUI skillUI;
    private void Awake()
    {
        skillUI = GetComponent<SkillUI>();
        skillUI.OnChangedUsedSkill += ChangeCurSkills;
        for(int i = 0; i < hideImage.Length; ++i)
        {
            hideImage[i].enabled = false;
        }
    }
    private void Update()
    {
        for(int i = 0; i < images.Length;++i)
        {
            if (images[i].enabled == true)
                CoolTimeUpdate(i);
        }
    }
    void CoolTimeUpdate(int index)
    {
        PlayerSkills skills = GameManager.Instance.player.skills;
        hideImage[index].fillAmount =
        Utils.Percent(skills.GetCurCoolTime(index),skills.GetCoolTime(index));
    }
    void ChangeCurSkills(UsedSkills skills)
    {
        for (int i = 0; i < images.Length; ++i)
            images[i].enabled = false;
        if (skills.skill_1 != null)
        {
            images[0].sprite = skills.skill_1.skillIcon;
            images[0].enabled = true;
            hideImage[0].enabled = true;
            hideImage[0].fillAmount = 0f;
        }
        if (skills.skill_2 != null)
        {
            images[1].sprite = skills.skill_2.skillIcon;
            images[1].enabled = true;
            hideImage[1].enabled = true;
            hideImage[1].fillAmount = 0f;
        }
        if (skills.skill_3 != null)
        {
            images[2].sprite = skills.skill_3.skillIcon;
            images[2].enabled = true;
            hideImage[2].enabled = true;
            hideImage[2].fillAmount = 0f;
        }
        if (skills.skill_4 != null)
        {
            images[3].sprite = skills.skill_4.skillIcon;
            images[3].enabled = true;
            hideImage[3].enabled = true;
            hideImage[3].fillAmount = 0f;
        }
    }
}
