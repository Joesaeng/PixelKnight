using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurUsedSkills : MonoBehaviour
{
    // 현재 사용중인 스킬의 이미지를 활성화 및 쿨타임을 업데이트하는 UI입니다.
    public Image[] images;
    public Image[] hideImage;
    UI_SkillMenu skillUI;
    PlayerSkills playerSkills;
    private void Awake()
    {
        skillUI = GetComponent<UI_SkillMenu>();
        skillUI.OnChangedUsedSkill += ChangeCurSkills;
        for(int i = 0; i < hideImage.Length; ++i)
        {
            hideImage[i].enabled = false;
        }
    }
    public void Init(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
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
        hideImage[index].fillAmount =
        Utils.Percent(playerSkills.GetCurCoolTime(index), playerSkills.GetCoolTime(index));
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
