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

        Debug.Log($"Jump Pressed : {isPressedJump}");
        if (coll.onGround)
        {
            // 수평 속도는 유지, 수직만 초기화 후 점프
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.linearVelocity += Vector2.up * jumpPower;
        }

        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

    }
    void BetterJump()
    {
        if (rb.linearVelocityY < 0f)
            rb.gravityScale = fallMultiplier;
        else if (rb.linearVelocityY > 0f && !isPressedJump)
            rb.gravityScale = lowJumpMultiplier;
        else
            rb.gravityScale = 1f;
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb.linearVelocity.y);

        if (coll.onRightWall && moveInput.x > 0 || coll.onLeftWall && moveInput.x < 0)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = -slideSpeed;
        }
    }

    private void FixedUpdate()
    {
        Move();
        BetterJump();
    }
    
}
