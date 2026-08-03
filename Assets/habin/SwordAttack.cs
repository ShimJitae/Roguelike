using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private SpriteRenderer sp;
    [SerializeField] private Transform player;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어에게 접촉시 삭제 추후 대미지 넣기
        }
    }
}