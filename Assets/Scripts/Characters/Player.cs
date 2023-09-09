using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // 컴포넌트
    Rigidbody2D rigid;           
    Animator animator;           
    Collider2D myCollider;       // 플레이어의 중력 충돌에 관련된 콜라이더 컴포넌트
    Collider2D ladderCollider;   // 사다리를 사용할 때 사용하는 Collider2D 컴포넌트

    // 키 체크 관련
    public Transform chkPos;    // 플레이어의 발 위치 체크용 포지션
    public Transform frontChk;  // 플레이어의 정면 위치 체크용 포지션
    public PlayerStatus playerStatus;  // 플레이어의 상태 관련 정보를 가진 스크립트
    public PlayerSkills skills;        // 플레이어의 스킬 관련 정보를 가진 스크립트

    // 애니메이션 관련
    public List<AnimationClip> animationClips;
    public AnimationClip attackAnimationClip; 
    public RuntimeAnimatorController[] animCon;    // 다양한 캐릭터를 위한 애니메이션 컨트롤러 목록

    // 공격 관련 변수
    public HashSet<Rigidbody2D> targets = new HashSet<Rigidbody2D>();         // 플레이어의 공격 대상
    public HashSet<Rigidbody2D> judgementtargets = new HashSet<Rigidbody2D>(); // 특정 스킬의 대상
    public string attackAnimationClipName; // 공격 애니메이션 클립 이름


    public float attackSpeed = 1.0f;       // 공격 속도
    private WaitForSeconds WFSattackDelay; // 공격 딜레이용 WaitForSeconds 객체

    // 레이어 관련 변수
    private int groundChkLayer;     // 바닥 레이어 마스크
    private int airGroundChkLayer;  // 공중 발판 레이어 마스크
    private int airFloorLayer;      // 공중 발판 레이어
    private int playerLayer;        // 플레이어 레이어

    // 이동 관련 변수
    Vector3 moveDir;                // 이동 방향 벡터
    Vector3 xFlipScale;             // X축 반전용 스케일 벡터
    int xFlip;                       // X축 반전 여부를 나타내는 변수
    public float moveSpeed;         // 이동 속도
    public float jumpForce;         // 점프 힘
    public float horizontal;        // 수평 입력값
    public float vertical;          // 수직 입력값
    public int jumpCount;           // 점프 가능 횟수

    // 스태미너 관련 변수
    public float jumpStamina = 5f;  // 점프에 필요한 스태미너
    public float attackStamina = 8f; // 공격에 필요한 스태미너

    public float attackDelay;        // 공격 딜레이 시간
    private bool isAttacking = false; // 공격 중 여부
    private bool isGround = true;     // 바닥에 닿아 있는지 여부
    private bool isAirGround = false; // 공중에서 발판 위에 있는지 여부
    private bool isStun = false;      // 스턴 상태 여부
    private bool isDead = false;      // 사망 상태 여부
    private bool isSkill = false;     // 스킬 사용 중 여부
    private bool isSlope = false;     // 경사면 위에 있는지 여부
    private bool isJump = false;      // 점프 중 여부
    public bool canUseLadder = false; // 사다리를 사용할 수 있는지 여부
    private bool isLadder = false;    // 사다리를 타고 있는지 여부

    public float slopeDistance; // 경사면 체크 거리
    public float slopeAngle;    // 경사각
    Vector2 slopePerp;          // 경사면에 수직인 벡터

    public bool IsDead { get => isDead; set => isDead = value; }

    private void Awake()
    {
        // 컴포넌트 초기화
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
        skills = GetComponent<PlayerSkills>();
        myCollider = GetComponent<Collider2D>();

        // 레이어 초기화
        groundChkLayer = LayerMask.GetMask("Floor");
        airGroundChkLayer = LayerMask.GetMask("AirFloor");
        airFloorLayer = LayerMask.NameToLayer("AirFloor");
        playerLayer = LayerMask.NameToLayer("Player");
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        xFlip = 1;

        // 초기화 상태 설정
        isAttacking = false;
        isGround = true;
        isStun = false;
        IsDead = false;
        jumpCount = 1;

        // 이벤트 리스너 설정
        playerStatus.OnStatsCalculated += UpdatePlayerStats;
        playerStatus.OnPlayerDead += PlayerDead;
        playerStatus.OnPlayerHit += PlayerStun;
    }

    private void OnEnable()
    {
        // 초기화 상태 설정
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

    // UpdatePlayerStats 함수: 플레이어 스탯이 변경될 때 호출되는 함수
    private void UpdatePlayerStats()
    {
        // 플레이어 애니메이션 컨트롤러 설정
        animator.runtimeAnimatorController = animCon[GameManager.Instance.selectPlayerData.playerID];
        // 공격 애니메이션 가져오기
        attackAnimationClipName = GameManager.Instance.selectPlayerData.attackAnimationName;

        // 공격 속도 설정
        attackSpeed = playerStatus.AttackSpeed;
        // 공격속도에 따라 공격애니메이션의 파라미터를 설정해서 애니메이션과 동기화
        animator.SetFloat("AttackSpeed", attackSpeed);

        // 공격 애니메이션을 가져옴
        animationClips = new List<AnimationClip>(animator.runtimeAnimatorController.animationClips);
        attackAnimationClip = animationClips.Find(clip => clip.name == attackAnimationClipName);

        // 공격 애니메이션이 끝나는데 걸리는 시간 계산
        if (attackAnimationClip != null)
        {
            attackDelay = attackAnimationClip.length / attackSpeed;
        }
        else
            Debug.Log("어택 애니메이션이 널");
        WFSattackDelay = new WaitForSeconds(attackDelay);
    }

    void Update()
    {
        if (IsDead) return;
        GetKey();
        moveDir = new Vector3(horizontal, 0, 0); // 스프라이트의 Xflip값을 위한 방향값

        // 경사면 체크
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

    // GetKey 함수: 입력을 처리하는 함수
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

    // GetSkillKeyDown 함수: 스킬 사용 입력을 처리하는 함수
    void GetSkillKeyDown()
    {
        KeyAction key = InputSystem.Instance.GetSkillKeyDown();
        if (key == KeyAction.KeyCount) return;
        if (skills.GetData(key - KeyAction.Skill_1) == null) return;     // 입력된 슬롯에 스킬이 등록되어있는지 여부 체크
        if (skills.CanUseSkill(key - KeyAction.Skill_1))                 // 쿨타임중인지 여부 체크
            if (playerStatus.UseStamina(skills.GetData(key - KeyAction.Skill_1).staminaUsage)) // 스태미나가 충분한지 여부
            {
                if (skills.UseSkill(key - KeyAction.Skill_1))   // 최종적으로 스킬을 사용
                {
                    StartCoroutine(CoSkill(skills.GetData(key - KeyAction.Skill_1).skillName));
                }
            }
    }

    private void FixedUpdate()
    {
        isGround = IsCheckGrounded(); // 바닥 체크

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

        Jump(); // 점프 처리
    }

    private void LateUpdate()
    {
        Animation(); // 애니메이션 처리
    }

    // Move 함수: 플레이어의 이동 처리를 하는 함수
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

    // MoveLadder 함수: 사다리를 탈 때 이동 처리를 하는 함수
    void MoveLadder()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, vertical * moveSpeed);
    }

    // UseHpPotion 함수: 체력 포션 사용을 처리하는 함수
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

    // Jump 함수: 점프 처리를 하는 함수
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

    // CoDownJump 함수: 아래방향으로 점프를 할 때 사용하는 함수
    IEnumerator CoDownJump()
    {
        myCollider.enabled = false;
        isJump = true;
        yield return new WaitForSeconds(0.2f);
        myCollider.enabled = true;
    }

    // IsCheckGrounded 함수: 바닥에 닿아 있는지 여부를 체크하는 함수
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

    // IsCheckSlope 함수: 경사면 체크를 하는 함수
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

    // Animation 함수: 애니메이션 처리를 하는 함수
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
        yield return WFSattackDelay; // * WFSattackDelay == 공격 애니메이션이 끝나는 시간까지의 딜레이
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
                    yield return new WaitForSeconds(skillData.animationLength * 0.66f); // 애니메이션의 속도를 1.5로 설정해둬서 0.66f를 곱해줌
                    break;
                }
            case SkillName.Judgement:
                {
                    foreach (var target in GetJudgetTarget())
                    {
                        target.GetComponent<Enemy>().JudgementHit();
                    }
                    yield return new WaitForSeconds(skillData.animationLength * 0.66f); // 애니메이션의 속도를 1.5로 설정해둬서 0.66f를 곱해줌
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

    #region 플레이어의 AttackRange 콜라이더의 충돌 여부로 타겟을 추가하고 제거함
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
    // GetJudgetTarget 함수: 저지먼트 스킬의 대상들을 반환하는 함수
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

    // PlayerStun 함수: 플레이어가 스턴 상태가 되었을 때 호출되는 함수
    public void PlayerStun()
    {
        StartCoroutine(CoStun());
    }

    // CoStun 함수: 스턴 코루틴
    IEnumerator CoStun()
    {
        isStun = true;
        animator.SetTrigger("isHit");
        float WFS = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return WFS;
        isStun = false;
    }

    // PlayerDead 함수: 플레이어가 사망했을 때 호출되는 함수
    void PlayerDead()
    {
        rigid.velocity = Vector2.zero;
        IsDead = true;
        StopAllCoroutines();
        StartCoroutine(PlayerDeadAnimPlay());
    }

    // PlayerDeadAnimPlay 함수: 플레이어의 사망 애니메이션을 재생하는 코루틴
    IEnumerator PlayerDeadAnimPlay()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3f);
    }

    // BeginAttack 함수: 공격 시작을 알리는 함수
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

