using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Converters;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private CharacterAnimController ancon;
    private SwordAttack swordAttack;
    private MonsterClass monsterClass;
    [SerializeField] private Transform AttackField;
    private SpriteRenderer[] sp;
    private CapsuleCollider2D col;
    public bool isAttacking;
    public bool isDeath;

    [Header("이동 및 감지")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public Transform player;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float detcetRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float maxHeightDifference = 1.5f;
    [SerializeField] private Transform unitRoot;

    [SerializeField] private float attackCooldown = 2f;
    [Header("근접 공격")]
    [SerializeField] private GameObject swordObject;
    [Header("원거리 공격")]
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private float arrowSpeed;
    

    private float nextAttackTime { get; set; }

    private int currentIndex = 0;
    private int moveDirection = 1;
    private void Awake()
    {
        sp = GetComponentsInChildren<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        monsterClass = GetComponent<MonsterClass>();
        rb = GetComponent<Rigidbody2D>();
        ancon = GetComponent<CharacterAnimController>();
        unitRoot = transform.Find("UnitRoot");
        an = unitRoot.GetComponent<Animator>();
        swordAttack = GetComponent<SwordAttack>();
        if (swordObject != null)
        {
            swordAttack = swordObject.GetComponentInChildren<SwordAttack>(true);
        }
    }
    private void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!monsterClass.isAlive)
        {
            if (isDeath == false)
            {
                rb.linearVelocity = Vector2.zero;
                col.isTrigger = true;
                rb.gravityScale = 0;
                ancon.PlayDeath();
                isDeath = true;
            }
            return;
        }
        MonsterAlivePlay();

    }

    private void StartAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        an.SetFloat("IsSpeed", 0f);
    }

    private void TrackingPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        if (player.position.x < transform.position.x)
        {
            unitRoot.localScale = new Vector3(1, 1, 1);
            //AttackField.localPosition = new Vector3(-0.5f, 0.2f);
        }
        else if (player.position.x > transform.position.x)
        {
            unitRoot.localScale = new Vector3(-1, 1, 1);
            //AttackField.localPosition = new Vector2(0.5f, 0.2f);
        }

    }
    //몬스터 방향을 바꾸는 함수
    private void MonsterFlip()
    {
        if (currentIndex == 0)
        {
            unitRoot.localScale = new Vector3(1, 1, 1);
        }
        else if (currentIndex == 1)
        {
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
    private void MonsterAlivePlay()
    {
        Collider2D findPlayer = Physics2D.OverlapCircle(transform.position, detcetRange, playerLayer);
        Collider2D attack = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);
        if (isAttacking == true)
        {
            rb.linearVelocity = Vector2.zero;
            an.SetFloat("IsSpeed", 0f);
            return;
        }
        if (isAttacking == false)
        {
            an.SetFloat("IsSpeed", 3f);
        }
        //범위안의 플레이어 추격
        if (findPlayer != null && heightDifference <= maxHeightDifference)
        {

            if (attack != null)
            {
                if (Time.time >= nextAttackTime)
                {
                    StartAttack();
                    monsterClass.PlayerAttack();
                    nextAttackTime = Time.time + attackCooldown;
                    return;
                }
            }
            TrackingPlayer();
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
        Vector2 direction = ((Vector2)targetPoint.position - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        an.SetFloat("IsSpeed", direction.magnitude);
        float distance = Vector2.Distance(rb.position, targetPoint.position);
        if (distance < 0.1f)
        {
            ChangeTargetPoint();
        }
        MonsterFlip();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        PlayerClass playerclass = collision.gameObject.GetComponent<PlayerClass>();
        playerclass.OnHit?.Invoke(Mathf.Max(0, monsterClass.MonsterBodyAttack - playerclass.Def));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detcetRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
