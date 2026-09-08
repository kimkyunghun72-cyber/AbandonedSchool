using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isMovementLocked = false;

    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {

        if (isMovementLocked)
        {
            moveInput = Vector2.zero;
            UpdateAnimation();
            return;
        }
        // =========================
        // 이동 입력
        // =========================
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 대각선 이동 속도가 빨라지지 않도록 정규화
        moveInput = moveInput.normalized;


        // =========================
        // 마지막 방향 저장
        // =========================
        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput;
        }


        // =========================
        // 애니메이션
        // =========================
        UpdateAnimation();
    }


    void FixedUpdate()
    {
        if (isMovementLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }


    private void UpdateAnimation()
    {
        // =========================
        // 이동 중
        // =========================
        if (moveInput != Vector2.zero)
        {
            // 좌우 방향 우선
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                if (moveInput.x > 0)
                {
                    anim.Play("Walk_Right");
                }
                else
                {
                    anim.Play("Walk_Left");
                }
            }
            else
            {
                if (moveInput.y > 0)
                {
                    anim.Play("Walk_Up");
                }
                else
                {
                    anim.Play("Walk_Down");
                }
            }

            return;
        }


        // =========================
        // 정지 상태
        // =========================
        if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
        {
            if (lastDirection.x > 0)
            {
                anim.Play("Idle_Right");
            }
            else
            {
                anim.Play("Idle_Left");
            }
        }
        else
        {
            if (lastDirection.y > 0)
            {
                anim.Play("Idle_Up");
            }
            else
            {
                anim.Play("Idle_Down");
            }
        }
    }
    public void SetMovementLocked(bool locked)
    {
        isMovementLocked = locked;

        if (locked)
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }
}