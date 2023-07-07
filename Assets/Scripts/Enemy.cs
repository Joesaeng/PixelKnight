using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    EnemyStatus enemyStatus;
    public bool isHit = false;
    public int nextMove;
    float moveSpeed;
    Vector3 xFlipScale;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<EnemyStatus>();
        xFlipScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        moveSpeed = enemyStatus.moveSpeed;

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove*moveSpeed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1,LayerMask.GetMask("Floor"));
        if(rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 5);
        }
    }
    private void LateUpdate()
    {
        if (nextMove != 0)
        {
            xFlipScale.x = nextMove;
            transform.localScale = xFlipScale;
        }
    }
    void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", Random.Range(2f, 6f));

        anim.SetInteger("WalkSpeed", nextMove);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackRange") && !isHit)
        {
            Debug.Log("Enemy Hit!");
            isHit = true;
            StartCoroutine(ResetHitState(GameManager.Instance.player.attackDelay * 0.5f));
        }
    }
    IEnumerator ResetHitState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isHit = false;
    }
    /*
    
    Enemy ��ũ��Ʈ -����- ����---
    1. �⺻�����δ� �񼱰�����.
    2. �÷��̾�� ������ ������ ������ �� �÷��̾ Ÿ������ �����Ͽ� ���󰡰�
       �����ϴ� ���.
    3. �������͸� �Ҷ��� ��ĵ�� �־ �� �ȿ� ������ Ÿ������ �����ϰ� ���󰣴�.
    4. AttackRange �ȿ� ������ ������ �Ѵ�.
    5. ������ ���� �ִϸ��̼ǿ� ���缭 �ݶ��̴��� OnEnable ��Ű�� �� �����ӿ�
       Player�� �浹�� �Ͼ�� Player�� �ݶ��̴����� Player�� �����ͼ�
       Player�� TakeDamage �޼��带 �����Ų��.
     */
}
