using UnityEngine;

public class Arrow : MonoBehaviour
{
    private SpriteRenderer sp;
    [SerializeField] private float destroytimer = 10f;
    [SerializeField] private Transform player;
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어에게 접촉시 삭제 추후 대미지 넣기
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
