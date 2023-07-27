using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // 컴포넌트
    Rigidbody2D rigid;
    Animator animator;
    Collider2D col2D;
    public PlayerStatus playerStatus;
    public PlayerSkills skills;
    public List<AnimationClip> animationClips;
    public AnimationClip attackAnimationClip;
    public RuntimeAnimatorController[] animCon;
    Collider2D attackRange;
    
    public string attackAnimationClipName;

    public float attackSpeed = 1.0f;// 공격 속도
    private WaitForSeconds WFSattackDelay;

    private int floorLayer;
    Vector3 moveDir;
    Vector3 xFlipScale;
    int xFlip;
    public float moveSpeed;
    public float jumpForce;
    public float horizontal;
    public float vertical;

    public float jumpStamina = 5f;
    public float attackStamina = 8f;

    public float attackDelay;
    private bool isAttacking = false;
    private bool isGround = true;
    private bool isStun = false;
    private bool isDead = false;
    private bool isDash = false;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
        playerStatus = GetComponent<PlayerStatus>();
        skills = GetComponent<PlayerSkills>();

        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach(Collider2D col in cols)
        {
            if (col.transform == transform)
                continue;
            attackRange = col;
        }

        floorLayer = LayerMask.GetMask("Floor");
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        xFlip = 1;

        isAttacking = false;
        isGround = true;
        isStun = false;
        isDead = false;

        playerStatus.OnStatsCalculated += UpdatePlayerStats;
        playerStatus.OnPlayerDead += PlayerDead;
        playerStatus.OnPlayerHit += PlayerStun;
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
        isGround = IsCheckGrounded();
        //if (isAttacking)
        //    return;
    }

    void GetKey()
    {
        if (!isAttacking && Input.GetKey(KeySetting.keys[KeyAction.MeleeAttack]))
        {
            if(playerStatus.UseStamina(attackStamina))
                StartCoroutine(CoAttack());
        }
        if (!isAttacking)
        {
            GetSkillKeyDown();
        }
        horizontal = InputSystem.Instance.GetHorizontalInput();
        vertical = InputSystem.Instance.GetVerticalInput();
        //if ((horizontal == 0 && isGround)||(isAttacking && isGround && !isStun))
        //    rigid.velocity = new Vector2(0f, rigid.velocity.y);
        if (!isAttacking && !isStun && isGround && Input.GetKeyDown(KeySetting.keys[KeyAction.Jump]))
        {
            if(playerStatus.UseStamina(jumpStamina))
                Jump();
        }
    }
    void GetSkillKeyDown()
    {
        KeyAction key = InputSystem.Instance.GetSkillKeyDown();
        if (key == KeyAction.KeyCount) return;
        if (playerStatus.UseStamina(skills.GetData(key - KeyAction.Skill_1).staminaUsage))
        {
            skills.UseSkill(key - KeyAction.Skill_1);
            StartCoroutine(CoSkill(key - KeyAction.Skill_1));
            
        }

  
        
    }
    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && isGround)
        {
            if(isGround)
                rigid.velocity = new Vector2(0f, rigid.velocity.y);
            return;
        }
        Move();
    }
    private void LateUpdate()
    {
        Animation();
    }

    void Move()
    {
        if(!isAttacking && !isStun)
        {
            //rigid.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);
            //if (rigid.velocity.x > moveSpeed)
            //    rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
            //if (rigid.velocity.x < moveSpeed * (-1))
            //    rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
            rigid.velocity = new Vector2(horizontal * moveSpeed, rigid.velocity.y);
        }
    }
    void Jump()
    {
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetBool("isGround", false);
    }

    private bool IsCheckGrounded()
    {
        return Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.1f, floorLayer);
    }
    void Animation()
    {
        xFlip = moveDir.x > 0 ? 1 : -1;
        xFlipScale.x = xFlip;
        if (moveDir.x != 0)
            transform.localScale = xFlipScale;
        if (isGround)
        {
            animator.SetBool("isGround", true);
            animator.SetFloat("isMove", moveDir.magnitude); // 이동 애니메이션
        }
        else
            animator.SetBool("isGround", false);
    }
    IEnumerator CoAttack()
    {
        isAttacking = true;
        
        animator.SetTrigger("Attack");

        yield return WFSattackDelay;

        isAttacking = false;
    }

    IEnumerator CoSkill(int index)
    {
        isAttacking = true;
        isDash = true;
        animator.SetTrigger("Skill");
        Vector2 nextPosition = rigid.position + new Vector2(skills.GetData(index).range * transform.localScale.x,0f);
        transform.position = nextPosition;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length * 0.25f);

        isAttacking = false;
        isDash = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PlayerTriggerEnter");
        if (collision.CompareTag("EnemyAttackRange") && !isDead)
        {
            if(playerStatus.CalculatedHit(collision.GetComponentInParent<EnemyStatus>()))
            {
                
            }
            else
            {
                
            }

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
        attackRange.enabled = true;
    }
    public void EndAttack()
    {
        attackRange.enabled = false;
    }
}
