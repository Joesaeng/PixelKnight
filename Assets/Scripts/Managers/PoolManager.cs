using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Enemy,
    EnemyHpUI,
    FieldConsumable,
    FieldEquip,
    DamageEffect,
    Skill,
    Slot,
    Bullet
}

public class PoolManager : Singleton<PoolManager>
{
    public GameObject enemyPrefab;
    public GameObject enemyHpUIPrefab;
    public GameObject fieldEquipPrefab;
    public GameObject fieldConsumablePrefab;
    public GameObject damageEffectPrefab;
    public GameObject skillPrefab;
    public GameObject inventorySlotPrefab;
    public GameObject bulletPrefab;
    int poolSize = 2;

    List<GameObject> enemyPool;
    List<GameObject> enemyHpUIPool;
    List<GameObject> fieldEquipPool;
    List<GameObject> fieldConsumablePool;
    List<GameObject> damageEffectPool;
    List<GameObject> skillPool;
    List<GameObject> slotPool;
    List<GameObject> bulletPool;

    private void Awake()
    {
        enemyPool = new List<GameObject>();
        enemyHpUIPool = new List<GameObject>();
        fieldEquipPool = new List<GameObject>();
        fieldConsumablePool = new List<GameObject>();
        damageEffectPool = new List<GameObject>();
        skillPool = new List<GameObject>();
        slotPool = new List<GameObject>();
        bulletPool = new List<GameObject>();
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
            GameObject newObj = Instantiate(fieldEquipPrefab,transform);
            newObj.SetActive(false);
            fieldEquipPool.Add(newObj);
        }
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(fieldConsumablePrefab, transform);
            newObj.SetActive(false);
            fieldConsumablePool.Add(newObj);
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
        for (int i = 0; i < poolSize; ++i)
        {
            GameObject newObj = Instantiate(bulletPrefab, transform);
            newObj.SetActive(false);
            bulletPool.Add(newObj);
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
            case PoolType.FieldConsumable:
                {
                    foreach (GameObject equip in fieldConsumablePool)
                    {
                        if (!equip.activeSelf)
                        {
                            select = equip;
                            break;
                        }
                    }
                    if (!select)
                    {
                        select = Instantiate(fieldConsumablePrefab, transform);
                        fieldConsumablePool.Add(select);
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
            case PoolType.Skill:
                {
                    Debug.LogError("스킬은 GetSkill()을 쓰셔야 합니다");
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
    public GameObject GetBullet(int EnemyID)
    {
        GameObject select = null;
        foreach (GameObject obj in bulletPool)
        {
            if (!obj.activeSelf)
            {
                select = obj;
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(bulletPrefab, transform);
            bulletPool.Add(select);
        }

        select.SetActive(false);
        select.SetActive(true);
        return select;
    }
}
