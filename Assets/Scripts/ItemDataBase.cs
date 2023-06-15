using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;
    private void Awake()
    {
        instance = this;
    }
    public List<Item> itemDB = new List<Item>();

    public GameObject fieldItemPrefab;
    public Vector2[] pos;

    private void Start()
    {
        for(int i = 5 - 1; i >= 0; --i)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0,3)]);
        }
    }
}
