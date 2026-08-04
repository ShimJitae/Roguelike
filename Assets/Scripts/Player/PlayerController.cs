using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
     플레이어 컨트롤관련(이동, 공격, 점프)로직
        기본적으로 WASD 이동, X 공격, C 점프
     */
    [SerializeField] private Rigidbody2D rb;
    private Collision coll;
    private PlayerAnim anim;

    // 점프 관련
    [SerializeField] private float jumpPower;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] private bool isPressedJump;

    // 이동 관련 - 나중에 Stat쪽으로 빠질 수 있음
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    // 벽 타기 관련
    [SerializeField] private float slideSpeed;
    [Range(1, 10)]
    [SerializeField] float wallJumpPower = 7f;
    [SerializeField] private float wallJumpDelay = 0.1f;

    // 벽슬라이드/벽점프 상태
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpTimer;

    // 키입력
    public Vector2 moveInput { get; private set; }
    public bool isFlip { get; private set; }

    private void Awake()
    {
        coll = GetComponent<Collision>();
        anim = GetComponentInChildren<PlayerAnim>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Start()
    {
        
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        

        if (moveInput.x < 0)
        {
            isFlip = false;
            anim.Move();
            
            Debug.Log($"Flip: {isFlip}");
        }
        else if(moveInput.x > 0)
        {
            isFlip = true;
            anim.Move();
            Debug.Log($"Flip: {isFlip}");
        }
        else
        {
            anim.Idle();
        }
    }

    public void OnAttack(InputValue value)
    {
        anim.Attack();
    }

    public void OnJump(InputValue value)
    {
        isPressedJump = value.isPressed;

        if (!isPressedJump)
            return;

        

        if (isWallSliding && !coll.onGround)
        {
            DoWallJump();
            return;
        }

        if (coll.onGround)
        {
            // 수평 속도는 유지, 수직만 초기화 후 점프
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.linearVelocity += Vector2.up * jumpPower;
        }

        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

    }

    void DoWallJump()
    {
        int wallDir = GetWallDirectionX();   // +1: 오른쪽 벽, -1: 왼쪽 벽

        // 벽 반대 방향으로 수평, 위로 수직 점프
        Vector2 jumpVel = new Vector2(-wallDir * wallJumpPower, jumpPower);

        rb.linearVelocity = jumpVel;

        isWallJumping = true;
        wallJumpTimer = wallJumpDelay;   // 잠깐 동안 Move() 가 수평 속도를 덮어쓰지 않게 함

        Debug.Log($"벽점프! wallDir: {wallDir}, jumpVel: {jumpVel}");
    }

    void BetterJump()
    {
        if (rb.linearVelocityY < 0f)
            rb.gravityScale = fallMultiplier;
        else if (rb.linearVelocityY > 0f && !isPressedJump)
        {
            rb.linearVelocityY = 0f;
            rb.gravityScale = fallMultiplier;
        }
        else
            rb.gravityScale = 1f;
    }

    int GetWallDirectionX()
    {
        // 플레이어 기준 벽 방향: 오른쪽 벽이면 +1, 왼쪽 벽이면 -1
        if (coll.onRightWall) return 1;
        if (coll.onLeftWall) return -1;
        return 0;
    }

    void Move()
    {
        // 벽점프 중에는 수평 입력으로 속도를 덮어쓰지 않음
        if (isWallJumping)
            return;

        // 기본 이동
        rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb.linearVelocity.y);

        bool hittingRightWall = coll.onRightWall && moveInput.x > 0;
        bool hittingLeftWall = coll.onLeftWall && moveInput.x < 0;
        bool onWall = hittingRightWall || hittingLeftWall;
        bool notOnGround = !coll.onGround;

        // 벽슬라이드 조건: 벽에 붙어 있고, 공중이며, 벽 방향으로 입력 중
        if (onWall && notOnGround)
        {
            isWallSliding = true;

            // X 속도 멈추고, Y 속도를 슬라이드 속도로 제한
            rb.linearVelocityX = 0f;
            rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -slideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }


    private void FixedUpdate()
    {
        // 벽점프 딜레이 해제
        if (isWallJumping)
        {
            wallJumpTimer -= Time.fixedDeltaTime;
            if (wallJumpTimer <= 0f)
                isWallJumping = false;
        }

        Move();
        BetterJump();
    }
    
}
