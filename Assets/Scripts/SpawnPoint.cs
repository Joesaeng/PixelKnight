using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    Spawner spawner;
    public int enemyID;
    private void Start()
    {
        spawner = GetComponentInParent<Spawner>();
    }
    public void EnemyDead()
    {
        Debug.Log("EnemyEead_Spawnpoint");
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10);
        spawner.SpawnEnemy(this,enemyID);
    }
}
