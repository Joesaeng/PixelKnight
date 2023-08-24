using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttackType
{
    Meele,
    Ranged,
}
[CreateAssetMenu(fileName = "Enemy", menuName = "EnemyData")]

public class EnemyData : ScriptableObject
{
    [Header("Main Info")]
    public int enemyID;
    public EnemyAttackType attackType;
    public string charName;
    public float expReward;
    public int goldReward;
    public float attackRange;

    [Header("Basic Status")]
    public float maxHp;
    public float maxPoise;
    public float damage;
    public float defence;
    public float stagger;
    public float evade;
    public float hitrate;
    public float moveSpeed;
}
