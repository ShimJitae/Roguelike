using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    [SerializeField] private SpriteRenderer[] sp;
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
        sp = GetComponentsInChildren<SpriteRenderer>();
        
    }

    void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detcetRange, playerLayer);
        Collider2D attack = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (player != null)
        {
            TrackingPlayer();
            Debug.Log("발견");
            return;
        }
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
        rb.linearVelocity = direction * moveSpeed;
        float distance = Vector2.Distance(rb.position, targetPoint.position);
        if (distance < 0.1f)
        {
            ChangeTargetPoint();
        }
        MonsterFlip();
        
    }
    private void TrackingPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        if(player.position.x < transform.position.x)
        {
           Transform unitRoot = transform.Find("UnitRoot");

            unitRoot.localScale = new Vector3(1, 1, 1);
        }
        else if (player.position.x > transform.position.x)
        {
            Transform unitRoot = transform.Find("UnitRoot");

            unitRoot.localScale = new Vector3(-1, 1, 1);
        }

    }
    //몬스터 방향을 바꾸는 함수
    private void MonsterFlip()
    {
        if (currentIndex == 0)
        {
            Transform unitRoot = transform.Find("UnitRoot");

            unitRoot.localScale = new Vector3(1, 1, 1);
        }
        else if (currentIndex == 1)
        {
            Transform unitRoot = transform.Find("UnitRoot");

            unitRoot.localScale = new Vector3(-1, 1, 1);
        }
    }
    //다음 목표로 이동하는 함수
    private void ChangeTargetPoint()
    {
        if (currentIndex == 0)
        {
            currentIndex = 1;
        }
        else
        {
            currentIndex = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detcetRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
