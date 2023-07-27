using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public List<Transform> enemySpawnPoint;
    public Action OnEnemySpawn;
    private void Awake()
    {
        instance = this;
        Transform[] temp = GetComponentsInChildren<Transform>();
        foreach(Transform t in temp)
        {
            if (t.transform == this.transform) continue;
            enemySpawnPoint.Add(t);
        }
    }

    private void Start()
    {
        EnemySpawn();
    }
    void EnemySpawn()
    {
        for(int i = 0; i < enemySpawnPoint.Count; ++i)
        {
            GameObject enemy = PoolManager.Instance.Get(PoolType.Enemy);
            enemy.transform.position = enemySpawnPoint[i].position;
            enemy.GetComponent<EnemyStatus>().SetData();
        }
        OnEnemySpawn?.Invoke();
        //Invoke("EnemySpawn", 15f);
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
            GameObject obj = PoolManager.Instance.Get(PoolType.FieldEquip);
            obj.GetComponent<FieldEquip>().SetEquip(equip);
            obj.transform.position = pos;
        }
    }
}
