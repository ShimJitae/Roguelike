using UnityEngine;

public class BossMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private CharacterAnimController ancon;
    private MonsterClass monsterClass;
    [SerializeField] private Transform AttackField;
    [Header("이동 및 감지")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public Transform player;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown = 2f;
    [Header("근접 공격")]
    [SerializeField] private GameObject prickObject;
    [Header("원거리 공격")]
    [SerializeField] private GameObject swordAuraObject;
    [SerializeField] private float swordAuraSpeed;
    private float nextAttackTime { get; set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        ancon = GetComponent<CharacterAnimController>();
        monsterClass = GetComponent<MonsterClass>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
