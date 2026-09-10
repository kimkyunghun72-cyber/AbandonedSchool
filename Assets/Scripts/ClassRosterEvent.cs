using UnityEngine;

public class ClassRosterEvent : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("명단 이미지")]
    [SerializeField] private Sprite normalRoster;
    [SerializeField] private Sprite corruptedRoster;
    private bool hasInteracted = false;


    public void Interact()
    {
        if (dialogueManager == null || hasInteracted)
        {
            return;
        }

        hasInteracted = true;

        dialogueManager.SetNextRoster(
            normalRoster,
            "명단 한쪽이 심하게 긁혀 있다.",
            "사진 속 여자아이의 이름만 알아볼 수 없다."
        );

        dialogueManager.SetRosterHorror(
            corruptedRoster,
            "날 잊어버린거야?"
        );

        dialogueManager.ShowDialogueSequence(
            "렌",
            "...이 이름."
        );
    }
}