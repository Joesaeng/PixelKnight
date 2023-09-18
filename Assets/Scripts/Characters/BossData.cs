using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossSpell
{
    Roar,
    JumpAttack,
    None,
};
[CreateAssetMenu(fileName = "Boss", menuName = "BossData")]
public class BossData : EnemyData
{
    [Header("Boss Spell Info")]
    public List<BossSpell> bossSpells;
    public List<float> cooltimes;
    public List<float> damageRatio;
    public List<float> staggerRatio;
}
