using Unity.Android.Gradle.Manifest;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator an;
    private CharacterAnimController ancon;
    private SwordAttack swordAttack;
    private MonsterClass monsterClass;
    [SerializeField] private Transform AttackField;
    private SpriteRenderer[] sp;

    [Header("이동 및 감지")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public Transform player;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float detcetRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;

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
        monsterClass = GetComponent<MonsterClass>();
        rb = GetComponent<Rigidbody2D>();
        ancon = GetComponent<CharacterAnimController>();
        Transform unitRoot = transform.Find("UnitRoot");
        an = unitRoot.GetComponent<Animator>();
        swordAttack = GetComponent<SwordAttack>();
        if (swordObject != null)
        {
            swordAttack = swordObject.GetComponentInChildren<SwordAttack>(true);
        }

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
                    rb.linearVelocity = Vector2.zero;
                    monsterClass.PlayerAttack();
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
    public void SpawnSword()
    {
        swordObject.SetActive(true);
        //검공격에 대미지 저장
        swordAttack.SetDamage(monsterClass.Atk);
    }
    public void RemoveSword()
    {
        swordObject.SetActive(false);

    }
    public void SpawnArrow()
    {
        //화살 생성
        GameObject arrow = Instantiate(arrowObject, AttackField.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetDamage(monsterClass.Atk);
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        if (AttackField.localPosition.x == -0.5f)
        {
            arrowRb.linearVelocity = -AttackField.right * arrowSpeed;
        }
        else if (AttackField.localPosition.x == 0.5f)
        {
            arrowRb.linearVelocity = AttackField.right * arrowSpeed;
        }
    }
    private void TrackingPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        if (player.position.x < transform.position.x)
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
    private IEnumerator HitEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (SpriteRenderer sps in sp)
            {
                sps.enabled = false;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (SpriteRenderer sps in sp)
            {
                sps.enabled = true;
            }

            yield return new WaitForSeconds(0.1f);
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
