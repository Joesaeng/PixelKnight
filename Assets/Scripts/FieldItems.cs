using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    // 필드에 드랍되는 아이템들의 부모클래스입니다.
    public SpriteRenderer image;
    public Rigidbody2D rigid;
    public CircleCollider2D coll2d; // 플레이어와 충돌되는 콜라이더입니다.

    public virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<CircleCollider2D>();
        image = GetComponent<SpriteRenderer>();
    }
    public void SetItem()
    {
        // Spawner에서 필드에 아이템이 드랍될 때 세팅합니다.
        // SetImage를 Invoke로 불러오는 이유는 아이템 이미지를 어드레서블에셋을 통해 로드하는데.
        // 어드레서블 로드는 비동기로 진행되기 때문에 동시에 로드를 하면 
        // 이미지가 null인 상태로 로드되는 현상이 있기 때문입니다.
        Invoke("SetImage", 0.3f);
        // OnCollider는 드랍되자마자 획득하는걸 방지합니다.
        Invoke("OnCollider", 1.5f);
    }
    public void OnEnable()
    {
        // 이전에 생성될때 실행된 코루틴이 남아있을지 모르기 때문에 Stop을 한번 해줍니다.
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
        StopCoroutine(OverTimeDestroy());
        coll2d.enabled = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}
