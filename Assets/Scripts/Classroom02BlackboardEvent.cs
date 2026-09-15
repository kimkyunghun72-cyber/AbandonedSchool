using System.Collections;
using UnityEngine;

public class Classroom02BlackboardEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("기존 칠판 조사")]
    [SerializeField] private InspectableObject inspectableObject;

    [Header("급훈 이벤트")]
    [SerializeField] private Classroom02MottoEvent mottoEvent;

    [Header("플레이어")]
    [SerializeField] private PlayerController playerController;

    [Header("숨겨진 스위치")]
    [SerializeField] private Collider2D secretSwitchCollider;

    [Header("칠판 이동")]
    [SerializeField] private Transform blackboardToMove;

    // 오른쪽으로 얼마나 움직일지
    [SerializeField] private float moveDistanceX = 0.8f;

    // 칠판이 움직이는 데 걸리는 시간
    [SerializeField] private float moveDuration = 1.2f;


    private bool hasFoundSwitch = false;
    private bool isRevealing = false;


    private void Start()
    {
        // 스위치는 처음부터 존재하지만
        // 칠판 뒤에 가려져 있고 상호작용은 불가능
        if (secretSwitchCollider != null)
        {
            secretSwitchCollider.enabled = false;
        }
    }


    public void Interact()
    {
        // =========================
        // 칠판 이동 연출 중에는 조사 불가
        // =========================
        if (isRevealing)
        {
            return;
        }


        // =========================
        // 급훈 단서를 아직 찾지 않았을 때
        // 기존 InspectableObject 대사 사용
        // =========================
        if (mottoEvent == null || !mottoEvent.HasFoundClue())
        {
            if (inspectableObject != null)
            {
                if (inspectableObject.UseHorrorText())
                {
                    dialogueManager.ShowHorrorText( inspectableObject.GetMessage() );
                }
                else
                {
                    dialogueManager.ShowDialogue( inspectableObject.GetMessage());
                }
            }

            return;
        }


        // =========================
        // 급훈 단서 획득 후 첫 조사
        // =========================
        if (!hasFoundSwitch)
        {
            dialogueManager.ShowDialogueSequence(
                "렌",
                "…아까 그 글.",
                "계속 앞을 보라고 했다는 건…",
                "이 칠판을 말하는 건가?",
                "…잠깐.",
                "칠판 옆쪽에 작은 틈이 있다.",
                "안쪽에 뭔가 숨겨져 있다."
            );

            hasFoundSwitch = true;

            StartCoroutine(RevealSwitchRoutine());

            return;
        }


        // =========================
        // 스위치 발견 후 칠판 재조사
        // =========================
        dialogueManager.ShowDialogue(
            "칠판 뒤쪽에 작은 스위치가 드러나 있다."
        );
    }


    private IEnumerator RevealSwitchRoutine()
    {
        isRevealing = true;


        // =========================
        // 현재 대화가 전부 끝날 때까지 기다림
        // =========================
        while (dialogueManager.IsDialogueOpen())
        {
            yield return null;
        }


        // =========================
        // 플레이어 이동 잠금
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        // 연출 시작 전 살짝 멈춤
        yield return new WaitForSeconds(0.2f);


        // =========================
        // 칠판 이동
        // =========================
        if (blackboardToMove != null)
        {
            Vector3 startPosition = blackboardToMove.localPosition;

            Vector3 targetPosition = startPosition + new Vector3(moveDistanceX, 0f, 0f);


            float elapsedTime = 0f;


            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;

                float t = elapsedTime / moveDuration;

                blackboardToMove.localPosition =
                    Vector3.Lerp( startPosition, targetPosition, t );

                yield return null;
            }


            // 정확한 최종 위치로 맞춤
            blackboardToMove.localPosition = targetPosition;
        }


        // =========================
        // 스위치 상호작용 활성화
        // =========================
        if (secretSwitchCollider != null)
        {
            secretSwitchCollider.enabled = true;
        }


        yield return new WaitForSeconds(0.4f);


        // =========================
        // 플레이어 이동 해제
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }


        isRevealing = false;
    }


    public bool HasFoundSwitch()
    {
        return hasFoundSwitch;
    }
}