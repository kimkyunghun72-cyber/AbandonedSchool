using UnityEngine;

public class LockedClassroomDoor : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("문 조사")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] messages =
    {
        "문이 열리지 않는다.",
        "문은 다시 잠겨 버렸다."
    };


    public void Interact()
    {
        if (dialogueManager == null)
        {
            return;
        }

        dialogueManager.ShowNarrationSequence(messages);
    }
}