using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용")]
    [SerializeField] private float interactionDistance = 0.8f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    // 마지막으로 바라보는 방향
    private Vector2 lookDirection = Vector2.down;


    void Update()
    {
        UpdateLookDirection();


        // =========================
        // 상호작용
        // =========================
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 대화창이 이미 열려 있으면 다음 대사
            if (dialogueManager != null &&
                dialogueManager.IsDialogueOpen())
            {
                dialogueManager.ContinueDialogue();
                return;
            }

            // 대화창이 없으면 새 상호작용
            TryInteract();
        }
    }


    private void UpdateLookDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        // 입력이 없으면 마지막 방향 유지
        if (horizontal == 0 && vertical == 0)
        {
            return;
        }


        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
            {
                lookDirection = Vector2.right;
            }
            else
            {
                lookDirection = Vector2.left;
            }
        }
        else
        {
            if (vertical > 0)
            {
                lookDirection = Vector2.up;
            }
            else
            {
                lookDirection = Vector2.down;
            }
        }
    }


    private void TryInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast( transform.position, lookDirection, interactionDistance, interactableLayer );


        if (hit.collider == null)
        {
            return;
        }


        Debug.Log("상호작용 성공 : " + hit.collider.gameObject.name);


        // =========================
        // 퍼즐 책상
        // =========================
        PuzzleDesk puzzleDesk = hit.collider.GetComponentInParent<PuzzleDesk>();

        if (puzzleDesk != null)
        {
            puzzleDesk.Interact();
            return;
        }


        // =========================
        // 교실 탈출 문 이벤트
        // =========================
        ClassroomDoorEvent classroomDoor = hit.collider.GetComponentInParent<ClassroomDoorEvent>();

        if (classroomDoor != null)
        {
            classroomDoor.Interact();
            return;
        }
        // =========================
        // 복도 잠긴 교실 문
        // =========================
        LockedClassroomDoor lockedDoor = hit.collider.GetComponentInParent<LockedClassroomDoor>();

        if (lockedDoor != null)
        {
            lockedDoor.Interact();
            return;
        }

        // =========================
        // 조사 오브젝트
        // =========================
        InspectableObject inspectable = hit.collider.GetComponentInParent<InspectableObject>();

        if (inspectable != null)
        {
            if (inspectable.UseHorrorText())
            {
                dialogueManager.ShowHorrorText( inspectable.GetMessage() );
            }
            else
            {
                dialogueManager.ShowDialogue( inspectable.GetMessage());
            }

            return;
        }


        // =========================
        // 전등 스위치
        // =========================
        LightSwitch lightSwitch = hit.collider.GetComponentInParent<LightSwitch>();

        if (lightSwitch != null)
        {
            lightSwitch.Interact();
            return;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine( transform.position, transform.position + (Vector3)(lookDirection * interactionDistance) );
    }
}