using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Enemy,
    EnemyHpUI,
    FieldItem,
    FieldEquip,
    DamageEffect,
}

public class PoolManager : Singleton<PoolManager>
{
    public GameObject enemyPrefab;
    public GameObject enemyHpUIPrefab;
    public GameObject fieldItemPrefab;
    public GameObject fieldEquipPrefab;
    public GameObject damageEffectPrefab;
    int poolSize = 2;

    List<GameObject> enemyPool;
    List<GameObject> enemyHpUIPool;
    List<GameObject> fieldItemPool;
    List<GameObject> fieldEquipPool;
    List<GameObject> damageEffectPool;

    private void Awake()
    {
        enemyPool = new List<GameObject>();
        enemyHpUIPool = new List<GameObject>();
        fieldItemPool = new List<GameObject>();
        fieldEquipPool = new List<GameObject>();
        damageEffectPool = new List<GameObject>();
        for(int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(enemyPrefab,transform);
            newObj.SetActive(false);
            enemyPool.Add(newObj);
        }
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(enemyHpUIPrefab, transform);
            newObj.SetActive(false);
            enemyHpUIPool.Add(newObj);
        }
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(fieldItemPrefab,transform);
            newObj.SetActive(false);
            fieldItemPool.Add(newObj);
        }
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(fieldEquipPrefab,transform);
            newObj.SetActive(false);
            fieldEquipPool.Add(newObj);
        }
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(damageEffectPrefab, transform);
            newObj.SetActive(false);
            damageEffectPool.Add(newObj);
        }

    }
    public GameObject Get(PoolType type)
    {
        GameObject select = null;

        switch (type)
        {
            case PoolType.Enemy:
                {
                    foreach(GameObject enemy in enemyPool)
                    {
                        if(!enemy.activeSelf)
                        {
                            select = enemy;
                            select.SetActive(true);
                            break;
                        }
                    }
                    if (!select)
                    {
                        select = Instantiate(enemyPrefab, transform);
                        enemyPool.Add(select);
                    }
                }
                break;
            case PoolType.EnemyHpUI:
                {
                    foreach (GameObject enemyUI in enemyHpUIPool)
                    {
                        if (!enemyUI.activeSelf)
                        {
                            select = enemyUI;
                            select.SetActive(true);
                            break;
                        }
                    }
                    if (!select)
                    {
                        select = Instantiate(enemyHpUIPrefab, transform);
                        enemyHpUIPool.Add(select);
                    }
                }
                break;
            case PoolType.FieldEquip:
                {
                    foreach (GameObject equip in fieldEquipPool)
                    {
                        if (!equip.activeSelf)
                        {
                            select = equip;
                            select.SetActive(true);
                            break;
                        }
                    }
                    if (!select)
                    {
                        select = Instantiate(fieldEquipPrefab, transform);
                        fieldEquipPool.Add(select);
                    }
                }
                break;
            case PoolType.FieldItem:
                {
                    foreach (GameObject item in fieldItemPool)
                    {
                        if (!item.activeSelf)
                        {
                            select = item;
                            select.SetActive(true);
                            break;
                        }
                    }
                    if (!select)
                    {
                        select = Instantiate(fieldItemPrefab, transform);
                        fieldItemPool.Add(select);
                    }
                }
                break;
            case PoolType.DamageEffect:
                {
                    foreach (GameObject item in damageEffectPool)
                    {
                        if (!item.activeSelf)
                        {
                            select = item;
                            select.SetActive(true);
                            break;
                        }
                    }
                    if (!select)
                    {
                        select = Instantiate(damageEffectPrefab, transform);
                        damageEffectPool.Add(select);
                    }
                }
                break;
        }
            

        return select;
    }
}
