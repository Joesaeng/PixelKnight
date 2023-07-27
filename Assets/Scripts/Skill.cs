using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject[] skills;
    public SkillData data;
    GameObject curSkill;
    public void SetData(int index)
    {
        curSkill = skills[index];
        data = DataManager.Instance.GetSkillData(index);
    }
    private void OnEnable()
    {
        if (curSkill == null) return;
        StartCoroutine(PlaySkillAnimation());
    }
    IEnumerator PlaySkillAnimation()
    {
        curSkill.SetActive(true);

        yield return new WaitForSeconds(data.animationLength);

        curSkill.SetActive(false);
        gameObject.SetActive(false);
    }
}
