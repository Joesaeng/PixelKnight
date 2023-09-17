using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    // 플레이어 오브젝트에 컴포넌트로 부착되는 스킬 클래스 입니다.

    // 현재 사용중인 스킬들의 데이터
    Player player;
    SkillManager skill;

    public Collider2D judgementRange;
    public bool UseSkill(int slotnum) 
        // 스킬 키 입력시 Player에서 호출하는 메서드입니다.
    {
        bool isUse = false;
        SkillData cur = skill.skilldatas[skill.GetUsedSkill(slotnum)];
        switch (cur.skillName)
        {
            case SkillName.Dash: // Dash
                GameObject skill = PoolManager.Instance.GetSkill(cur.skillName);
                skill.transform.position = transform.position;
                skill.transform.localScale = transform.localScale;
                isUse = true;
                break;
            case SkillName.Judgement: // Judgement
                foreach (var target in player.GetJudgetTarget())
                {
                    if (target.GetComponent<Enemy>().IsDead()) continue;
                    GameObject judge = PoolManager.Instance.GetSkill(cur.skillName);
                    judge.transform.position = target.transform.position;
                }
                isUse = true;
                break;
            default:
                break;
        }
        return isUse;
    }
    private void Start()
    {
        player = GetComponent<Player>();
        skill = SkillManager.Instance;
    }
    
    
}
