using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private SPUM_Prefabs spumPrefabs;
    private PlayerState currentState = PlayerState.IDLE;

    private void Awake()
    {
        spumPrefabs = GetComponentInParent<SPUM_Prefabs>();
    }

    void Start()
    {
        spumPrefabs.OverrideControllerInit();

        spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = PlayerState.ATTACK;
            spumPrefabs.PlayAnimation(currentState, 0); // ATTACK 0 번 애니메이션 재생
        }
    }
}
