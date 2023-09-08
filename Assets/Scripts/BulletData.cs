using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletName
{
    GoblinAx,


    None,
}
[CreateAssetMenu(fileName = "Bullet", menuName = "BulletData")]
public class BulletData : ScriptableObject
{
    public BulletName bulletName;
    public float durTime;
    public float speed;
}
