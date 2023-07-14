using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public RuntimeAnimatorController[] effects;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    private void OnEnable()
    {
        StartCoroutine(PlayEffect());
    }
    IEnumerator PlayEffect()
    {
        animator.enabled = true;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.enabled = false;
        gameObject.SetActive(false);
    }
    public void SetEffect(int index)
    {
        animator.runtimeAnimatorController = effects[index];
    }
}
