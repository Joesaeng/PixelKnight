using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    // 풀매니저에서 Enemy, Skill, FieldItem 등을 스폰하는 클래스입니다.
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
        Invoke("EnemySpawn", 1f);
    }
    void EnemySpawn()
    {
        foreach (SpawnPoint point in enemySpawnPoints)
        {
            SpawnEnemy(point,point.enemyID);
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
            GameObject obj;
            int randomIndex = UnityEngine.Random.Range(0, ItemDataBase.Instance.itemDB.Count);
            if (randomIndex < 5)
            {
                Equip equip = new Equip();
                equip.SetItemData(ItemDataBase.Instance.GetEquipData(randomIndex));
                float ran = UnityEngine.Random.value;
             
                if (ran <= 0.05f)
                    equip.LevelUpItem(ItemLevel.Unique);
                else if (ran <= 0.15f)
                    equip.LevelUpItem(ItemLevel.Rare);
                else if (ran <= 0.4f)
                    equip.LevelUpItem(ItemLevel.Advanced);
                else
                    equip.LevelUpItem(ItemLevel.Common);
                obj = PoolManager.Instance.Get(PoolType.FieldEquip);
                obj.GetComponent<FieldEquip>().SetEquip(equip);
            }
            else
            {
                Consumable consum = ItemDataBase.Instance.GetConsumableData(randomIndex);
                obj = PoolManager.Instance.Get(PoolType.FieldConsumable);
                obj.GetComponent<FieldConsumable>().SetConsumable(consum);
            }
            
            obj.transform.position = pos;
        }
    }
 
    public void SpawnEnemy(SpawnPoint point,int enemyID)
    {
        GameObject enemy = PoolManager.Instance.Get(PoolType.Enemy);
        enemy.GetComponent<EnemyStatus>().SetData(enemyID);
        enemy.transform.position = point.transform.position;
        enemy.transform.SetParent(point.transform);
        OnEnemySpawn?.Invoke(enemy);
    }
}
