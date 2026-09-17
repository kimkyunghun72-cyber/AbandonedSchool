using UnityEngine;

public class Classroom02FinalMemoEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;
    [Header("플레이어")]
    [SerializeField] private PlayerController playerController;

    [Header("데모 엔딩")]
    [SerializeField] private DemoEnding demoEnding;


    private bool hasReadMemo = false;


    public void Interact()
    {
        Debug.Log("마지막 메모 조사 실행");

        if (hasReadMemo)
        {
            dialogueManager.ShowNarrationSequence(
                "여자아이의 글씨가 적힌 오래된 메모다.",
                "마지막에는 내 이름이 적혀 있다."
            );

            return;
        }


        hasReadMemo = true;
        // =========================
        // 마지막 메모 이후 이동 완전 금지
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        // =========================
        // 메모가 전부 끝난 뒤
        // 렌의 마지막 독백 예약
        // =========================
        dialogueManager.SetNextDialogue(
            "렌",
            "……\n내 이름…?"
        );


        // =========================
        // 모든 대화가 완전히 끝난 뒤
        // 데모 엔딩 시작
        // =========================
        dialogueManager.SetDialogueEndAction(() =>
        {
            if (demoEnding != null)
            {
                demoEnding.StartDemoEnding();
            }
        });


        // =========================
        // 마지막 메모
        // =========================
        dialogueManager.ShowNarrationSequence(
            "바닥에 낡은 종이 한 장이 떨어져 있다.",
            "삐뚤어진 글씨로 무언가 적혀 있다.",
            "「오늘도 여기다.」",
            "「선생님은 내가 없는 일을 지어낸다고 했다.」",
            "「문이 닫히자 복도 소리도 들리지 않았다.」",
            "「아무도 내가 여기 있다는 걸 모를 거다.」",
            "「그래도 괜찮다.」",
            "「렌은 알고 있으니까.」",
            "「금방 돌아오겠다고 했다.」",
            "「약속했으니까… 조금만 더 기다리면 된다.」"
        );
    }


    public bool HasReadMemo()
    {
        return hasReadMemo;
    }
}