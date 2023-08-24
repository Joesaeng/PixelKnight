using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    EnemyStatus enemyStatus;
    Rigidbody2D target;
    Rigidbody2D attackTarget;
    Player player;
    EnemyAttackType attackType;
    float attackRange;

    public RuntimeAnimatorController[] animCon;

    Vector2 initPosition;

    Vector2 targetPos;

    bool isDead;
    public bool IsDead
    { get { return isDead; } }    
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
    }

    private void OnEnable()
    {
        initPosition = transform.position;
        //moveSpeed = enemyStatus.moveSpeed;
        nextMove = 0;
        target = null;
        isDead = false;
        isAttacking = false;
        //enemyStatus.SetData();
        //anim.runtimeAnimatorController = animCon[enemyStatus.enemyID];
        anim.SetBool("isDead", isDead);
        Invoke("Think", 5);
        enemyStatus.OnEnemyDead += EnemyDead;
    }
    public void SetData(int enemyID)
    {
        anim.runtimeAnimatorController = animCon[enemyID];
        moveSpeed = enemyStatus.moveSpeed;
        attackType = DataManager.Instance.GetEnemyData(enemyID).attackType;
        attackRange = DataManager.Instance.GetEnemyData(enemyID).attackRange;
    }
    void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || isDead || isAttacking)
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            anim.SetFloat("WalkSpeed", 0f);
            return;
        }
        if (target)
        {
            //CancelInvoke();
            Chase();
        }
        else
        {
            Move();
        }
        anim.SetFloat("WalkSpeed", Mathf.Abs(rigid.velocity.x));
        if (rigid.velocity.y <= -50f) // 맵 밖으로 이탈했을때 강제로 사망 처리
            enemyStatus.ModifyHp(-10000000f);
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
        
        switch (attackType)
        {
            case EnemyAttackType.Meele:
                {
                    Vector2 dir = targetPos - rigid.position;
                    RaycastHit2D ray = Physics2D.Raycast(rigid.position, dir, 0.7f, LayerMask.GetMask("PlayerHit"));

                    if (!isAttacking && ray.collider != null && ray.collider.CompareTag("PlayerHit"))
                    {
                        StartCoroutine(CoAttack());
                    }
                    else
                    {
                        Move();
                    }
                }
                break;
            case EnemyAttackType.Ranged:
                {
                    float dis = Vector2.Distance(targetPos, rigid.position);
                    if(!isAttacking && dis <= attackRange)
                    {
                        StartCoroutine(CoAttack());
                    }
                    else
                    {
                        Move();
                    }
                }
                break;
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
    void ChaseTarget()
    {
        if (target == null) return;
        else
        {
            targetPos = target.position;
            nextMove = rigid.position.x - targetPos.x >= 0f ? -1 : 1;
        }
        Invoke("ChaseTarget", 1f);
    }
    void Move()
    {
        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
            rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("AirFloor"));
        if (rayHit.collider == null && target == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 5);
        }
        else if((rayHit.collider == null && target != null)|| Vector2.Distance(rigid.position,targetPos) >20f
            || (player != null && player.IsDead))
        {
            target = null;
            player = null;
            enemyStatus.ResetHpPoise();
            Invoke("Think", 5);
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", Random.Range(2f, 6f));
    }
    void SetTarget()
    {
        if (target != null) return;
        player = GameManager.Instance.player;
        target = player.GetComponent<Rigidbody2D>();
        ChaseTarget();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerSkillRange") && !isDead)
        {
            SetTarget();
            if (enemyStatus.CalculatedHit(GameManager.Instance.player.playerStatus, collision.GetComponentInParent<Skill>().data))
            {
                anim.SetTrigger("isHit");
            }
        }
    }
    public void Hit(PlayerStatus playerstatus)
    {
        if (isDead) return;
        SetTarget();
        if (enemyStatus.CalculatedHit(playerstatus))
        {
            anim.SetTrigger("isHit");
        }

    }
    public void JudgementHit()
    {
        if (isDead) return;
        SetTarget();
        enemyStatus.CalculatedHit(GameManager.Instance.player.playerStatus, DataManager.Instance.GetSkillData(SkillName.Judgement));
        anim.SetTrigger("isHit");
    }

    void EnemyDead()
    {
        if (!isDead) Spawner.instance.ItemSpawn(transform.position);
        isDead = true;
        CancelInvoke();
        StartCoroutine(EnemyDeadAnimPlay());
    }
    IEnumerator EnemyDeadAnimPlay()
    {
        anim.SetBool("isDead", isDead);
        transform.SetParent(PoolManager.Instance.transform);
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHit"))
            attackTarget = collision.attachedRigidbody;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHit"))
            attackTarget = null;
    }
    public void BeginAttack()
    {
        switch (attackType)
        {
            case EnemyAttackType.Meele:
                if (attackTarget != null)
                {
                    attackTarget.GetComponent<PlayerStatus>().CalculatedHit(enemyStatus);
                }
                break;
            case EnemyAttackType.Ranged:
                {
                    // Bullet 소환, TargetPos로 방향 전달
                    Vector3 dir = (target.transform.position - transform.position).normalized;
                    GameObject bullet = PoolManager.Instance.GetBullet(enemyStatus.enemyID);
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up,dir);
                    bullet.GetComponent<Bullet>().Init(dir,enemyStatus);
                }
                break;
        }
        
    }
}
