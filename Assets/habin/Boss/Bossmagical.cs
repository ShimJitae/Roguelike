using UnityEngine;

public class Bossmagical : MonoBehaviour
{
    [SerializeField] private float destroytimer = 10f;
    [SerializeField] private Transform player;
    private float Damage;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.5f, 0.5f);
        }
        Destroy(gameObject, destroytimer);

    }

}
