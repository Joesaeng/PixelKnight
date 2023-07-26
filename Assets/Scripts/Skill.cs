using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject[] skills;
    public SkillData data;
    GameObject curSkill;
    Animator animator;
    public void SetData(int index)
    {
        curSkill = skills[index];
        data = DataManager.Instance.GetSkillData(index);
        animator = curSkill.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if (curSkill == null) return;
        StartCoroutine(PlaySkillAnimation());
    }
    IEnumerator PlaySkillAnimation()
    {
        curSkill.SetActive(true);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        curSkill.SetActive(false);
        gameObject.SetActive(false);
    }

}
