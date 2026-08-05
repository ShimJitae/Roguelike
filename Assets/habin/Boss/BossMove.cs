using DG.Tweening;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private SpriteRenderer sp;
    private BossAnimController ancon;
    private MonsterClass monsterClass;
    private BossMeleeAttack bossMeleeAttack;
    [SerializeField] private Transform AttackField;
    [Header("이동 및 감지")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public Transform player;
    [SerializeField] private float detcetRange;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float saveAttackCooldown;
    [Header("근접 공격")]
    [SerializeField] private float meleeAttackRange;
    [SerializeField] private GameObject meleeAttackObject;
    [Header("원거리 공격")]
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private GameObject swordAuraObject;
    [SerializeField] private Transform swordAuraField;
    [SerializeField] private float swordAuraSpeed;
    [Header("마법 공격")]
    [SerializeField] private GameObject magObject;
    [SerializeField] private Transform magField;
    [SerializeField] private float magSpeed;
    private float nextAttackTime { get; set; }
    private int attackCount = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        ancon = GetComponent<BossAnimController>();
        monsterClass = GetComponent<MonsterClass>();
        saveAttackCooldown = attackCooldown;
        if (meleeAttackObject != null)
        {
            bossMeleeAttack = meleeAttackObject.GetComponentInChildren<BossMeleeAttack>(true);
        }
    }
    private void Update()
    {
        Collider2D detcetPlayer = Physics2D.OverlapCircle(transform.position, detcetRange, playerLayer);
        Collider2D meleeAttack = Physics2D.OverlapCircle(transform.position, meleeAttackRange, playerLayer);
        Collider2D rangedAttack = Physics2D.OverlapCircle(transform.position, rangedAttackRange, playerLayer);
        if (attackCount < 8)
        {
            if (player.position.x < transform.position.x)
            {
                sp.flipX = true;
            }
            else if (player.position.x > transform.position.x)
            {
                sp.flipX = false;
            }
            if (rangedAttack != null)
            {
                if (meleeAttack != null)
                {
                    if (Time.time >= nextAttackTime)
                    {
                        attackCount++;
                        rb.linearVelocity = Vector2.zero;
                        ancon.PlayAttack2();
                        nextAttackTime = Time.time + attackCooldown;
                    }

                    return;
                }
                else if (rangedAttack != null)
                {
                    if (Time.time >= nextAttackTime)
                    {
                        attackCount++;
                        rb.linearVelocity = Vector2.zero;
                        ancon.PlayAttack();
                        nextAttackTime = Time.time + attackCooldown;
                    }

                }

            }
            else if (meleeAttack == null)
            {
                if (detcetPlayer != null)
                {
                    Vector2 direction = (player.position - transform.position).normalized;
                    rb.linearVelocity = direction * moveSpeed;
                    an.SetFloat("IsSpeed", direction.magnitude);
                }

            }
        else if (attackCount >= 8 )
        {
                attackCooldown = 30f;
                //마법 공격
                ancon.PlayCast();
                attackCount = 0;
                attackCooldown = saveAttackCooldown;
        }
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
        Rigidbody2D arrowRb = swordAura.GetComponent<Rigidbody2D>();
        if (sp.flipX == true)
        {
            arrowRb.linearVelocity = -swordAuraField.right * swordAuraSpeed;
        }
        else if (sp.flipX == false)
        {
            arrowRb.linearVelocity = swordAuraField.right * swordAuraSpeed;
        }
    }
    public void SpawnMag()
    {
        GameObject mag = Instantiate(magObject, magField.position, Quaternion.identity);
        Bossmagical magScript = mag.GetComponent<Bossmagical>();
        magScript.SetDamage(monsterClass.Atk);
        Rigidbody2D arrowRb = mag.GetComponent<Rigidbody2D>();

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

