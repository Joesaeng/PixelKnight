using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    EnemyStatus enemyStatus;
    public bool isHit = false;
    public int nextMove;
    float moveSpeed;
    Vector3 xFlipScale;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<EnemyStatus>();
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        moveSpeed = enemyStatus.moveSpeed;

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove*moveSpeed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1,LayerMask.GetMask("Floor"));
        if(rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 5);
        }
    }
    private void LateUpdate()
    {
        if (nextMove != 0)
        {
            xFlipScale.x = nextMove;
            transform.localScale = xFlipScale;
        }
    }
    void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", Random.Range(2f, 6f));

        anim.SetInteger("WalkSpeed", nextMove);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackRange") && !isHit)
        {
            Debug.Log("Enemy Hit!");
            isHit = true;
            StartCoroutine(ResetHitState(GameManager.Instance.player.attackDelay * 0.5f));
        }
    }
    IEnumerator ResetHitState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isHit = false;
    }
    /*
    
    Enemy 스크립트 -공격- 구상---
    1. 기본적으로는 비선공몬스터.
    2. 플레이어에게 공격을 받으면 공격을 한 플레이어를 타겟으로 설정하여 따라가고
       공격하는 방식.
    3. 선공몬스터를 할때는 스캔을 넣어서 그 안에 들어오면 타겟으로 설정하고 따라간다.
    4. AttackRange 안에 들어오면 공격을 한다.
    5. 공격은 공격 애니메이션에 맞춰서 콜라이더를 OnEnable 시키고 그 프레임에
       Player와 충돌이 일어나면 Player의 콜라이더에서 Player를 가져와서
       Player의 TakeDamage 메서드를 실행시킨다.
     */
}
