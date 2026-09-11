using UnityEngine;

public class SecondBulletinBoard : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Ghost Event")]
    [SerializeField] private GhostApproachEvent ghostApproachEvent;

    public bool LightOffClueFound { get; private set; } = false;


    public void Interact()
    {
        if (dialogueManager == null)
            return;


        // =========================
        // GhostGirl 이벤트 전
        // =========================
        if (ghostApproachEvent == null || !ghostApproachEvent.EventFinished)
        {
            dialogueManager.ShowDialogue(
                "낡은 공지들이 붙어 있다."
            );

            return;
        }

        LightOffClueFound = true;
        // =========================
        // GhostGirl 이벤트 이후
        // =========================
        dialogueManager.ShowNarrationSequence(
            "낡은 공지들이 붙어 있다.",
            "그런데 종이 한 장 뒤에 뭔가 적혀 있다.",
            "<color=#B91515>…불을 꺼.</color>"
        );
    }
}