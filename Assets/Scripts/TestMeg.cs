using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeg : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            UI_MessageBox.instance.SetText("테스트데스요");
        }
    }
}
