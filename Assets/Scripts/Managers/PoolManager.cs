using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject enemyPrefab;
    public GameObject fieldItemPrefab;
    public GameObject fieldEquipPrefab;
    int poolSize = 10;

    List<GameObject> enemyPool;
    List<GameObject> fieldItemPool;
    List<GameObject> fieldEquipPool;

    private void Awake()
    {
        enemyPool = new List<GameObject>();
        fieldItemPool = new List<GameObject>();
        fieldEquipPool = new List<GameObject>();
        for(int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(enemyPrefab);
            newObj.SetActive(false);
            enemyPool.Add(newObj);
        }
        for(int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(fieldItemPrefab);
            newObj.SetActive(false);
            fieldItemPool.Add(newObj);
        }
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(fieldEquipPrefab);
            newObj.SetActive(false);
            fieldEquipPool.Add(newObj);
        }

    }
}
