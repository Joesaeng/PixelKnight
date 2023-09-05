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

    bool isDead;
    public bool IsDead
    { get { return isDead; } }
    public bool isAttacking;
    public WaitForSeconds attackDelay;
    public int nextMove;
    float moveSpeed;
    Vector3 xFlipScale;

    public float thinkTime = 5f;
    float curThinkTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<EnemyStatus>();
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        attackDelay = new WaitForSeconds(1.5f);
        curThinkTime = 0f;
    }

    private void OnEnable()
    {
        curThinkTime = 0f;
        initPosition = transform.position;
        nextMove = 0;
        target = null;
        isDead = false;
        isAttacking = false;
        anim.SetBool("isDead", isDead);
        enemyStatus.OnEnemyDead += EnemyDead;
    }
    public void SetData(int enemyID)
    {
        anim.runtimeAnimatorController = animCon[enemyID];
        moveSpeed = enemyStatus.moveSpeed;
        attackType = DataManager.Instance.GetEnemyData(enemyID).attackType;
        attackRange = DataManager.Instance.GetEnemyData(enemyID).attackRange;
    }
    private void Update()
    {
        curThinkTime += Time.deltaTime;
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
            nextMove = (target.position.x - rigid.position.x) > 0 ? 1 : -1;
            Chase();
        }
        else
        {
            if (curThinkTime >= thinkTime)
                Think();
            Move();
        }
        if (rigid.velocity.y <= -50f) // 맵 밖으로 이탈했을때 강제로 사망 처리
            enemyStatus.ModifyHp(-10000000f);
    }
    private void LateUpdate()
    {
        anim.SetFloat("WalkSpeed", Mathf.Abs(rigid.velocity.x));
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
                    Vector2 dir = target.position - rigid.position;
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
                    float dis = Vector2.Distance(target.position, rigid.position);
                    if (!isAttacking && dis <= attackRange)
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
        }
        else if ((rayHit.collider == null && target != null) || // 타겟을 쫓는 중에 더이상 진행할 수 없다면
            (target != null && Vector2.Distance(rigid.position, target.position) > 20f) // 타겟과의 거리가 일정거리만큼 벌어졌다면
            || (player != null && player.IsDead)) // 플레이어가 죽었을 때
        {
            target = null;
            player = null;
            enemyStatus.ResetHpPoise();
            curThinkTime = 0f;
            // 타겟
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);
        curThinkTime = 0f;
    }
    void SetTarget()
    {
        if (target != null) return;
        player = GameManager.Instance.player;
        target = player.GetComponent<Rigidbody2D>();
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
        curThinkTime = 0f;
        GetComponentInParent<SpawnPoint>()?.EnemyDead();
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
                    bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    bullet.GetComponent<Bullet>().Init(dir, enemyStatus);
                }
                break;
        }

    }
}
