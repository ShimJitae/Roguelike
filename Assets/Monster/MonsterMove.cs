using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rb;
    Animator an;
    [SerializeField] private MonsterData monsterData;
    [Header("이동 및 감지")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float detcetRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detcetRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
