using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAnim : MonoBehaviour
{
    private SPUM_Prefabs spumPrefabs;
    private PlayerController move;
    
    [SerializeField] private PlayerState currentState = PlayerState.IDLE;
    private bool isAttacking;
    private bool previousFlipState;

    private void Awake()
    {
        spumPrefabs = GetComponentInParent<SPUM_Prefabs>();
        move = GetComponentInParent<PlayerController>();
    }

    void Start()
    {
        spumPrefabs.OverrideControllerInit();

        spumPrefabs.PlayAnimation(PlayerState.IDLE, 0);
    }

    public void FlipX()
    {
        if (isAttacking)
            return;

        Vector3 scale = transform.localScale;
        scale.x = move.isFlip ? -1.0f : 1.0f;
        transform.localScale = scale;

    }


    /* 벽 점프 때 필요할 수도 있음
    public void Flip(int side)
    {

        if (move.wallGrab || move.wallSlide)
        {
            if (side == -1 && transform.localScale.x == 1.0f)
                return;

            if (side == 1 && transform.localScale.x == -1.0f)
            {
                return;
            }
        }

        bool state = (side == 1) ? false : true;
        transform.localScale.x = 1.0f;
    }
    */

    public void Move()
    {
        currentState = PlayerState.MOVE;
        if (move.isFlip != previousFlipState)
        {
            FlipX();
            previousFlipState = move.isFlip;
        }
        spumPrefabs.PlayAnimation(currentState, 0);
    }

    public void Idle()
    {
        currentState = PlayerState.IDLE;
        FlipX();
        spumPrefabs.PlayAnimation(currentState, 0);
    }

    public void Attack()
    {
        if (isAttacking)
            return;
        isAttacking = true;
        currentState = PlayerState.ATTACK;
        spumPrefabs.PlayAnimation(currentState, 0);
    }

    // 애니메이션 이벤트로 호출될 메서드
    public void OnAttackEnd()
    {
        isAttacking = false;
        if (move.moveInput.x != 0)
        {
            // 이동 중이면 Move 상태로 전환
            currentState = PlayerState.MOVE;
            FlipX();
            spumPrefabs.PlayAnimation(currentState, 0);
        }
        else
        {
            // 이동 입력 없으면 Idle
            currentState = PlayerState.IDLE;
            spumPrefabs.PlayAnimation(currentState, 0);
        }

    }

}
