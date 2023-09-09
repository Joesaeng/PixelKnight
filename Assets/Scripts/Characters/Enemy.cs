/*
 * Enemy�� �̵�, �÷��̾� �߰�, ����, �ǰ� �� �ִϸ��̼��� ����ϴ� ��ũ��Ʈ�Դϴ�. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ������ ������Ʈ��
    private Rigidbody2D rigid;
    private Animator anim;
    private EnemyStatus enemyStatus;

    // ���� ����
    private Rigidbody2D target;
    private Rigidbody2D attackTarget; // Enemy�� ���ݹ��� ���� Player�� �ִٸ� Player�� ��� �����Դϴ�.
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


    // �̵� �� ���� ���� ����
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

        if (rigid.velocity.y <= -50f) // �� ������ ��Ż������ ������ ��� ó��
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
    void Chase() // Ÿ���� �����ϴ� ����� �ϴ� �޼����Դϴ�.
    {
        // attackType�� ���� ���� �ٸ� ������ �ϰ� �˴ϴ�.
        switch (attackType)
        {
            case EnemyAttackType.Meele: // �������� Ÿ��
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

        // ���� �����Ǻ��� ��¦ �տ��� �ٴ��� ���� ��� ���̸� �����Ͽ� �տ� ���� �ִ����� Ȯ���մϴ�.
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
            rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("AirFloor"));
        if (rayHit.collider == null && target == null)
        {
            nextMove *= -1;
        }
        else if ((rayHit.collider == null && target != null) ||                         // Ÿ���� �Ѵ� �߿� ���̻� ������ �� ���ٸ�
            (target != null && Vector2.Distance(rigid.position, target.position) > 20f) // Ÿ�ٰ��� �Ÿ��� �����Ÿ���ŭ �������ٸ�
            || (player != null && player.IsDead))                                       // �÷��̾ �׾��� ��
        {
            // ��ϵ� Ÿ�ٰ� �÷��̾ null�� �о��ְ� ü�� �� �������� �ʱ�ȭ�մϴ�.
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
        // �÷��̾� ��ų �� Dash ��ų�� ������ƮǮ���� �������鼭 Enemy�� �浹�ϱ� ������
        // �ʿ��� �޼��� �Դϴ�. ���� Ÿ���õǴ� ��ų�� �ƴ� ���������� ������ ��ų�� ���� �� ��
        // ��� �����մϴ�.
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
        // �÷��̾��� �⺻������ �ǰ��� ����մϴ�.
        if (IsDead) return;
        SetTarget();
        if (enemyStatus.CalculatedHit(playerstatus))
        {
            anim.SetTrigger("isHit");
        }

    }
    public void JudgementHit() 
    {
        // �÷��̾� ��ų �� Judgement�� �ǰ��� �����ϴ� �޼����Դϴ�.
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
    public void BeginAttack()
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
