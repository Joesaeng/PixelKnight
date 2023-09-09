using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    string nextSceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.GameSaveForSceneChange();
            FakeLoading.instance.StartFakeLoding(2f,nextSceneName,true);
        }
    }
}
