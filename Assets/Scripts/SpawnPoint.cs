using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Spawner가 Enemy를 스폰할 때 사용되는 클래스입니다.
    Spawner spawner;

    public int enemyID;
    private void Start()
    {
        spawner = GetComponentInParent<Spawner>();
    }
    public void EnemyDead()
    {
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10);
        spawner.SpawnEnemy(this,enemyID);
    }
}
