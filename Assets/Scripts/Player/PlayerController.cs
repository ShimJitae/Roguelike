using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
     플레이어 컨트롤관련(이동, 공격, 점프)로직
        기본적으로 WASD 이동, X 공격, C 점프
     */
    [SerializeField] private Transform childTransform;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector3 flip;

    [SerializeField] private float jumpPower;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;


    private void Awake()
    {
        childTransform = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
        flip = childTransform.localScale;
    }

    void Start()
    {
        
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if(moveInput.x != 0)
        {
            FlipX();
        }
    }

    public void OnAttack(InputValue value)
    {

    }

    public void OnJump(InputValue value)
    {

    }

    //TODO 이부분이 애니메이션 스크립트로 빠질수있음 현재는 자식 트랜스폼을 가져오는 형태
    public void FlipX()
    {
        flip.x = -flip.x;
        childTransform.localScale = flip;
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb.linearVelocity.y);

        //rb.linearVelocityX = moveInput.x * walkSpeed;
    }
}
