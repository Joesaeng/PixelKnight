using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    // 플레이어의 기본공격 범위 콜라이더 클래스입니다.
    Player player;
    public int index;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            switch (index)
            {
                case 0:
                    player.AddTarget(collision.attachedRigidbody);
                    break;
                case 1:
                    player.AddJudgeTarget(collision.attachedRigidbody);
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (index)
        {
            case 0:
                player.RemoveTarget(collision.attachedRigidbody);
                break;
            case 1:
                player.RemoveJudgeTarget(collision.attachedRigidbody);
                break;
        }
        
    }
}
