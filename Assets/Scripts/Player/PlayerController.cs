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

    [SerializeField] private float jumpPower;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

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

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb.linearVelocity.y);
    }
}
