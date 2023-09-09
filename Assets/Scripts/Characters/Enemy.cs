/*
 * Enemy의 이동, 플레이어 추격, 공격, 피격 및 애니메이션을 담당하는 스크립트입니다. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 보유한 컴포넌트들
    private Rigidbody2D rigid;
    private Animator anim;
    private EnemyStatus enemyStatus;

    // 공격 관련
    private Rigidbody2D target;
    private Rigidbody2D attackTarget; // Enemy의 공격범위 내에 Player가 있다면 Player를 담는 변수입니다.
    private Player player;
    private EnemyAttackType attackType;
    private float attackRange;
    private BulletName bulletName;
    
    [SerializeField]
    RuntimeAnimatorController[] animCon;

    private bool isDead;
    public bool IsDead { get => isDead; set => isDead = value; }
    private bool isAttacking;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }


    // 이동 및 공격 관련 변수
    private WaitForSeconds attackDelay;
    private int nextMove;
    private float moveSpeed;
    private Vector3 xFlipScale;

    public float thinkTime = 5f;
    private float curThinkTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<EnemyStatus>();
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        
        curThinkTime = 0f;
    }

    private void OnEnable()
    {
        curThinkTime = 0f;
        nextMove = 0;
        target = null;
        IsDead = false;
        IsAttacking = false;
        anim.SetBool("isDead", IsDead);
        enemyStatus.OnEnemyDead += EnemyDead;
    }
    public void SetData(int enemyID)
    {
        anim.runtimeAnimatorController = animCon[enemyID];
        bulletName = enemyStatus.bulletName;
        attackDelay = new WaitForSeconds(enemyStatus.attackDelay);
        moveSpeed = enemyStatus.moveSpeed;
        attackType = DataManager.Instance.GetEnemyData(enemyID).attackType;
        attackRange = DataManager.Instance.GetEnemyData(enemyID).attackRange;
    }
    private void Update()
    {
        curThinkTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || IsDead || IsAttacking)
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
    void Chase() // 타겟을 추적하는 기능을 하는 메서드입니다.
    {
        // attackType에 따라 조금 다른 동작을 하게 됩니다.
        switch (attackType)
        {
            case EnemyAttackType.Meele: // 근접공격 타입
                {
                    if (!IsAttacking && attackTarget != null)
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
                    if (!IsAttacking && dis <= attackRange)
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
        IsAttacking = true;
        anim.SetTrigger("isAttack");
        yield return attackDelay;
        IsAttacking = false;
    }
    void Move()
    {
        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);

        // 현재 포지션보다 살짝 앞에서 바닥을 향해 쏘는 레이를 생성하여 앞에 길이 있는지를 확인합니다.
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
            rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("AirFloor"));
        if (rayHit.collider == null && target == null)
        {
            nextMove *= -1;
        }
        else if ((rayHit.collider == null && target != null) ||                         // 타겟을 쫓는 중에 더이상 진행할 수 없다면
            (target != null && Vector2.Distance(rigid.position, target.position) > 20f) // 타겟과의 거리가 일정거리만큼 벌어졌다면
            || (player != null && player.IsDead))                                       // 플레이어가 죽었을 때
        {
            // 등록된 타겟과 플레이어를 null로 밀어주고 체력 및 경직도를 초기화합니다.
            target = null;
            player = null;
            enemyStatus.ResetHpPoise();
            curThinkTime = 0f;
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
        // 플레이어 스킬 중 Dash 스킬이 오브젝트풀에서 꺼내오면서 Enemy와 충돌하기 때문에
        // 필요한 메서드 입니다. 추후 타게팅되는 스킬이 아닌 범위공격을 가지는 스킬을 구현 할 때
        // 사용 가능합니다.
        if (collision.CompareTag("PlayerSkillRange") && !IsDead)
        {
            SetTarget();
            if (enemyStatus.CalculatedHit(player.playerStatus, collision.GetComponentInParent<Skill>().data))
            {
                anim.SetTrigger("isHit");
            }
        }
    }
    public void MeeleAttackHit(PlayerStatus playerstatus)
    {
        // 플레이어의 기본공격의 피격을 담당합니다.
        if (IsDead) return;
        SetTarget();
        if (enemyStatus.CalculatedHit(playerstatus))
        {
            anim.SetTrigger("isHit");
        }

    }
    public void JudgementHit() 
    {
        // 플레이어 스킬 중 Judgement의 피격을 관리하는 메서드입니다.
        if (IsDead) return;
        SetTarget();
        enemyStatus.CalculatedHit(player.playerStatus, DataManager.Instance.GetSkillData(SkillName.Judgement));
        anim.SetTrigger("isHit");
    }

    void EnemyDead()
    {
        if (!IsDead) Spawner.instance.ItemSpawn(transform.position);
        IsDead = true;
        curThinkTime = 0f;
        GetComponentInParent<SpawnPoint>()?.EnemyDead();
        StartCoroutine(EnemyDeadAnimPlay());
    }
    IEnumerator EnemyDeadAnimPlay()
    {
        anim.SetBool("isDead", IsDead);
        transform.SetParent(PoolManager.Instance.transform);
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    // Player가 Enemy의 공격범위 안에 있는지를 OnTrigger를 통해 확인합니다.
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
    
    // Enemy의 공격 애니메이션에서 호출되는 이벤트 메서드 입니다.
    public void BeginAttack()
    {
        switch (attackType)
        {
            case EnemyAttackType.Meele:
                if (attackTarget != null)
                {
                    // 타겟이 공격범위 안에 있을 때 타겟의 피격 메서드를 실행합니다.
                    attackTarget.GetComponent<Player>().Hit(enemyStatus);
                }
                break;
            case EnemyAttackType.Ranged:
                {
                    // Bullet 을 오브젝트풀에서 꺼낸 후에 타겟 방향을 전달합니다.
                    Vector3 dir = (target.transform.position - transform.position).normalized;
                    GameObject bullet = PoolManager.Instance.GetBullet(enemyStatus.enemyID);
                    bullet.transform.SetLocalPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, dir));
                    // 생성된 Bullet 오브젝트에 현재 Enemy의 스테이터스 정보를 전달합니다.
                    bullet.GetComponent<Bullet>().Init(dir, enemyStatus, bulletName);
                }
                break;
        }

    }
}
