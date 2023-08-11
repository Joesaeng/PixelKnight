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
    Skill,
    Slot
}

public class PoolManager : Singleton<PoolManager>
{
    public GameObject enemyPrefab;
    public GameObject enemyHpUIPrefab;
    public GameObject fieldItemPrefab;
    public GameObject fieldEquipPrefab;
    public GameObject damageEffectPrefab;
    public GameObject skillPrefab;
    public GameObject inventorySlotPrefab;
    int poolSize = 2;

    List<GameObject> enemyPool;
    List<GameObject> enemyHpUIPool;
    List<GameObject> fieldItemPool;
    List<GameObject> fieldEquipPool;
    List<GameObject> damageEffectPool;
    List<GameObject> skillPool;
    List<GameObject> slotPool;

    private void Awake()
    {
        enemyPool = new List<GameObject>();
        enemyHpUIPool = new List<GameObject>();
        fieldItemPool = new List<GameObject>();
        fieldEquipPool = new List<GameObject>();
        damageEffectPool = new List<GameObject>();
        skillPool = new List<GameObject>();
        slotPool = new List<GameObject>();
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
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(skillPrefab, transform);
            newObj.SetActive(false);
            skillPool.Add(newObj);
        }
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(inventorySlotPrefab, transform);
            newObj.SetActive(false);
            slotPool.Add(newObj);
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
                    foreach (GameObject obj in damageEffectPool)
                    {
                        if (!obj.activeSelf)
                        {
                            select = obj;
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
            case PoolType.Slot:
                {
                    foreach (GameObject obj in slotPool)
                    {
                        if (!obj.activeSelf)
                        {
                            select = obj;
                            break;
                        }
                    }
                    if (!select)
                    {
                        select = Instantiate(inventorySlotPrefab, transform);
                        slotPool.Add(select);
                    }
                }
                break;
        }
        select.SetActive(false);

        select.SetActive(true);

        return select;
    }
    public GameObject GetSkill(SkillName name)
    {
        GameObject select = null;
        foreach (GameObject obj in skillPool)
        {
            if (!obj.activeSelf)
            {
                select = obj;
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(skillPrefab, transform);
            skillPool.Add(select);
        }
        select.GetComponent<Skill>().SetData(name);

        select.SetActive(false);
        select.SetActive(true);
        return select;
    }
}
