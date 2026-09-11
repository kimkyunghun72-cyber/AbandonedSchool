using UnityEngine;

public class FireExtinguisherEvent : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("복도 조명 이벤트")]
    [SerializeField] private CorridorLightSwitch corridorLightSwitch;

    // 1-3반 열쇠 획득 여부
    public bool HasClassroom13Key { get; private set; } = false;


    public void Interact()
    {
        if (dialogueManager == null)
            return;


        // =========================
        // 열쇠를 이미 얻은 상태
        // =========================
        if (HasClassroom13Key)
        {
            dialogueManager.ShowDialogue(
                "낡은 소화기다."
            );

            return;
        }


        // =========================
        // 붉은 조명 이벤트 전
        // =========================
        if (corridorLightSwitch == null ||
            !corridorLightSwitch.EventFinished)
        {
            dialogueManager.ShowDialogue(
                "낡은 소화기다."
            );

            return;
        }


        // =========================
        // 붉은 조명 이벤트 이후
        // 열쇠 발견
        // =========================
        HasClassroom13Key = true;

        dialogueManager.ShowNarrationSequence(
            "소화기 뒤쪽에 뭔가 끼어 있다.",
            "작은 열쇠다.",
            "「1-3반 열쇠를 얻었다.」"
        );
    }
}