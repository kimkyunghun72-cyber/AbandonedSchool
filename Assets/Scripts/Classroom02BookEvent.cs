using UnityEngine;

public class Classroom02BookEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    private bool hasReadBook = false;


    public void Interact()
    {
        // =========================
        // 처음 조사
        // =========================
        if (!hasReadBook)
        {
            dialogueManager.ShowDialogueSequence(
                "렌",
                "책상 위에 낡은 책 한 권이 놓여 있다.",
                "이상하다. 다른 곳은 먼지가 쌓여 있는데 이것만 비교적 깨끗하다.",
                "책 사이에 종이 한 장이 끼워져 있다.",
                "「선생님은 저걸 급훈이라고 불렀다.」",
                "「저 말은 우리를 위한 게 아니었다.」",
                "…급훈?"
            );

            hasReadBook = true;
            return;
        }


        // =========================
        // 다시 조사
        // =========================
        dialogueManager.ShowDialogue(
            "「선생님은 저걸 급훈이라고 불렀다.」" +
            "「저 말은 우리를 위한 게 아니었다.」"
        );
    }


    public bool HasReadBook()
    {
        return hasReadBook;
    }
}