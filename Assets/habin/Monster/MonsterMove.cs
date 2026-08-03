using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Windows;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private CharacterAnimController ancon;
    [SerializeField] private MonsterData monsterData;

    [SerializeField] private Transform AttackField;
    [Header("이동 및 감지")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public Transform player;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float detcetRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;

    [Header("원거리 화살")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private float arrowSpeed;
    private float nextAttackTime {  get; set; }

    private int currentIndex = 0; 
    private int moveDirection = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ancon = GetComponent<CharacterAnimController>();
        Transform unitRoot = transform.Find("UnitRoot");
        an = unitRoot.GetComponent<Animator>();

    }

    void Update()
    {
        Collider2D findPlayer = Physics2D.OverlapCircle(transform.position, detcetRange, playerLayer);
        Collider2D attack = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        //범위안의 플레이어 추격
        if (findPlayer != null)
        {
            TrackingPlayer();
            Debug.Log("발견");
            if (attack != null)
            {
                if (Time.time >= nextAttackTime)
                {
                    PlayerAttack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
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
        an.SetFloat("IsSpeed", direction.magnitude);
        float distance = Vector2.Distance(rb.position, targetPoint.position);
        if (distance < 0.1f)
        {
            ChangeTargetPoint();
        }
        MonsterFlip();
        
    }
    private void PlayerAttack()
    {
        ancon.PlayAttack();
        //몬스터 타입이 원거리이면
        if(monsterData.AttackType == MonsterAttackType.ranged)
        {
            GameObject arrow = Instantiate(arrowObject, AttackField.position,Quaternion.identity);
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if(AttackField.localPosition.x == -0.5f)
            {
                arrowRb.linearVelocity = -AttackField.right * arrowSpeed;
            }
            else if (AttackField.localPosition.x == 0.5f)
            {
                arrowRb.linearVelocity = AttackField.right * arrowSpeed;
            }
        }
    }
    private void TrackingPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        if(player.position.x < transform.position.x)
        {
           Transform unitRoot = transform.Find("UnitRoot");
            unitRoot.localScale = new Vector3(1, 1, 1);
            AttackField.localPosition = new Vector3(-0.5f, 0.2f);
        }
        else if (player.position.x > transform.position.x)
        {
            Transform unitRoot = transform.Find("UnitRoot");
            unitRoot.localScale = new Vector3(-1, 1, 1);
            AttackField.localPosition = new Vector2(0.5f, 0.2f);
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
