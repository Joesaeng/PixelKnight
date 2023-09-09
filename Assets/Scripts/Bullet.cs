using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    EnemyStatus enemyStatus;
    Rigidbody2D rigid;
    SpriteRenderer spriteR;
    BulletData bulletData;
    float time;
    float speed;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }
    public void SetBulletData(BulletName bulletName)
    {
        bulletData = DataManager.Instance.GetBulletData(bulletName);
    }
    public void Init(Vector3 dir,EnemyStatus _enemyStatus, BulletName bulletName)
    {
        SetBulletData(bulletName);
        enemyStatus = _enemyStatus;
        time = bulletData.durTime;
        speed = bulletData.speed;
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
            player.Hit(enemyStatus);
            gameObject.SetActive(false);
        }
    }
}
