using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillName
{
    Dash,
    Judgement
}
[CreateAssetMenu(fileName = "Skill", menuName = "SkillData")]
public class SkillData : ScriptableObject
{
    public int skillId;
    public SkillName skillName;
    public float skillSpeed;
    public float animationLength;
    public float damageRatio;
    public float range;
    public float staminaUsage;
}
