using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float offSetX;
    [SerializeField] private float offSetY;
    private void Awake()
    {
        offSetY = 3.0f;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void LateUpdate()
    {
        Transform playerScale = player.transform.GetChild(0);
        /*
        if (playerScale.localScale.x == 1)
            offSetX = -2f;
        else
            offSetX = 2f;
        */
        transform.position = new Vector3(player.transform.position.x + offSetX, player.transform.position.y + offSetY, -10f);
    }
}
