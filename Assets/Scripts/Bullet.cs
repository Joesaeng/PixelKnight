using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    EnemyStatus enemyStatus;
    Rigidbody2D rigid;
    SpriteRenderer spriteR;
    float time = 3f;
    float speed = 4f;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }
    public void Init(Vector3 dir,EnemyStatus _enemyStatus)
    {
        enemyStatus = _enemyStatus;
        time = 3f;
        spriteR.flipX = dir.x < 0 ? true : false;
        rigid.velocity = dir * speed;
    }
    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
            gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHit"))
        {
            Player player = collision.GetComponentInParent<Player>();
            if (player.IsDead == true) return;
            player.playerStatus.CalculatedHit(enemyStatus);
            gameObject.SetActive(false);
        }
    }
}
