using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    EnemyStatus enemyStatus;
    Rigidbody2D target;
    Collider2D attackRange;

    public RuntimeAnimatorController[] animCon;

    Vector2 initPosition;
    public bool isHit;
    bool isDead;
    public bool isAttacking;
    public WaitForSeconds attackDelay;
    public int nextMove;
    float moveSpeed;
    Vector3 xFlipScale;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<EnemyStatus>();
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        attackDelay = new WaitForSeconds(1.5f);
        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols)
        {
            if (col.transform == transform)
                continue;
            attackRange = col;
        }
    }

    private void OnEnable()
    {
        initPosition = transform.position;
        moveSpeed = enemyStatus.moveSpeed;
        isHit = false;
        isDead = false;
        isAttacking = false;
        //enemyStatus.SetData();
        anim.runtimeAnimatorController = animCon[enemyStatus.enemyID];
        anim.SetBool("isDead", isDead);
        Invoke("Think", 5);
        enemyStatus.OnEnemyDead += EnemyDead;
    }

    void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || isDead || isAttacking)
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            anim.SetFloat("WalkSpeed", 0f);
            return;
        }
        if(target)
        {
            CancelInvoke();
            Chase();
        }
        else
        {
            Move();
        }
        anim.SetFloat("WalkSpeed", Mathf.Abs(rigid.velocity.x));
    }
    private void LateUpdate()
    {
        if (nextMove != 0)
        {
            xFlipScale.x = nextMove;
            transform.localScale = xFlipScale;
        }
    }

    void Chase()
    {
        nextMove = rigid.position.x - target.position.x >= 0f ? -1 : 1;
        Vector2 dir = target.position - rigid.position;
        RaycastHit2D ray = Physics2D.Raycast(rigid.position, dir, 0.7f, LayerMask.GetMask("Player"));
        
        if(!isAttacking && ray.collider != null && ray.collider.CompareTag("Player"))
        {
            StartCoroutine(CoAttack());
        }
        else
        {
            Move();
        }
    }
    IEnumerator CoAttack()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");
        yield return attackDelay;
        isAttacking = false;
        Move();
    }
    void Move()
    {
        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 5);
        }
    }

    void Think()
    {
        
        nextMove = Random.Range(-1, 2);

        Invoke("Think", Random.Range(2f, 6f));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackRange") && !isHit && !isDead)
        {
            if (target == null) target = collision.gameObject.GetComponentInParent<Rigidbody2D>();
            isHit = true;
            if (!enemyStatus.CalculatedHit(collision.gameObject.GetComponentInParent<PlayerStatus>()))
            {
                
            }
            else
            {
                anim.SetTrigger("isHit");
            }
            
            StartCoroutine(ResetHitState(GameManager.Instance.player.attackDelay * 0.5f));
        }
    }
    IEnumerator ResetHitState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isHit = false;
    }

    void EnemyDead()
    {
        isDead = true;
        CancelInvoke();
        StartCoroutine(EnemyDeadAnimPlay());
    }
    IEnumerator EnemyDeadAnimPlay()
    {
        anim.SetBool("isDead", isDead);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    public void BeginAttack()
    {
        attackRange.enabled = true;
    }
    public void EndAttack()
    {
        attackRange.enabled = false;
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
