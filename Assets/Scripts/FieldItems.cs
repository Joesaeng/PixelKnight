using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public SpriteRenderer image;
    public Rigidbody2D rigid;
    public CircleCollider2D coll2d;

    public virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<CircleCollider2D>();
        image = GetComponent<SpriteRenderer>();
    }
    public void SetItem()
    {
        Invoke("SetImage", 0.3f);
        Invoke("OnCollider", 1.5f);
    }
    public void OnEnable()
    {
        rigid.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
        Invoke("DestroyItem", 30f);
    }
    
    public virtual void SetImage() { }
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color originalColor = image.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // 알파값을 1로 설정 (완전 불투명)

        while (elapsedTime < 0.5f)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 0.5f);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor; // 애니메이션이 끝나면 최종 알파값을 1로 설정
    }

    public void OnCollider()
    {
        coll2d.enabled = true;
    }

    public void DestroyItem()
    {
        coll2d.enabled = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}
