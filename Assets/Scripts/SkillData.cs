using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillData")]
public class SkillData : ScriptableObject
{
    public int skillId;
    public string skillName;
    public float damageRatio;
    public float range;
    public float staminaUsage;
}
