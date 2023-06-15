using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Main Info")]
    public string charName;
    public string attackAnimationName;

    [Header("Basic Status")]
    public int vitality;
    public int endurance;
    public int strength;
    public int dexterity;
    public int luck;
    public float minAttackSpeed;
    public float maxAttackSpeed;
}