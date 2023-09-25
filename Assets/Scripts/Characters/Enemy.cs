/*
 * Enemy의 이동, 플레이어 추격, 공격, 피격 및 애니메이션을 담당하는 스크립트입니다. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    protected enum State
    {
        Init,
        Idle,
        Chase,
        Patrol,
        Stun,
        Attack,
        Dead,
        Spell,
        None,
    };
    // 보유한 컴포넌트들
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected EnemyStatus enemyStatus;
    [SerializeField]
    protected State curState;
    // 공격 관련
    protected Rigidbody2D target;       // 추적에 필요한 타겟입니다.
    protected Rigidbody2D attackTarget; // Enemy의 공격범위 내에 Player가 있다면 Player를 담는 변수입니다.
    protected Player player;
    private EnemyAttackType attackType;
    protected float attackRange;
    private BulletName bulletName;

    [SerializeField]
    RuntimeAnimatorController[] animCon;

    // 이동 및 공격 관련 변수
    protected int nextMove;
    protected float moveSpeed;
    protected Vector3 xFlipScale;

    protected bool isStun;
    protected bool isAttack;
    protected bool isDead;

    protected WaitForSeconds attackDelay;
    protected readonly WaitForSeconds thinkTime = new WaitForSeconds(4f);
    protected readonly WaitForSeconds chaseTime = new WaitForSeconds(0.5f);
    protected readonly WaitForSeconds stunTime = new WaitForSeconds(1f);

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<EnemyStatus>();
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    public virtual void SetData(int enemyID)
    {
        anim.runtimeAnimatorController = animCon[enemyID];
        bulletName = enemyStatus.bulletName;
        attackDelay = new WaitForSeconds(enemyStatus.attackDelay);
        moveSpeed = enemyStatus.moveSpeed;
        attackType = DataManager.Instance.GetEnemyData(enemyID).attackType;
        attackRange = DataManager.Instance.GetEnemyData(enemyID).attackRange;
    }

    protected virtual void OnEnable()
    {
        nextMove = 0;
        target = null;
        isStun = false;
        isAttack = false;
        isDead = false;
        anim.SetBool("isDead", isDead);
        enemyStatus.OnEnemyDead += EnemyDead;
        SetState(State.None);
    }

    protected virtual void UpdateState()
    {
        switch (curState)
        {
            case State.Init:
                StartCoroutine(CoStateInit());
                break;
            case State.Idle:
                StartCoroutine(CoStateIdle());
                break;
            case State.Chase:
                StartCoroutine(CoStateChase());
                break;
            case State.Patrol:
                StartCoroutine(CoStatePatrol());
                break;
            case State.Stun:
                StartCoroutine(CoStateStun());
                break;
            case State.Attack:
                StartCoroutine(CoStateAttack());
                break;
            case State.Dead:
                StartCoroutine(CoStateDead());
                break;
            default:
                SetState(State.Init);
                break;
        }
    }
    protected void SetState(State state)
    {
        if (curState == state) return;
        curState = state;
        UpdateState();
    }
    public bool IsDead()
    {
        return isDead;
    }
    protected IEnumerator CoStateInit()
    {
        yield return new WaitForSeconds(0.5f);
        nextMove = Random.Range(-1, 2);
        SetState(State.Idle);
    }
    protected virtual IEnumerator CoStateIdle()
    {
        while (curState == State.Idle)
        {
            if (target != null)
            {
                SetState(State.Chase);
                yield break;
            }
            if (nextMove != 0)
            {
                SetState(State.Patrol);
                yield break;
            }
            nextMove = Random.Range(-1, 2);
            yield return thinkTime;
        }
        yield break;
    }
    protected virtual IEnumerator CoStateChase()
    {
        while (curState == State.Chase && target != null)
        {
            nextMove = target.position.x - rigid.position.x > 0 ? 1 : -1;
            // attackType에 따라 조금 다른 동작을 하게 됩니다.
            switch (attackType)
            {
                case EnemyAttackType.Meele: // 근접공격 타입
                    {
                        if (attackTarget != null)
                        {
                            SetState(State.Attack);
                            yield break;
                        }
                    }
                    break;
                case EnemyAttackType.Ranged:
                    {
                        float dis = Vector2.Distance(target.position, rigid.position);
                        if (dis <= attackRange)
                        {
                            SetState(State.Attack);
                            yield break;
                        }
                    }
                    break;
            }
            yield return chaseTime;
        }
        yield break;
    }
    protected IEnumerator CoStatePatrol()
    {
        while (curState == State.Patrol)
        {
            if (target != null)
            {
                SetState(State.Chase);
                yield break;
            }
            if (nextMove == 0)
            {
                SetState(State.Idle);
                yield break;
            }
            nextMove = Random.Range(-1, 2);
            yield return thinkTime;
        }
    }
    public virtual void Stun()
    {
        SetState(State.Stun);
    }

    protected virtual IEnumerator CoStateStun()
    {
        if (isStun) yield break;
        isStun = true;
        anim.SetTrigger("isHurt");
        yield return stunTime;
        isStun = false;
        enemyStatus.ResetPoise();
        SetState(State.Idle);
    }
    protected IEnumerator CoStateAttack()
    {
        if (isAttack || isStun) yield break;
        isAttack = true;
        anim.SetTrigger("isAttack");
        yield return attackDelay;
        isAttack = false;
        if (isStun) yield break;
        SetState(State.Idle);
    }
    protected virtual void EnemyDead()
    {
        if (IsDead()) return;
        isDead = true;
        enemyStatus.OnEnemyDead -= EnemyDead;
        SetState(State.Dead);
    }
    protected virtual IEnumerator CoStateDead()
    {
        Spawner.instance.ItemSpawn(transform.position);
        if (GetComponentInParent<SpawnPoint>() == null)
        {
            Debug.LogError("SpawnPointNull");
            yield break;
        }
        GetComponentInParent<SpawnPoint>()?.EnemyDead();
        anim.SetBool("isDead", IsDead());
        transform.SetParent(PoolManager.Instance.transform);
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        if (IsDead())
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            return;
        }
        CheckFloor();
        switch (curState)
        {
            case State.Patrol:
            case State.Chase:
                rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);
                break;
            default:
                rigid.velocity = new Vector2(0f, rigid.velocity.y);
                break;
        }
        if (rigid.velocity.y <= -50f && curState != State.Dead) // 맵 밖으로 이탈했을때 강제로 사망 처리
            enemyStatus.ModifyHp(-10000000f);
    }
    private void LateUpdate()
    {
        anim.SetFloat("WalkSpeed", Mathf.Abs(rigid.velocity.x));
        if (nextMove != 0 && isStun == false)
        {
            xFlipScale.x = nextMove;
            transform.localScale = xFlipScale;
        }
    }
    protected void CheckFloor()
    {
        // 현재 포지션보다 살짝 앞에서 바닥을 향해 쏘는 레이를 생성하여 앞에 길이 있는지를 확인합니다.
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
            rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("AirFloor"));
        if (rayHit.collider == null && target == null)
        {
            nextMove *= -1;
            rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);
        }
        else if ((rayHit.collider == null && target != null) ||                         // 타겟을 쫓는 중에 더이상 진행할 수 없다면
            (target != null && Vector2.Distance(rigid.position, target.position) > 20f) 
            || (player != null && player.IsDead))                                       // 플레이어가 죽었을 때
        {
            // 등록된 타겟과 플레이어를 null로 밀어주고 체력 및 경직도를 초기화합니다.
            target = null;
            player = null;
            enemyStatus.ResetHpPoise();
            SetState(State.Idle);
        }
    }
    protected void SetTarget()
    {
        if (target != null) return;
        player = GameManager.Instance.player;
        target = player.GetComponent<Rigidbody2D>();
        SetState(State.Chase);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 스킬 중 Dash 스킬이 오브젝트풀에서 꺼내오면서 Enemy와 충돌하기 때문에
        // 필요한 메서드 입니다. 추후 타게팅되는 스킬이 아닌 범위공격을 가지는 스킬을 구현 할 때
        // 사용 가능합니다.
        if (collision.CompareTag("PlayerSkillRange") && curState != State.Dead)
        {
            SetTarget();
            if (enemyStatus.CalculatedHit(player.playerStatus, collision.GetComponentInParent<SkillEffect>().data))
            {
            }
        }
    }
    public void MeeleAttackHit(PlayerStatus playerstatus)
    {
        // 플레이어의 기본공격의 피격을 담당합니다.
        if (IsDead()) return;
        SetTarget();
        if (enemyStatus.CalculatedHit(playerstatus))
        {
            enemyStatus.ModifyPoise(playerstatus.dPlayerFixedStatus[FixedStatusName.Stagger]);
        }

    }
    public void JudgementHit()
    {
        // 플레이어 스킬 중 Judgement의 피격을 관리하는 메서드입니다.
        if (curState == State.Dead) return;
        SetTarget();
        if (enemyStatus.CalculatedHit(player.playerStatus, DataManager.Instance.GetSkillData((int)SkillName.Judgement)))
        {
        }
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
    public virtual void BeginAttack()
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
