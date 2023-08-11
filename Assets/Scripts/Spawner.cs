using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public SpawnPoint[] enemySpawnPoints;
    public Action<GameObject> OnEnemySpawn;
    private void Awake()
    {
        instance = this;
        enemySpawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    private void Start()
    {
        EnemySpawn();
    }
    void EnemySpawn()
    {
        foreach (SpawnPoint point in enemySpawnPoints)
        {
            SpawnEnemy(point);
        }

    }

    public GameObject ShowDamageEffect(int index)
    {
        GameObject effect = PoolManager.Instance.Get(PoolType.DamageEffect);
        effect.GetComponent<DamageEffect>().SetEffect(index);

        return effect;
    }
    public void ItemSpawn(Vector2 pos)
    {
        //if(UnityEngine.Random.value <= 0.5f)
        {
            Equip equip = new Equip();
            int randomIndex = UnityEngine.Random.Range(0, ItemDataBase.Instance.itemDB.Count);
            equip.SetItemData(ItemDataBase.Instance.GetEquipData(randomIndex));
            if (UnityEngine.Random.value <= 0.05f)
                equip.LevelUpItem(ItemLevel.Unique);
            else if (UnityEngine.Random.value <= 0.15f)
                equip.LevelUpItem(ItemLevel.Rare);
            else if (UnityEngine.Random.value <= 0.4f)
                equip.LevelUpItem(ItemLevel.Advanced);
            else
                equip.LevelUpItem(ItemLevel.Common);
            GameObject obj = PoolManager.Instance.Get(PoolType.FieldEquip);
            obj.GetComponent<FieldEquip>().SetEquip(equip);
            obj.transform.position = pos;
        }
    }
 
    public void SpawnEnemy(SpawnPoint point)
    {
        GameObject enemy = PoolManager.Instance.Get(PoolType.Enemy);
        enemy.transform.position = point.transform.position;
        enemy.transform.SetParent(point.transform);
        enemy.GetComponent<EnemyStatus>().SetData();
        point.SetEnemy(enemy.GetComponent<Enemy>());
        OnEnemySpawn?.Invoke(enemy);
    }
}
