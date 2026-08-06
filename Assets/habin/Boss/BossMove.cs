using DG.Tweening;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Animator an;
    private SpriteRenderer sp;
    private BossAnimController ancon;
    private MonsterClass monsterClass;
    private BossMeleeAttack bossMeleeAttack;
    private PlayerClass playerClass;
    private bool isAttacking;
    [Header("이동 및 감지")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public Transform player;
    [SerializeField] private float detcetRange;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float maxHeightDifference = 30.5f;
    [Header("근접 공격")]
    [SerializeField] private float meleeAttackRange;
    [SerializeField] private GameObject meleeAttackObject;
    [SerializeField] private Transform meleeAttackField;
    [Header("원거리 공격")]
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private GameObject swordAuraObject;
    [SerializeField] private Transform swordAuraField;
    [SerializeField] private float swordAuraSpeed;
    [Header("마법 공격")]
    [SerializeField] private GameObject magObject;
    [SerializeField] private Transform magField;
    private float nextAttackTime { get; set; }
    private int attackCount = 0;
    private bool isDeath = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        ancon = GetComponent<BossAnimController>();
        monsterClass = GetComponent<MonsterClass>();
        if (meleeAttackObject != null)
        {
            bossMeleeAttack = meleeAttackObject.GetComponentInChildren<BossMeleeAttack>(true);
        }

    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerClass = player.GetComponent<PlayerClass>();
    }
    private void Update()
    {
        if (!monsterClass.isAlive)
        {
            if (isDeath == false)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0;
                col.isTrigger = true;
                playerClass.mobCount++;
                ancon.PlayDeath();
                isDeath = true;
                
            }
            
            return;
        }
        BossAlivePlay();

    }

    private void StartAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        an.SetFloat("IsSpeed", 0f);

    }
    public void EndAttack()
    {
        isAttacking = false;
    }

    public void AttackFlip()
    {
        if (sp.flipX == true)
        {
            meleeAttackField.localPosition = new Vector3(-0.5f, 0);
            swordAuraField.localPosition = new Vector3(-0.3f, 0);
        }
        else if (sp.flipX == false)
        {
            meleeAttackField.localPosition = new Vector3(0.5f, 0);
            swordAuraField.localPosition = new Vector3(0.3f, 0);
        }
    }
    public void SpawnSword()
    {

        meleeAttackObject.SetActive(true);
        //검공격에 대미지 저장
        bossMeleeAttack.SetDamage(monsterClass.Atk);
    }
    public void RemoveSword()
    {
        meleeAttackObject.SetActive(false);

    }
    public void SpawnSwordAura()
    {
        //검기 생성
        GameObject swordAura = Instantiate(swordAuraObject, swordAuraField.position, Quaternion.identity);
        BossRangedAttack arrowScript = swordAura.GetComponent<BossRangedAttack>();
        arrowScript.SetDamage(monsterClass.Atk);
        Rigidbody2D swordAuraRb = swordAura.GetComponent<Rigidbody2D>();
        Vector2 playerPosition = (player.position - swordAuraField.position).normalized;
        swordAuraRb.linearVelocity = playerPosition * swordAuraSpeed;
    }
    public void SpawnMag()
    {
        GameObject mag = Instantiate(magObject, magField.position, Quaternion.identity);
        Bossmagical magScript = mag.GetComponent<Bossmagical>();
        magScript.SetDamage(monsterClass.Atk);
    }
    public void BossAlivePlay()
    {
        Collider2D detcetPlayer = Physics2D.OverlapCircle(transform.position, detcetRange, playerLayer);
        Collider2D meleeAttack = Physics2D.OverlapCircle(transform.position, meleeAttackRange, playerLayer);
        Collider2D rangedAttack = Physics2D.OverlapCircle(transform.position, rangedAttackRange, playerLayer);
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            an.SetFloat("IsSpeed", 0f);
            return;
        }
        //몬스터 및 공격 범위의 방향
        if (player.position.x < transform.position.x)
        {

            sp.flipX = true;
        }
        else if (player.position.x > transform.position.x)
        {
            sp.flipX = false;
        }
        AttackFlip();


        if (attackCount >= 5)
        {
            if (Time.time >= nextAttackTime)
            {
                StartAttack();
                ancon.PlayCast();
                attackCount = 0;
                nextAttackTime = Time.time + attackCooldown;
                return;
            }
        }
        if (meleeAttack != null && heightDifference <= maxHeightDifference)
        {

            if (Time.time >= nextAttackTime)
            {
                StartAttack();
                attackCount++;
                ancon.PlayAttack2();
                nextAttackTime = Time.time + attackCooldown;
                return;
            }

        }
        if (rangedAttack != null && heightDifference <= maxHeightDifference)
        {

            if (Time.time >= nextAttackTime)
            {
                StartAttack();
                attackCount++;
                rb.linearVelocity = Vector2.zero;
                ancon.PlayAttack();
                nextAttackTime = Time.time + attackCooldown;
                return;
            }
        }

        if (detcetPlayer != null && heightDifference <= maxHeightDifference)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            an.SetFloat("IsSpeed", direction.magnitude);
            return;
        }
        rb.linearVelocity = Vector2.zero;
        an.SetFloat("IsSpeed", 0f);

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
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detcetRange);
    }

}

