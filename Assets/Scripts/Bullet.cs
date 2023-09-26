using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    EnemyStatus enemyStatus;
    Rigidbody2D rigid;
    SpriteRenderer spriteR;
    BulletData bulletData;
    float speed;
    WaitForSeconds durTime;
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
        durTime = new WaitForSeconds(bulletData.durTime);
        speed = bulletData.speed;
        spriteR.flipX = dir.x < 0 ? true : false;
        rigid.velocity = dir * speed;
        StartCoroutine(CoBulletReturn());
    }

    IEnumerator CoBulletReturn()
    {
        yield return durTime;
        gameObject.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHit"))
        {
            Player player = collision.GetComponentInParent<Player>();
            player.Hit(enemyStatus);
            StopCoroutine(CoBulletReturn());
            gameObject.SetActive(false);
        }
    }
}
