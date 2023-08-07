using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    Spawner spawner;
    Enemy enemy;
    private void Start()
    {
        spawner = GetComponentInParent<Spawner>();
    }
    public void SetEnemy(Enemy _enemy)
    {
        enemy = _enemy;
        enemy.GetComponent<EnemyStatus>().OnEnemyDead += EnemyDead;
    }
    void EnemyDead()
    {
        Invoke("Respawn", 5f);
        enemy.GetComponent<EnemyStatus>().OnEnemyDead -= EnemyDead;
    }
    void Respawn()
    {
        spawner.SpawnEnemy(this);
    }
}
