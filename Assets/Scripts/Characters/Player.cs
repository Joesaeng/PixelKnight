using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // ������Ʈ
    Rigidbody2D rigid;           
    Animator animator;           
    Collider2D myCollider;       // �÷��̾��� �߷� �浹�� ���õ� �ݶ��̴� ������Ʈ
    Collider2D ladderCollider;   // ��ٸ��� ����� �� ����ϴ� Collider2D ������Ʈ

    // Ű üũ ����
    public Transform chkPos;    // �÷��̾��� �� ��ġ üũ�� ������
    public Transform frontChk;  // �÷��̾��� ���� ��ġ üũ�� ������
    public PlayerStatus playerStatus;  // �÷��̾��� ���� ���� ������ ���� ��ũ��Ʈ
    public PlayerSkills skills;        // �÷��̾��� ��ų ���� ������ ���� ��ũ��Ʈ

    // �ִϸ��̼� ����
    public List<AnimationClip> animationClips;
    public AnimationClip attackAnimationClip; 
    public RuntimeAnimatorController[] animCon;    // �پ��� ĳ���͸� ���� �ִϸ��̼� ��Ʈ�ѷ� ���

    // ���� ���� ����
    public HashSet<Rigidbody2D> targets = new HashSet<Rigidbody2D>();         // �÷��̾��� ���� ���
    public HashSet<Rigidbody2D> judgementtargets = new HashSet<Rigidbody2D>(); // Ư�� ��ų�� ���
    public string attackAnimationClipName; // ���� �ִϸ��̼� Ŭ�� �̸�


    public float attackSpeed = 1.0f;       // ���� �ӵ�
    private WaitForSeconds WFSattackDelay; // ���� �����̿� WaitForSeconds ��ü

    // ���̾� ���� ����
    private int groundChkLayer;     // �ٴ� ���̾� ����ũ
    private int airGroundChkLayer;  // ���� ���� ���̾� ����ũ
    private int airFloorLayer;      // ���� ���� ���̾�
    private int playerLayer;        // �÷��̾� ���̾�

    // �̵� ���� ����
    Vector3 moveDir;                // �̵� ���� ����
    Vector3 xFlipScale;             // X�� ������ ������ ����
    int xFlip;                       // X�� ���� ���θ� ��Ÿ���� ����
    public float moveSpeed;         // �̵� �ӵ�
    public float jumpForce;         // ���� ��
    public float horizontal;        // ���� �Է°�
    public float vertical;          // ���� �Է°�
    public int jumpCount;           // ���� ���� Ƚ��

    // ���¹̳� ���� ����
    public float jumpStamina = 5f;  // ������ �ʿ��� ���¹̳�
    public float attackStamina = 8f; // ���ݿ� �ʿ��� ���¹̳�

    public float attackDelay;        // ���� ������ �ð�
    private bool isAttacking = false; // ���� �� ����
    private bool isGround = true;     // �ٴڿ� ��� �ִ��� ����
    private bool isAirGround = false; // ���߿��� ���� ���� �ִ��� ����
    private bool isStun = false;      // ���� ���� ����
    private bool isDead = false;      // ��� ���� ����
    private bool isSkill = false;     // ��ų ��� �� ����
    private bool isSlope = false;     // ���� ���� �ִ��� ����
    private bool isJump = false;      // ���� �� ����
    public bool canUseLadder = false; // ��ٸ��� ����� �� �ִ��� ����
    private bool isLadder = false;    // ��ٸ��� Ÿ�� �ִ��� ����

    public float slopeDistance; // ���� üũ �Ÿ�
    public float slopeAngle;    // ��簢
    Vector2 slopePerp;          // ���鿡 ������ ����

    public bool IsDead { get => isDead; set => isDead = value; }

    private void Awake()
    {
        // ������Ʈ �ʱ�ȭ
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
        skills = GetComponent<PlayerSkills>();
        myCollider = GetComponent<Collider2D>();

        // ���̾� �ʱ�ȭ
        groundChkLayer = LayerMask.GetMask("Floor");
        airGroundChkLayer = LayerMask.GetMask("AirFloor");
        airFloorLayer = LayerMask.NameToLayer("AirFloor");
        playerLayer = LayerMask.NameToLayer("Player");
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        xFlip = 1;

        // �ʱ�ȭ ���� ����
        isAttacking = false;
        isGround = true;
        isStun = false;
        IsDead = false;
        jumpCount = 1;

        // �̺�Ʈ ������ ����
        playerStatus.OnStatsCalculated += UpdatePlayerStats;
        playerStatus.OnPlayerDead += PlayerDead;
        playerStatus.OnPlayerHit += PlayerStun;
    }

    private void OnEnable()
    {
        // �ʱ�ȭ ���� ����
        isAttacking = false;
        isGround = true;
        isStun = false;
        IsDead = false;
        isSkill = false;
        isSlope = false;
        isJump = false;
        canUseLadder = false;
        isLadder = false;
        jumpCount = 1;
    }

    // UpdatePlayerStats �Լ�: �÷��̾� ������ ����� �� ȣ��Ǵ� �Լ�
    private void UpdatePlayerStats()
    {
        // �÷��̾� �ִϸ��̼� ��Ʈ�ѷ� ����
        animator.runtimeAnimatorController = animCon[GameManager.Instance.selectPlayerData.playerID];
        // ���� �ִϸ��̼� ��������
        attackAnimationClipName = GameManager.Instance.selectPlayerData.attackAnimationName;

        // ���� �ӵ� ����
        attackSpeed = playerStatus.AttackSpeed;
        // ���ݼӵ��� ���� ���ݾִϸ��̼��� �Ķ���͸� �����ؼ� �ִϸ��̼ǰ� ����ȭ
        animator.SetFloat("AttackSpeed", attackSpeed);

        // ���� �ִϸ��̼��� ������
        animationClips = new List<AnimationClip>(animator.runtimeAnimatorController.animationClips);
        attackAnimationClip = animationClips.Find(clip => clip.name == attackAnimationClipName);

        // ���� �ִϸ��̼��� �����µ� �ɸ��� �ð� ���
        if (attackAnimationClip != null)
        {
            attackDelay = attackAnimationClip.length / attackSpeed;
        }
        else
            Debug.Log("���� �ִϸ��̼��� ��");
        WFSattackDelay = new WaitForSeconds(attackDelay);
    }

    void Update()
    {
        if (IsDead) return;
        GetKey();
        moveDir = new Vector3(horizontal, 0, 0); // ��������Ʈ�� Xflip���� ���� ���Ⱚ

        // ���� üũ
        RaycastHit2D hit = Physics2D.Raycast(chkPos.position, Vector2.down, slopeDistance, groundChkLayer);
        RaycastHit2D fronthit = Physics2D.Raycast(frontChk.position, Vector2.down, 0.4f, groundChkLayer);

        if (hit || fronthit)
        {
            if (fronthit)
                IsCheckSlope(fronthit);
            else if (hit)
                IsCheckSlope(hit);
        }
    }

    // GetKey �Լ�: �Է��� ó���ϴ� �Լ�
    void GetKey()
    {
        if (!isAttacking && Input.GetKey(KeySetting.keys[KeyAction.MeleeAttack]))
        {
            if (playerStatus.UseStamina(attackStamina))
                StartCoroutine(CoAttack());
        }
        if (!isSkill)
        {
            GetSkillKeyDown();
        }
        horizontal = InputSystem.Instance.GetHorizontalInput();
        vertical = InputSystem.Instance.GetVerticalInput();
        if (Input.GetKey(KeySetting.keys[KeyAction.UseHpPotion]))
            UseHpPotion();
    }

    // GetSkillKeyDown �Լ�: ��ų ��� �Է��� ó���ϴ� �Լ�
    void GetSkillKeyDown()
    {
        KeyAction key = InputSystem.Instance.GetSkillKeyDown();
        if (key == KeyAction.KeyCount) return;
        if (skills.GetData(key - KeyAction.Skill_1) == null) return;     // �Էµ� ���Կ� ��ų�� ��ϵǾ��ִ��� ���� üũ
        if (skills.CanUseSkill(key - KeyAction.Skill_1))                 // ��Ÿ�������� ���� üũ
            if (playerStatus.UseStamina(skills.GetData(key - KeyAction.Skill_1).staminaUsage)) // ���¹̳��� ������� ����
            {
                if (skills.UseSkill(key - KeyAction.Skill_1))   // ���������� ��ų�� ���
                {
                    StartCoroutine(CoSkill(skills.GetData(key - KeyAction.Skill_1).skillName));
                }
            }
    }

    private void FixedUpdate()
    {
        isGround = IsCheckGrounded(); // �ٴ� üũ

        if ((isGround && rigid.velocity.y <= 0.1f))
        {
            myCollider.enabled = true;
            Physics2D.IgnoreLayerCollision(playerLayer, airFloorLayer, false);
            rigid.gravityScale = 0f;
            jumpCount = 1;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerLayer, airFloorLayer, true);
            rigid.gravityScale = 2f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            return;
        }

        if (isAttacking && (isGround || isSlope))
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            return;
        }

        if (canUseLadder && !isLadder)
        {
            if (vertical != 0)
            {
                isLadder = true;
                animator.SetBool("isLadder", true);
                jumpCount = 1;
                rigid.position = new Vector2(ladderCollider.transform.position.x, rigid.position.y);
                rigid.velocity = Vector2.zero;
            }
        }

        if (isLadder)
        {
            rigid.gravityScale = 0f;
            MoveLadder();
        }
        else
        {
            rigid.gravityScale = 2f;
            Move();
            animator.SetBool("isLadder", false);
        }

        Jump(); // ���� ó��
    }

    private void LateUpdate()
    {
        Animation(); // �ִϸ��̼� ó��
    }

    // Move �Լ�: �÷��̾��� �̵� ó���� �ϴ� �Լ�
    void Move()
    {
        rigid.constraints = horizontal == 0 ? RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation
            : RigidbodyConstraints2D.FreezeRotation;
        if (IsDead) return;
        if (isSlope && isGround && !isJump)
        {
            rigid.velocity = slopePerp * moveSpeed * horizontal * -1;
        }
        else if (!isSlope && isGround && !isJump)
        {
            rigid.velocity = new Vector2(horizontal * moveSpeed, 0);
        }
        else
        {
            rigid.velocity = new Vector2(horizontal * moveSpeed, rigid.velocity.y);
        }
    }

    // MoveLadder �Լ�: ��ٸ��� Ż �� �̵� ó���� �ϴ� �Լ�
    void MoveLadder()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, vertical * moveSpeed);
    }

    // UseHpPotion �Լ�: ü�� ���� ����� ó���ϴ� �Լ�
    void UseHpPotion()
    {
        Inventory inventory = Inventory.Instance;
        if (inventory.GetHpPotion() != null)
            if (GameManager.Instance.GetHpPotionCooltime() <= 0.01f)
            {
                inventory.GetHpPotion().Use();
                inventory.UseHpPotion();
            }
    }

    // Jump �Լ�: ���� ó���� �ϴ� �Լ�
    void Jump()
    {
        if (rigid.velocity.y <= 0f) isJump = false;
        if (jumpCount > 0 && !isAttacking && !isStun && !isJump && Input.GetKeyDown(KeySetting.keys[KeyAction.Jump]))
        {
            if (isLadder)
            {
                if (horizontal == 0) return;
                else
                {
                    if (playerStatus.UseStamina(jumpStamina))
                    {
                        isLadder = false;
                        rigid.AddForce(new Vector2(horizontal, 0.5f) * jumpForce, ForceMode2D.Impulse);
                        animator.SetBool("isGround", false);
                        isJump = true;
                        jumpCount--;
                    }
                }
            }
            else
            {
                if (isAirGround && vertical == -1 && playerStatus.UseStamina(jumpStamina))
                {
                    StartCoroutine(CoDownJump());
                    animator.SetBool("isGround", false);
                    jumpCount--;
                }
                else if (playerStatus.UseStamina(jumpStamina))
                {
                    rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    animator.SetBool("isGround", false);
                    isJump = true;
                    jumpCount--;
                }
            }
        }
    }

    // CoDownJump �Լ�: �Ʒ��������� ������ �� �� ����ϴ� �Լ�
    IEnumerator CoDownJump()
    {
        myCollider.enabled = false;
        isJump = true;
        yield return new WaitForSeconds(0.2f);
        myCollider.enabled = true;
    }

    // IsCheckGrounded �Լ�: �ٴڿ� ��� �ִ��� ���θ� üũ�ϴ� �Լ�
    private bool IsCheckGrounded()
    {
        bool isGround = false;
        if (Physics2D.BoxCast(chkPos.position, new Vector2(myCollider.bounds.size.x, 0.1f), 0f, Vector2.down, 0.01f, groundChkLayer))
        {
            myCollider.enabled = true;
            isGround = true;
            isAirGround = false;
        }
        if (Physics2D.BoxCast(chkPos.position, new Vector2(myCollider.bounds.size.x, 0.1f), 0f, Vector2.down, 0.01f, airGroundChkLayer))
        {
            isGround = true;
            isAirGround = true;
        }
        if (isLadder) isGround = false;
        return isGround;
    }

    // IsCheckSlope �Լ�: ���� üũ�� �ϴ� �Լ�
    private void IsCheckSlope(RaycastHit2D hit)
    {
        if (hit)
        {
            slopePerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0)
                isSlope = true;
            else
                isSlope = false;
        }
    }

    // Animation �Լ�: �ִϸ��̼� ó���� �ϴ� �Լ�
    void Animation()
    {
        if (!isAttacking)
            xFlip = moveDir.x > 0 ? 1 : -1;
        xFlipScale.x = xFlip;
        if (moveDir.x != 0 && !isAttacking)
            transform.localScale = xFlipScale;
        if (isGround && rigid.velocity.y <= 0.1f)
        {
            animator.SetBool("isGround", true);
            animator.SetFloat("isMove", moveDir.magnitude);
        }
        else
            animator.SetBool("isGround", false);
        if (isLadder)
        {
            if (rigid.velocity.y == 0)
            {
                animator.SetFloat("isLadderFloat", 0f);
            }
            else
            {
                animator.SetFloat("isLadderFloat", 1f);
            }
        }
    }

    IEnumerator CoAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return WFSattackDelay; // * WFSattackDelay == ���� �ִϸ��̼��� ������ �ð������� ������
        isAttacking = false;
    }

    IEnumerator CoSkill(SkillName name)
    {
        isAttacking = true;
        isSkill = true;
        SkillData skillData = DataManager.Instance.GetSkillData(name);
        animator.SetFloat("SkillSpeed", skillData.skillSpeed);
        animator.SetTrigger("Skill");

        switch (name)
        {
            case SkillName.Dash:
                {
                    Vector2 nextPosition;
                    var ray = Physics2D.Raycast(rigid.position, Vector2.right * transform.localScale.x, skillData.range, groundChkLayer);
                    if (!ray) nextPosition = rigid.position + new Vector2(skillData.range * transform.localScale.x, 0f);
                    else nextPosition = new Vector2(ray.point.x - (0.2f * transform.localScale.x), rigid.position.y);

                    transform.position = nextPosition;
                    yield return new WaitForSeconds(skillData.animationLength * 0.66f); // �ִϸ��̼��� �ӵ��� 1.5�� �����صּ� 0.66f�� ������
                    break;
                }
            case SkillName.Judgement:
                {
                    foreach (var target in GetJudgetTarget())
                    {
                        target.GetComponent<Enemy>().JudgementHit();
                    }
                    yield return new WaitForSeconds(skillData.animationLength * 0.66f); // �ִϸ��̼��� �ӵ��� 1.5�� �����صּ� 0.66f�� ������
                    break;
                }
            default:
                {
                    yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length * 0.25f);
                    break;
                }
        }

        isAttacking = false;
        isSkill = false;
    }

    #region �÷��̾��� AttackRange �ݶ��̴��� �浹 ���η� Ÿ���� �߰��ϰ� ������
    public void AddTarget(Rigidbody2D target)
    {
        targets.Add(target);
    }

    public void RemoveTarget(Rigidbody2D target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    public void AddJudgeTarget(Rigidbody2D target)
    {
        judgementtargets.Add(target);
    }

    public void RemoveJudgeTarget(Rigidbody2D target)
    {
        if (judgementtargets.Contains(target))
            judgementtargets.Remove(target);
    }
    #endregion
    // GetJudgetTarget �Լ�: ������Ʈ ��ų�� ������ ��ȯ�ϴ� �Լ�
    public HashSet<Rigidbody2D> GetJudgetTarget()
    {
        return judgementtargets;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") && !IsDead)
        {
            canUseLadder = true;
            ladderCollider = collision.GetComponent<Collider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") && !IsDead)
        {
            canUseLadder = false;
            isLadder = false;
            ladderCollider = null;
        }
    }
    public bool Hit(EnemyStatus enemyStatus)
    {
        if (IsDead) return false;
        return playerStatus.CalculatedHit(enemyStatus);
    }

    // PlayerStun �Լ�: �÷��̾ ���� ���°� �Ǿ��� �� ȣ��Ǵ� �Լ�
    public void PlayerStun()
    {
        StartCoroutine(CoStun());
    }

    // CoStun �Լ�: ���� �ڷ�ƾ
    IEnumerator CoStun()
    {
        isStun = true;
        animator.SetTrigger("isHit");
        float WFS = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return WFS;
        isStun = false;
    }

    // PlayerDead �Լ�: �÷��̾ ������� �� ȣ��Ǵ� �Լ�
    void PlayerDead()
    {
        rigid.velocity = Vector2.zero;
        IsDead = true;
        StopAllCoroutines();
        StartCoroutine(PlayerDeadAnimPlay());
    }

    // PlayerDeadAnimPlay �Լ�: �÷��̾��� ��� �ִϸ��̼��� ����ϴ� �ڷ�ƾ
    IEnumerator PlayerDeadAnimPlay()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3f);
    }

    // BeginAttack �Լ�: ���� ������ �˸��� �Լ�
    public void BeginAttack()
    {
        if (targets.Count > 0)
        {
            foreach (var target in targets)
            {
                target.GetComponent<Enemy>().MeeleAttackHit(playerStatus);
            }
        }
    }
}

