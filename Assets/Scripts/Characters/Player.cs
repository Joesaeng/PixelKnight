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
    private float horizontal;


    public float attackDelay;
    private bool isAttacking = false;
    private bool isGround = true;
    private bool isStun = false;
    private bool isDead = false;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
        playerStatus = GetComponent<PlayerStatus>();

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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            return;
        }
        if (!isAttacking && Input.GetButton("meleeAttack"))
        {
            StartCoroutine(CoAttack());
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        if ((horizontal == 0 && isGround)||(isAttacking && isGround && !isStun))
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
        if (!isAttacking && !isStun && isGround && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        //float vertical = Input.GetAxis("Vertical");

    }
    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
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
            rigid.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);
            if (rigid.velocity.x > moveSpeed)
                rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
            if (rigid.velocity.x < moveSpeed * (-1))
                rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyAttackRange") && !isDead)
        {
            if(playerStatus.CalculatedHit(collision.GetComponentInParent<EnemyStatus>()))
            {
                PlayerStun();
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
