using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SwordAttack : MonoBehaviour
{
    private SpriteRenderer sp;
    [SerializeField] private Transform player;
    private float swordDamage;

    // 공격을 생성한 오브젝트 데이터
    [SerializeField] private Entity entity;
    private Transform owner;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sp = GetComponent<SpriteRenderer>();
        Init();

    }

    private void Init()
    {
        swordDamage = entity.Atk;
    }

    private void Update()
    {
        /*
        if (player.position.x > transform.position.x)
        {
            sp.flipX = false;
        }
        if (player.position.x < transform.position.x)
        {
            sp.flipX = true;
        }
        */
    }

    /* Arrow스크립트와 맞추려고 있는건데 일단은 필요없을듯
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
    */


    public void SetOwner(Transform ownerTransform)
    {
        owner = ownerTransform;
        entity = ownerTransform.GetComponent<Entity>();
    }

    public void SetDamage(float attack)
    {
        swordDamage = attack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //같은 팀은 때리지 않는다.
        if (collision.CompareTag(owner.tag) || collision.CompareTag("Ground"))
            return;

        Entity target = collision.GetComponent<Entity>();

        target.OnHit?.Invoke(Mathf.Max(0, swordDamage - target.Def));
        Debug.Log("검 피격!");
        Destroy(gameObject);
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerClass playerClass = other.GetComponent<PlayerClass>();

        if (playerClass != null)
        {
            playerClass.OnHit?.Invoke(Mathf.Max(0, swordDamage - playerClass.Def));
            Debug.Log($"남은 채력{playerClass.Hp}");
        }

    }
    */
}