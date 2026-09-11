using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Animator anim;

    [Header("시작 방향")]
    [SerializeField] private Vector2 startDirection = Vector2.down;

    private bool isMovementLocked = false;

    private Vector2 moveInput;
    private Vector2 lastDirection;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Start()
    {
        // Inspector에서 지정한 방향으로 시작
        lastDirection = startDirection;

        // 시작하자마자 해당 방향 Idle 재생
        UpdateAnimation();
    }


    void Update()
    {
        // =========================
        // 이동 잠금 상태
        // =========================
        if (isMovementLocked)
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;

            UpdateAnimation();
            return;
        }


        // =========================
        // 이동 입력
        // =========================
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");


        // =========================
        // 대각선 이동 방지
        // =========================
        if (moveInput.x != 0 && moveInput.y != 0)
        {
            // 이전에 좌우를 보고 있었다면 좌우 이동 유지
            if (lastDirection.x != 0)
            {
                moveInput.y = 0;
            }
            // 이전에 위아래를 보고 있었다면 위아래 이동 유지
            else
            {
                moveInput.x = 0;
            }
        }


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
        // =========================
        // 이동 잠금 상태
        // =========================
        if (isMovementLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        // =========================
        // 실제 이동
        // =========================
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


    // =========================
    // 이동 잠금 / 해제
    // =========================
    public void SetMovementLocked(bool locked)
    {
        isMovementLocked = locked;

        if (locked)
        {
            moveInput = Vector2.zero;

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            // 즉시 Idle 애니메이션으로 변경
            if (anim != null)
            {
                UpdateAnimation();
            }
        }
    }
}