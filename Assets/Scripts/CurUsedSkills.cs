using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurUsedSkills : MonoBehaviour
{
    public Image[] images;
    SkillUI skillUI;
    private void Awake()
    {
        skillUI = GetComponentInParent<SkillUI>();
        skillUI.OnChangedUsedSkill += ChangeCurSkills;
    }
    void ChangeCurSkills(UsedSkills skills)
    {
        for (int i = 0; i < images.Length; ++i)
            images[i].enabled = false;
        if (skills.skill_1 != null)
        {
            images[0].sprite = skills.skill_1.skillIcon;
            images[0].enabled = true;
        }
        if (skills.skill_2 != null)
        {
            images[1].sprite = skills.skill_2.skillIcon;
            images[1].enabled = true;
        }
        if (skills.skill_3 != null)
        {
            images[2].sprite = skills.skill_3.skillIcon;
            images[2].enabled = true;
        }
        if (skills.skill_4 != null)
        {
            images[3].sprite = skills.skill_4.skillIcon;
            images[3].enabled = true;
        }
    }
}
