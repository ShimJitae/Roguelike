using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private SpriteRenderer sp;
    [SerializeField] private Transform player;
    private float swordDamage;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sp = GetComponent<SpriteRenderer>();
      
    }
    private void Update()
    {
        if (player.position.x > transform.position.x)
        {
            sp.flipX = false;
        }
        if (player.position.x < transform.position.x)
        {
            sp.flipX = true;
        }
    }
    public void SetDamage(float attack)
    {
        swordDamage = attack;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerClass playerClass = other.GetComponent<PlayerClass>();

        if (playerClass != null)
        {
            playerClass.OnHit?.Invoke(Mathf.Max(0, swordDamage - playerClass.Def));
            Debug.Log($"남은 채력{playerClass.Hp}");
        }

    }

}