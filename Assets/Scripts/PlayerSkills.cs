using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    // �÷��̾� ������Ʈ�� ������Ʈ�� �����Ǵ� ��ų Ŭ���� �Դϴ�.

    // ���� ������� ��ų���� ������
    Player player;
    SkillManager skill;

    public Collider2D judgementRange;
    public bool UseSkill(int slotnum) 
        // ��ų Ű �Է½� Player���� ȣ���ϴ� �޼����Դϴ�.
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
