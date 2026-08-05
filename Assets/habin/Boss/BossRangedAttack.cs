using UnityEngine;

public class BossRangedAttack : MonoBehaviour
{
    [SerializeField] private float destroytimer = 10f;
    [SerializeField] private Transform player;
    private float Damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-0.5f,0.5f);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.5f, 0.5f);
        }
        Destroy(gameObject, destroytimer);

    }
    public void SetDamage(float attack)
    {
        Damage = attack;
    }
    // Update is called once per frame
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
