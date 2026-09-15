using UnityEngine;

public class Classroom02MottoEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("책 이벤트")]
    [SerializeField] private Classroom02BookEvent bookEvent;

    private bool foundClue = false;


    public void Interact()
    {
        // =========================
        // 책을 아직 읽지 않았을 때
        // =========================
        if (bookEvent == null || !bookEvent.HasReadBook())
        {
            dialogueManager.ShowNarrationSequence(
                "낡은 급훈이 걸려 있다.",
                "글씨가 바래서 군데군데 잘 보이지 않는다."
            );

            return;
        }


        // =========================
        // 책을 읽은 뒤 첫 조사
        // =========================
        if (!foundClue)
        {
            dialogueManager.ShowDialogueSequence(
                "렌",
                "…이 종이를 말한 건가.",
                "자세히 보니 한쪽이 벽에서 조금 떨어져 있다.",
                "뒤쪽에 무언가 적혀 있다.",
                "「선생님은 항상 앞만 보라고 했다.」",
                "「그래서 아무도 그 뒤를 보지 않았다.」",
                "「<color=#C00000>나를 제외하고..</color>」",
                "..뒤?"
            );

            foundClue = true;
            return;
        }


        // =========================
        // 단서를 찾은 뒤 재조사
        // =========================
        dialogueManager.ShowNarrationSequence(
            "「선생님은 항상 앞만 보라고 했다.」",
            "「그래서 아무도 그 뒤를 보지 않았다.」"
        );
    }


    public bool HasFoundClue()
    {
        return foundClue;
    }
}