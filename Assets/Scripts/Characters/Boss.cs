using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField]
    float stunTime;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<BossStatus>();
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        enemyStatus.OnEnemyDead += EnemyDead;
        StartCoroutine(SetTarget());
    }
    IEnumerator SetTarget() // temp
    {
        yield return new WaitForSeconds(3f);
        player = GameManager.Instance.player;
        target = player.GetComponent<Rigidbody2D>();  
    }
    public override void SetData(int enemyID)
    {
        attackDelay = new WaitForSeconds(enemyStatus.attackDelay);
        moveSpeed = enemyStatus.moveSpeed;
    }

    public override void BeginAttack()
    {
        if (attackTarget != null)
            attackTarget.GetComponent<Player>().Hit(enemyStatus);
    }
   
}
/*
 * 플레이어 추적
 * 상태기계
 * 애니메이션
 * 이동
 */