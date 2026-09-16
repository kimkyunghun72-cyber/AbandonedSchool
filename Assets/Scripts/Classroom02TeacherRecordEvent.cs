using UnityEngine;

public class Classroom02TeacherRecordEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    private bool hasReadRecord = false;


    public void Interact()
    {
        // =========================
        // 처음 조사
        // =========================
        if (!hasReadRecord)
        {
            hasReadRecord = true;

            dialogueManager.ShowNarrationSequence(
                "책상 위에 낡은 서류철이 놓여 있다.",
                "표지에는 「생활지도 기록」이라고 적혀 있다.",
                "「수업 방해 학생은 수업 종료 후 별도 지도한다.」",
                "「교실 뒤편 시설은 학생에게 공개하지 않는다.」",
                "…별도 지도?",
                "이 방이 그 '별도 시설'이었던 건가…"
            );

            return;
        }


        // =========================
        // 다시 조사
        // =========================
        dialogueManager.ShowNarrationSequence(
            "오래된 생활지도 기록이다.",
            "「교실 뒤편 시설은 학생에게 공개하지 않는다.」"
        );
    }


    public bool HasReadRecord()
    {
        return hasReadRecord;
    }
}