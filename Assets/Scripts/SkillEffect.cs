using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    // 풀매니저에 있는 스킬 프리펩에 붙는 컴포넌트입니다.
    public GameObject[] skills;
    public GameObject[] enemySkills;
    public SkillData data;
    float length;
    [SerializeField]
    float[] enemySkillLength;
    GameObject curSkill;
    public void SetPlayerSKill(SkillName name)
    {
        curSkill = skills[(int)name];
        data = DataManager.Instance.GetSkillData((int)name);
        length = data.animationLength;
    }
    public void SetEnemySKill(int index)
    {
        curSkill = enemySkills[index];
        length = enemySkillLength[index];
    }
    private void OnEnable()
    {
        if (curSkill == null) return;
        StartCoroutine(PlaySkillAnimation());
    }
    IEnumerator PlaySkillAnimation()
    {
        curSkill.SetActive(true);

        yield return new WaitForSeconds(length);

        curSkill.SetActive(false);
        gameObject.SetActive(false);
    }
}
