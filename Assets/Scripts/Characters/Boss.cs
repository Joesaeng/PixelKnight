using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    new readonly WaitForSeconds stunTime = new WaitForSeconds(2f);
    new readonly WaitForSeconds thinkTime = new WaitForSeconds(3f);
    private BossSpell curBossSpell = BossSpell.None;
    public GameObject roarRange;
    Dictionary<BossSpell, WaitForSeconds> cooltimes = new();
    Dictionary<BossSpell, bool> isCooltimes = new();

    bool isSpell = false;
    bool isJump = false;
    Vector2 jumpAttackPos;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<BossStatus>();
        roarRange.SetActive(false);
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        enemyStatus.OnEnemyDead += EnemyDead;
    }
    protected override void OnEnable()
    {

    }
    private void Start()
    {
        isStun = false;
        isAttack = false;
        isDead = false;
        anim.SetBool("isDead", isDead);
        anim.SetBool("isStun", isStun);
        enemyStatus.OnEnemyDead += EnemyDead;
        StartCoroutine(CoSetTarget());
    }
    public override void SetData(int enemyID)
    {
        attackDelay = new WaitForSeconds(enemyStatus.attackDelay);
        moveSpeed = enemyStatus.moveSpeed;
        EnemyData tempdata = DataManager.Instance.GetEnemyData(enemyID);
        if (tempdata is BossData data)
        {
            for (int i = 0; i < data.bossSpells.Count; ++i)
            {
                cooltimes.Add(data.bossSpells[i], new WaitForSeconds(data.cooltimes[i]));
                isCooltimes.Add(data.bossSpells[i], false);
            }
        }
        else
            Debug.LogError("보스 데이터에 잘못된 값 할당");
    }
    protected override void UpdateState()
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
            case State.Stun:
                StartCoroutine(CoStateStun());
                break;
            case State.Attack:
                StartCoroutine(CoStateAttack());
                break;
            case State.Dead:
                StartCoroutine(CoStateDead());
                break;
            case State.Spell:
                StartCoroutine(CoStateSpell());
                break;
            default:
                SetState(State.Init);
                break;
        }
    }

    IEnumerator CoSetTarget() // temp
    {
        yield return new WaitForSeconds(5f);
        SetTarget();
    }
    protected override IEnumerator CoStateIdle()
    {
        while (isStun == false && curState == State.Idle)
        {
            nextMove = target.position.x - rigid.position.x > 0 ? 1 : -1;
            if (target != null)
            {
                SetState(State.Chase);
                yield break;
            }
            yield return thinkTime;
        }
        yield break;
    }
    protected override IEnumerator CoStateChase()
    {
        while (isStun == false && curState == State.Chase && target != null)
        {
            nextMove = target.position.x - rigid.position.x > 0 ? 1 : -1;
            float distance = Vector2.Distance(target.position, rigid.position);
            if (isSpell == false && distance < 5f && isCooltimes[BossSpell.Roar] == false)
            {
                curBossSpell = BossSpell.Roar;
                SetState(State.Spell);
                yield break;
            }
            if (isSpell == false && distance >= 4f && isCooltimes[BossSpell.JumpAttack] == false)
            {
                curBossSpell = BossSpell.JumpAttack;
                SetState(State.Spell);
                yield break;
            }
            if (attackTarget != null && isAttack == false)
            {
                SetState(State.Attack);
                yield break;
            }
            yield return chaseTime;
        }
        yield break;
    }
    IEnumerator CoStateSpell()
    {
        if (isSpell || isStun) yield break;
        isSpell = true;
        nextMove = target.position.x - rigid.position.x > 0 ? 1 : -1;
        switch (curBossSpell)
        {
            case BossSpell.Roar:
                StartCoroutine(CoCooltimeRoar());
                anim.SetTrigger("roar");
                roarRange.SetActive(true);
                yield return attackDelay;
                isSpell = false;
                SetState(State.Idle);
                break;
            case BossSpell.JumpAttack:
                StartCoroutine(CoCooltimeJumpAttack());
                anim.SetBool("jumpStart", true);
                yield return new WaitForSeconds(0.5f);
                while (isJump)
                {
                    if (isStun) yield break;
                    yield return null;
                }
                anim.SetTrigger("jumplanding");
                isSpell = false;
                yield return attackDelay;
                SetState(State.Idle);
                break;
            default:
                isSpell = false;
                SetState(State.Idle);
                break;
        }
        curBossSpell = BossSpell.None;
    }
    IEnumerator CoCooltimeRoar()
    {
        isCooltimes[BossSpell.Roar] = true;
        yield return cooltimes[BossSpell.Roar];
        isCooltimes[BossSpell.Roar] = false;
    }
    IEnumerator CoCooltimeJumpAttack()
    {
        isCooltimes[BossSpell.JumpAttack] = true;
        yield return cooltimes[BossSpell.JumpAttack];
        isCooltimes[BossSpell.JumpAttack] = false;
    }
    protected override IEnumerator CoStateStun()
    {
        if (isStun) yield break;
        isStun = true;
        isAttack = false;
        isSpell = false;
        if (isJump) rigid.gravityScale = 2f;
        anim.SetBool("isStun", isStun);
        anim.SetTrigger("isHurt");
        yield return stunTime;
        isStun = false;
        enemyStatus.ResetPoise();
        anim.SetBool("isStun", isStun);
        SetState(State.Idle);
    }
    protected override IEnumerator CoStateDead()
    {
        Spawner.instance.ItemSpawn(transform.position);
        anim.SetTrigger("Death");
        anim.SetBool("isDead", IsDead());
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
    protected override void FixedUpdate()
    {
        if (IsDead() || isStun)
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            return;
        }
        if (isJump)
        {
            rigid.position = Vector2.Lerp(rigid.position, jumpAttackPos, Time.fixedDeltaTime * 5f);
            float distance = Mathf.Abs(rigid.position.x - jumpAttackPos.x);
            if (distance <= 0.2f)
                isJump = false;
            return;
        }
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
    public override void BeginAttack()
    {
        if (attackTarget != null)
            attackTarget.GetComponent<Player>().Hit(enemyStatus);
    }
    public void Roar()
    {
        float distance = Vector2.Distance(target.position, rigid.position);
        roarRange.SetActive(false);
        if (distance < 5f)
        {
            target.GetComponent<Player>().Hit(enemyStatus, BossSpell.Roar);
        }
    }
    public void Jump()
    {
        rigid.gravityScale = 0f;
        jumpAttackPos = target.position;
        isJump = true;
        GameObject eft = PoolManager.Instance.GetEnemySkill(0);
        eft.transform.position = transform.position;
        eft.transform.localScale = transform.localScale;
        rigid.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

    }
    public void Landing()
    {
        rigid.MovePosition(new Vector2(jumpAttackPos.x,-0.2f));
        rigid.velocity = Vector2.zero;
        GameObject eft = PoolManager.Instance.GetEnemySkill(1);
        eft.transform.position = new Vector2(jumpAttackPos.x, -0.5f);
        eft.transform.localScale = transform.localScale;
        rigid.gravityScale = 2f;
        float distance = Mathf.Abs(target.position.x - rigid.position.x);
        if(distance <= 1.5f)
        {
            target.GetComponent<Player>().Hit(enemyStatus, BossSpell.JumpAttack);
        }
    }
    public override void Stun()
    {
        SetState(State.Stun);
    }
}
/*
 * 플레이어 추적
 * 상태기계
 * 애니메이션
 * 이동
 */