using UnityEngine;

public class Bossmagical : MonoBehaviour
{
    [SerializeField] private float destroytimer = 10f;
    [SerializeField] private Transform player;
    private SpriteRenderer sp;
    private Rigidbody2D rb;
    private float Damage;
    private float Speed = 5;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * Speed;
        if(player.position.x < transform.position.x)
        {
            sp.flipX = false;
        }
        else if (player.position.x > transform.position.x)
        {
            sp.flipX = true;
        }
        Destroy(gameObject, destroytimer);
    }
    public void SetDamage(float attack)
    {
        Damage = attack;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerClass playerClass = collision.gameObject.GetComponent<PlayerClass>();
            if (playerClass != null)
            {
                playerClass.OnHit?.Invoke(Mathf.Max(0, Damage - playerClass.Def));
                Debug.Log($"남은 채력{playerClass.Hp}");
                Destroy(gameObject);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

}
