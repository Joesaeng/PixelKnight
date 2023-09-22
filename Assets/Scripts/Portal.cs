using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int portalID;
    public string nextSceneName;
    Animator animator;

    private bool isUseable;
    private void Awake()
    {
        isUseable = false;
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isUseable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isUseable = false;
        }
    }
    public Transform GetTransform()
    {
        return transform;
    }
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, GameManager.Instance.player.transform.position);
        if(distance <= 1.5f)
        {
            animator.SetBool("isOn", true);
        }
        else
        {
            animator.SetBool("isOn", false);
        }
        if (isUseable && Input.GetKey(KeySetting.keys[KeyAction.Interaction]))
        {
            PortalManager.Instance.SetCurUsePortalID(portalID);
            GameManager.Instance.GameSaveForSceneChange();
            FakeLoading.instance.StartFakeLoding(0.3f, nextSceneName, true);
        }
    }
}
