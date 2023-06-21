using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 컴포넌트
    Rigidbody2D rigid;
    Animator animator;
    Collider2D col2D;
    SpriteRenderer spriteRenderer;
    public PlayerStatus playerStatus;
    public List<AnimationClip> animationClips;
    public AnimationClip attackAnimationClip;
    public RuntimeAnimatorController[] animCon;

    public string attackAnimationClipName;

    public float attackSpeed = 1.0f;// 공격 속도
    private WaitForSeconds WFSattackDelay;

    private int floorLayer;
    Vector3 moveDir;
    Vector3 velocity;
    public float moveSpeed;
    public float jumpForce;
    private float horizontal;


    public float attackDelay;
    private bool isAttacking = false;
    private bool isGround = true;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
        playerStatus = GetComponent<PlayerStatus>();
        floorLayer = LayerMask.GetMask("Floor");
        
        playerStatus.OnStatsCalculated += UpdatePlayerStats;
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
        GetKey();
        moveDir = new Vector3(horizontal, 0, 0); // 스프라이트의 Xflip값을 위한 방향값
        isGround = IsCheckGrounded();
        //if (isAttacking)
        //    return;
    }

    void GetKey()
    {
        if (!isAttacking && Input.GetButton("meleeAttack"))
        {
            StartCoroutine(CoAttack());
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        if ((horizontal == 0 && isGround)||(isAttacking && isGround))
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
        if (!isAttacking && isGround && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        //float vertical = Input.GetAxis("Vertical");

    }
    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        Animation();
    }

    void Move()
    {
        if(!isAttacking)
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
        if (moveDir.x != 0)
            spriteRenderer.flipX = moveDir.x < 0;
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
    
    
}
