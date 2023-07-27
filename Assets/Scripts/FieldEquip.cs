using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEquip : MonoBehaviour
{
    public Equip equip;
    SpriteRenderer image;
    Rigidbody2D rigid;
    CircleCollider2D coll2d;
    private void Awake()
    {
        equip = new Equip();
        rigid = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<CircleCollider2D>();
        image = GetComponent<SpriteRenderer>();
    }
    public void SetEquip(Equip equip)
    {
        this.equip = equip;
        Invoke("SetImage", 0.3f);
        Invoke("OnCollider", 1.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Inventory.Instance.AddItem(equip))
            {
                DestroyEquip();
            }
        }
    }
    private void OnEnable()
    {
        rigid.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
    }
    void OnCollider()
    {
        coll2d.enabled = true;
    }
    void SetImage()
    {
        image.sprite = equip.itemImage;
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
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
    public Equip GetItem()
    {
        return equip;
    }
    public void DestroyEquip()
    {
        coll2d.enabled = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}
