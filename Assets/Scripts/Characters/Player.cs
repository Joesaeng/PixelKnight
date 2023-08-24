using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // 컴포넌트
    Rigidbody2D rigid;
    Animator animator;
    Collider2D myCollider;
    Collider2D ladderCollider;
    public Transform chkPos;
    public Transform frontChk;
    public PlayerStatus playerStatus;
    public PlayerSkills skills;
    public List<AnimationClip> animationClips;
    public AnimationClip attackAnimationClip;
    public RuntimeAnimatorController[] animCon;
    public Transform[] attackRangePos;

    public HashSet<Rigidbody2D> targets = new();
    public HashSet<Rigidbody2D> judgementtargets = new();

    public string attackAnimationClipName;

    public float attackSpeed = 1.0f;// 공격 속도
    private WaitForSeconds WFSattackDelay;

    private int groundChkLayer;
    private int airGroundChkLayer;
    private int airFloorLayer;
    private int playerLayer;
    Vector3 moveDir;
    Vector3 xFlipScale;
    int xFlip;
    public float moveSpeed;
    public float jumpForce;
    public float horizontal;
    public float vertical;
    public int jumpCount;

    public float jumpStamina = 5f;
    public float attackStamina = 8f;

    public float attackDelay;
    private bool isAttacking = false;
    private bool isGround = true;
    private bool isAirGround = false;
    private bool isStun = false;
    private bool isDead = false;
    public bool IsDead
    { get { return isDead; } }
    private bool isSkill = false;
    private bool isSlope = false;
    private bool isJump = false;
    public bool canUseLadder = false;
    private bool isLadder = false;

    public float slopeDistance;
    public float slopeAngle;
    Vector2 slopePerp;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
        skills = GetComponent<PlayerSkills>();
        myCollider = GetComponent<Collider2D>();

        groundChkLayer = LayerMask.GetMask("Floor");
        airGroundChkLayer = LayerMask.GetMask("AirFloor");
        airFloorLayer = LayerMask.NameToLayer("AirFloor");
        playerLayer = LayerMask.NameToLayer("Player");
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        xFlip = 1;

        isAttacking = false;
        isGround = true;
        isStun = false;
        isDead = false;
        jumpCount = 1;

        playerStatus.OnStatsCalculated += UpdatePlayerStats;
        playerStatus.OnPlayerDead += PlayerDead;
        playerStatus.OnPlayerHit += PlayerStun;
    }
    private void OnEnable()
    {
        isAttacking = false;
        isGround = true;
        isStun = false;
        isDead = false;
        isSkill = false;
        isSlope = false;
        isJump = false;
        canUseLadder = false;
        isLadder = false;
        jumpCount = 1;
    }

    private void UpdatePlayerStats()
    {
        animator.runtimeAnimatorController = animCon[GameManager.Instance.selectPlayerData.playerID];
        attackAnimationClipName = GameManager.Instance.selectPlayerData.attackAnimationName;

        attackSpeed = playerStatus.AttackSpeed;
        animator.SetFloat("AttackSpeed", attackSpeed);

        animationClips = new List<AnimationClip>(animator.runtimeAnimatorController.animationClips);
        attackAnimationClip = animationClips.Find(clip => clip.name == attackAnimationClipName);

        if (attackAnimationClip != null)
        {
            attackDelay = attackAnimationClip.length / attackSpeed;
        }
        else
            Debug.Log("어택애니메이션이 널");
        WFSattackDelay = new WaitForSeconds(attackDelay);
    }

    void Update()
    {
        if (isDead) return;
        GetKey();
        moveDir = new Vector3(horizontal, 0, 0); // 스프라이트의 Xflip값을 위한 방향값

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

    }
    void GetSkillKeyDown()
    {
        KeyAction key = InputSystem.Instance.GetSkillKeyDown();
        if (key == KeyAction.KeyCount) return;
        if (skills.GetData(key - KeyAction.Skill_1) == null) return;
        if (skills.CanUseSkill(key - KeyAction.Skill_1))
            if (playerStatus.UseStamina(skills.GetData(key - KeyAction.Skill_1).staminaUsage))
            {
                if (skills.UseSkill(key - KeyAction.Skill_1))
                {
                    StartCoroutine(CoSkill(skills.GetData(key - KeyAction.Skill_1).skillName));
                }
            }

    }
    private void FixedUpdate()
    {
        isGround = IsCheckGrounded();
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
        Jump();
    }
    private void LateUpdate()
    {
        Animation();
    }

    void Move()
    {
        rigid.constraints = horizontal == 0 ? RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation
            : RigidbodyConstraints2D.FreezeRotation;
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
    void MoveLadder()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, vertical * moveSpeed);

    }
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
    IEnumerator CoDownJump()
    {
        myCollider.enabled = false;
        isJump = true;
        yield return new WaitForSeconds(0.2f);
        myCollider.enabled = true;
    }
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
            animator.SetFloat("isMove", moveDir.magnitude); // 이동 애니메이션
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
        yield return WFSattackDelay;

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
            case SkillName.Dash: // Dash
                {
                    Vector2 nextPosition;
                    var ray = Physics2D.Raycast(rigid.position, Vector2.right * transform.localScale.x, skillData.range, groundChkLayer);
                    if (!ray) nextPosition = rigid.position + new Vector2(skillData.range * transform.localScale.x, 0f);
                    else nextPosition = new Vector2(ray.point.x - (0.2f * transform.localScale.x), rigid.position.y);

                    transform.position = nextPosition;
                    yield return new WaitForSeconds(skillData.animationLength * 0.66f);
                    break;
                }
            case SkillName.Judgement: // Judgement
                {
                    foreach (var target in GetJudgetTarget())
                    {
                        target.GetComponent<Enemy>().JudgementHit();
                    }
                    yield return new WaitForSeconds(skillData.animationLength * 0.66f);
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
    public HashSet<Rigidbody2D> GetJudgetTarget()
    {
        return judgementtargets;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") && !isDead)
        {
            canUseLadder = true;
            ladderCollider = collision.GetComponent<Collider2D>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") && !isDead)
        {
            canUseLadder = false;
            isLadder = false;
            ladderCollider = null;
        }
    }
    public void PlayerStun()
    {
        StartCoroutine(CoStun());
    }
    IEnumerator CoStun()
    {
        isStun = true;
        animator.SetTrigger("isHit");
        float WFS = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return WFS;
        isStun = false;
    }
    void PlayerDead()
    {
        isDead = true;
        StopAllCoroutines();
        StartCoroutine(PlayerDeadAnimPlay());
    }
    IEnumerator PlayerDeadAnimPlay()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3f);
    }
    public void BeginAttack()
    {
        if (targets.Count > 0)
        {
            foreach (var target in targets)
            {
                target.GetComponent<Enemy>().Hit(playerStatus);
            }
        }
    }
}
