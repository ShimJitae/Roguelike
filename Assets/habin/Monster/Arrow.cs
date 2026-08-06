using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Arrow : MonoBehaviour
{
    private SpriteRenderer sp;
    [SerializeField] private float destroytimer = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private float arrowDamage;

    // 화살데이터
    private Vector2 direction;
    private float speed = 7f;
    private float lifetime = 3f;

    // 화살을 생성한 오브젝트 데이터
    [SerializeField] private Entity entity;
    private Transform owner;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetOwner(Transform ownerTransform)
    {
        owner = ownerTransform;
        entity = ownerTransform.GetComponent<Entity>();
    }

    public void SetDamage(float attack)
    {
        arrowDamage = attack;
    }

    void Start()
    {
        Init();
        /*
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sp = GetComponent<SpriteRenderer>();
        if (player.position.x < transform.position.x)
        {
            sp.flipX = false;
        }
        if (player.position.x > transform.position.x)
        {
            sp.flipX = true;
        }

        //생성후 destroytimer에 맞춰 사라짐
        Destroy(gameObject, destroytimer);
        */

    }

    private void Init()
    {
        arrowDamage = entity.Atk;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("화살 트리거 in");
        //같은 팀은 때리지 않는다.
        if (collision.CompareTag(owner.tag))
            return;

        if (collision.CompareTag("Ground"))
            Destroy(gameObject);

        Entity target = collision.GetComponent<Entity>();

        target.OnHit?.Invoke(Mathf.Max(0, arrowDamage - target.Def));
        Debug.Log("화살 피격!");
        Destroy(gameObject);

    }
}
