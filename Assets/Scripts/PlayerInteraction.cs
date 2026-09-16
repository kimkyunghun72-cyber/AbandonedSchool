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
        // 낡은 반 명단
        // =========================
        ClassRosterEvent classRoster = hit.collider.GetComponentInParent<ClassRosterEvent>();

        if (classRoster != null)
        {
            classRoster.Interact();
            return;
        }


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
        // 두 번째 게시판
        // =========================
        SecondBulletinBoard secondBulletin = hit.collider.GetComponentInParent<SecondBulletinBoard>();

        if (secondBulletin != null)
        {
            secondBulletin.Interact();
            return;
        }
        // =========================
        // 1-3반 문
        // =========================
        Classroom13Door classroom13Door = hit.collider.GetComponentInParent<Classroom13Door>();

        if (classroom13Door != null)
        {
            classroom13Door.Interact();
            return;
        }
        // =========================
        // 복도 소화기
        // =========================
        FireExtinguisherEvent fireExtinguisher =  hit.collider.GetComponentInParent<FireExtinguisherEvent>();

        if (fireExtinguisher != null)
        {
            fireExtinguisher.Interact();
            return;
        }
        // =========================
        // 1-3반 책 이벤트
        // =========================
        Classroom02BookEvent classroom02Book = hit.collider.GetComponentInParent<Classroom02BookEvent>();

        if (classroom02Book != null)
        {
            classroom02Book.Interact();
            return;
        }
        // =========================
        // 1-3반 급훈 이벤트
        // =========================
        Classroom02MottoEvent classroom02Motto =
            hit.collider.GetComponentInParent<Classroom02MottoEvent>();

        if (classroom02Motto != null)
        {
            classroom02Motto.Interact();
            return;
        }
        // =========================
        // 1-3반 칠판 이벤트
        // =========================
        Classroom02BlackboardEvent classroom02Blackboard = hit.collider.GetComponentInParent<Classroom02BlackboardEvent>();

        if (classroom02Blackboard != null)
        {
            classroom02Blackboard.Interact();
            return;
        }
        // =========================
        // 1-3반 비밀 스위치
        // =========================
        Classroom02SecretSwitch secretSwitch = hit.collider.GetComponentInParent<Classroom02SecretSwitch>();

        if (secretSwitch != null)
        {
            secretSwitch.Interact();
            return;
        }
        // =========================
        // 비밀방 주인공 사진
        // =========================
        Classroom02PlayerPhotoEvent playerPhotoEvent = hit.collider.GetComponentInParent<Classroom02PlayerPhotoEvent>();

        if (playerPhotoEvent != null)
        {
            playerPhotoEvent.Interact();
            return;
        }
        // =========================
        // 비밀방 다른 여학생 사진
        // =========================
        Classroom02OtherGirlsPhotoEvent otherGirlsPhoto = hit.collider.GetComponentInParent<Classroom02OtherGirlsPhotoEvent>();

        if (otherGirlsPhoto != null)
        {
            otherGirlsPhoto.Interact();
            return;
        }
        // =========================
        // 비밀방 선생님 생활지도 기록
        // =========================
        Classroom02TeacherRecordEvent teacherRecord = hit.collider.GetComponentInParent<Classroom02TeacherRecordEvent>();

        if (teacherRecord != null)
        {
            teacherRecord.Interact();
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
        // 복도 잠긴 교실 문
        // =========================
        LockedClassroomDoor lockedDoor = hit.collider.GetComponentInParent<LockedClassroomDoor>();

        if (lockedDoor != null)
        {
            lockedDoor.Interact();
            return;
        }
        // =========================
        // 복도 전등 스위치
        // =========================
        CorridorLightSwitch corridorLightSwitch = hit.collider.GetComponentInParent<CorridorLightSwitch>();

        if (corridorLightSwitch != null)
        {
            corridorLightSwitch.Interact();
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