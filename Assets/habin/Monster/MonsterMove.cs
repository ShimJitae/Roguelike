using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private SpriteRenderer sp;
    [SerializeField] private MonsterData monsterData;
    [Header("이동 및 감지")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float detcetRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    
    private int currentIndex = 0; 
    private int moveDirection = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //포인트가 없을때 움직이지 않음
        if (movePoints == null || movePoints.Length == 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        //이동지점 확인
        Transform targetPoint = movePoints[currentIndex];
        //이동지점으로 이동
        Vector2 direction =((Vector2)targetPoint.position - rb.position).normalized;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detcetRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
