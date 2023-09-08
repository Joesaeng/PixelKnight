using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    // �ʵ忡 ����Ǵ� �����۵��� �θ�Ŭ�����Դϴ�.
    public SpriteRenderer image;
    public Rigidbody2D rigid;
    public CircleCollider2D coll2d; // �÷��̾�� �浹�Ǵ� �ݶ��̴��Դϴ�.

    public virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<CircleCollider2D>();
        image = GetComponent<SpriteRenderer>();
    }
    public void SetItem()
    {
        // Spawner���� �ʵ忡 �������� ����� �� �����մϴ�.
        // SetImage�� Invoke�� �ҷ����� ������ ������ �̹����� ��巹�������� ���� �ε��ϴµ�.
        // ��巹���� �ε�� �񵿱�� ����Ǳ� ������ ���ÿ� �ε带 �ϸ� 
        // �̹����� null�� ���·� �ε�Ǵ� ������ �ֱ� �����Դϴ�.
        Invoke("SetImage", 0.3f);
        // OnCollider�� ������ڸ��� ȹ���ϴ°� �����մϴ�.
        Invoke("OnCollider", 1.5f);
    }
    public void OnEnable()
    {
        // ������ �����ɶ� ����� �ڷ�ƾ�� ���������� �𸣱� ������ Stop�� �ѹ� ���ݴϴ�.
        StopCoroutine(OverTimeDestroy());
        rigid.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
        StartCoroutine(OverTimeDestroy());
    }
    IEnumerator OverTimeDestroy()
    {
        yield return null;
        float curtime = 0f;
        while(curtime < 15f)
        {
            curtime += Time.deltaTime;
            yield return null;
        }
        DestroyItem();
    }
    
    public virtual void SetImage() { }
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color originalColor = image.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // ���İ��� 1�� ���� (���� ������)

        while (elapsedTime < 0.5f)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 0.5f);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor; // �ִϸ��̼��� ������ ���� ���İ��� 1�� ����
    }

    public void OnCollider()
    {
        coll2d.enabled = true;
    }

    public void DestroyItem()
    {
        StopCoroutine(OverTimeDestroy());
        coll2d.enabled = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}
