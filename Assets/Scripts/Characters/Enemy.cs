/*
 * Enemy�� �̵�, �÷��̾� �߰�, ����, �ǰ� �� �ִϸ��̼��� ����ϴ� ��ũ��Ʈ�Դϴ�. 
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
    // ������ ������Ʈ��
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected EnemyStatus enemyStatus;
    [SerializeField]
    protected State curState;
    // ���� ����
    protected Rigidbody2D target;       // ������ �ʿ��� Ÿ���Դϴ�.
    protected Rigidbody2D attackTarget; // Enemy�� ���ݹ��� ���� Player�� �ִٸ� Player�� ��� �����Դϴ�.
    protected Player player;
    private EnemyAttackType attackType;
    protected float attackRange;
    private BulletName bulletName;

    [SerializeField]
    RuntimeAnimatorController[] animCon;

    // �̵� �� ���� ���� ����
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
            // attackType�� ���� ���� �ٸ� ������ �ϰ� �˴ϴ�.
            switch (attackType)
            {
                case EnemyAttackType.Meele: // �������� Ÿ��
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
        if (rigid.velocity.y <= -50f && curState != State.Dead) // �� ������ ��Ż������ ������ ��� ó��
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
        // ���� �����Ǻ��� ��¦ �տ��� �ٴ��� ���� ��� ���̸� �����Ͽ� �տ� ���� �ִ����� Ȯ���մϴ�.
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
            rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("AirFloor"));
        if (rayHit.collider == null && target == null)
        {
            nextMove *= -1;
            rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);
        }
        else if ((rayHit.collider == null && target != null) ||                         // Ÿ���� �Ѵ� �߿� ���̻� ������ �� ���ٸ�
            (target != null && Vector2.Distance(rigid.position, target.position) > 20f) 
            || (player != null && player.IsDead))                                       // �÷��̾ �׾��� ��
        {
            // ��ϵ� Ÿ�ٰ� �÷��̾ null�� �о��ְ� ü�� �� �������� �ʱ�ȭ�մϴ�.
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
        // �÷��̾� ��ų �� Dash ��ų�� ������ƮǮ���� �������鼭 Enemy�� �浹�ϱ� ������
        // �ʿ��� �޼��� �Դϴ�. ���� Ÿ���õǴ� ��ų�� �ƴ� ���������� ������ ��ų�� ���� �� ��
        // ��� �����մϴ�.
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
        // �÷��̾��� �⺻������ �ǰ��� ����մϴ�.
        if (IsDead()) return;
        SetTarget();
        if (enemyStatus.CalculatedHit(playerstatus))
        {
            enemyStatus.ModifyPoise(playerstatus.dPlayerFixedStatus[FixedStatusName.Stagger]);
        }

    }
    public void JudgementHit()
    {
        // �÷��̾� ��ų �� Judgement�� �ǰ��� �����ϴ� �޼����Դϴ�.
        if (curState == State.Dead) return;
        SetTarget();
        if (enemyStatus.CalculatedHit(player.playerStatus, DataManager.Instance.GetSkillData((int)SkillName.Judgement)))
        {
        }
    }


    // Player�� Enemy�� ���ݹ��� �ȿ� �ִ����� OnTrigger�� ���� Ȯ���մϴ�.
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

    // Enemy�� ���� �ִϸ��̼ǿ��� ȣ��Ǵ� �̺�Ʈ �޼��� �Դϴ�.
    public virtual void BeginAttack()
    {
        switch (attackType)
        {
            case EnemyAttackType.Meele:
                if (attackTarget != null)
                {
                    // Ÿ���� ���ݹ��� �ȿ� ���� �� Ÿ���� �ǰ� �޼��带 �����մϴ�.
                    attackTarget.GetComponent<Player>().Hit(enemyStatus);
                }
                break;
            case EnemyAttackType.Ranged:
                {
                    // Bullet �� ������ƮǮ���� ���� �Ŀ� Ÿ�� ������ �����մϴ�.
                    Vector3 dir = (target.transform.position - transform.position).normalized;
                    GameObject bullet = PoolManager.Instance.GetBullet(enemyStatus.enemyID);
                    bullet.transform.SetLocalPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, dir));
                    // ������ Bullet ������Ʈ�� ���� Enemy�� �������ͽ� ������ �����մϴ�.
                    bullet.GetComponent<Bullet>().Init(dir, enemyStatus, bulletName);
                }
                break;
        }

    }
}
