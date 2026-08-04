using UnityEngine;

public class Arrow : MonoBehaviour
{
    private SpriteRenderer sp;
    [SerializeField] private float destroytimer = 10f;
    [SerializeField] private Transform player;
    private float arrowDamage;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sp = GetComponent<SpriteRenderer>();
        if (player.position.x < transform.position.x)
        {
            sp.flipX = false;
        }
        if (player.position.x > transform.position.x)
        {
            sp.flipX = true;
        }
        //생성후 destroytimer에 맞춰 사라짐
        Destroy(gameObject, destroytimer);

    }
    public void SetDamage(float attack)
    {
        arrowDamage = attack;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerClass playerClass = collision.gameObject.GetComponent<PlayerClass>();
            if (playerClass != null)
            {
                //대미지 나중에 TakeDamage만들기
                //playerClass.hp -= Mathf.Max(0, arrowDamage - playerClass.Def);

                playerClass.OnHit?.Invoke(Mathf.Max(0, arrowDamage - playerClass.Def));
                Debug.Log($"남은 채력{playerClass.Hp}");
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            //벽에 접촉시 삭제
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            //바닥에 접촉시 삭제
            Destroy(gameObject);
        }



    }
}
