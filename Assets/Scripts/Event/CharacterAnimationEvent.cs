using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    // 애니메이션 이벤트 호출용 스크립트

    [SerializeField] protected GameObject arrowPrefab;
    [SerializeField] protected GameObject swordPrefab;
    [SerializeField] protected Vector3 arrowOffset;
    [SerializeField] protected Vector3 swordOffset;

    [SerializeField] protected GameObject swordObj;
    protected void Awake()
    {
        arrowPrefab = Resources.Load<GameObject>("Prefabs/arrow");
        arrowOffset = new Vector3(0.5f, 0.3f, 0f);

        swordPrefab = Resources.Load<GameObject>("Prefabs/attack");
        swordOffset = new Vector3(0.5f, 0.3f, 0f);
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
        if (transform.parent.Find("attack") == null)
        {
            Debug.Log("대상을 찾지 못함");
            swordObj = Instantiate(swordPrefab);
            swordObj.name = "attack";
            swordObj.transform.SetParent(transform.parent);
        }
        swordObj.SetActive(true);
        SpriteRenderer sr = swordObj.GetComponent<SpriteRenderer>();
        SwordAttack sword = swordObj.GetComponent<SwordAttack>();

        swordOffset.x = transform.localScale.x == 1 ? -0.5f : 0.5f;
        swordObj.transform.localScale = swordOffset.x > 0 ? 
            new Vector3(0.1f, swordObj.transform.localScale.y, swordObj.transform.localScale.z) :
            new Vector3(-0.1f, swordObj.transform.localScale.y, swordObj.transform.localScale.z);
        swordObj.transform.position = transform.position + swordOffset;
        //Vector2 shootDir = Vector2.right * swordOffset;
        //sword.SetDirection(shootDir);
        sword.SetOwner(transform.parent); //검을 휘두른 객체 알려주기
    }

    protected virtual void RemoveSword()
    {
        Debug.Log("검 이펙트 제거");
        if(swordObj != null)
            swordObj.SetActive(false);
    }
    protected virtual void OnDeath()
    {

    }
    protected virtual void DropItem()
    {
        
    }


}
