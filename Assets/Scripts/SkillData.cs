using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillName
{
    Dash,
    Judgement,
    NULL,
}
[CreateAssetMenu(fileName = "Skill", menuName = "SkillData")]
public class SkillData : ScriptableObject
{
    public int skillId;
    public SkillName skillName;
    public string skillNameString;
    public Sprite skillIcon;
    public float skillCoolTime;
    public int goldCost;
    public float skillSpeed;
    public float animationLength;
    public float damageRatio;
    public float range;
    public float staminaUsage;
    [TextArea]
    public string skillDesc;
}
