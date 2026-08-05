using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    // 애니메이션 이벤트 호출용 스크립트

    [SerializeField] protected GameObject arrowPrefab;
    [SerializeField] protected Vector3 arrowOffset;

    protected void Awake()
    {
        arrowPrefab = Resources.Load<GameObject>("Prefabs/arrow");
        arrowOffset = new Vector3(0.5f, 0.3f, 0f);
    }

    protected virtual void SpawnArrow()
    {
        Debug.Log("화살 생성");
        GameObject arrowObj = Instantiate(arrowPrefab);
        SpriteRenderer sr = arrowObj.GetComponent<SpriteRenderer>();
        Arrow arrow = arrowObj.GetComponent<Arrow>();

        // 화살 위치 및 바라보는 방향 조정
        arrowOffset.x = transform.localScale.x == 1 ? -0.5f : 0.5f;
        sr.flipX = arrowOffset.x > 0 ? true : false;
        arrowObj.transform.position = transform.position + arrowOffset;
        Vector2 shootDir = Vector2.right * arrowOffset;
        arrow.SetDirection(shootDir);
        arrow.SetOwner(transform.parent); //화살을 쏜 객체 알려주기
    }


    protected virtual void spawnSword()
    {
        Debug.Log("검 이펙트 스폰");
    }

    protected virtual void RemoveSword()
    {
        Debug.Log("검 이펙트 제거");
    }

}
