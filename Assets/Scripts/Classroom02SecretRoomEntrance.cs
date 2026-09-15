using UnityEngine;

public class Classroom02SecretRoomEntrance : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("플레이어")]
    [SerializeField] private PlayerController playerController;

    private bool isUnlocked = false;
    private bool hasEntered = false;


    public void UnlockEntrance()
    {
        isUnlocked = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("비밀방 Trigger 진입 : " + other.gameObject.name);

        if (!isUnlocked)
        {
            Debug.Log("비밀방 입구가 아직 잠겨 있음");
            return;
        }

        if (hasEntered)
        {
            return;
        }


        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            return;
        }


        hasEntered = true;

        player.SetMovementLocked(true);


        dialogueManager.ShowDialogueSequence(
            "렌",
            "…여긴 뭐지?",
            "이런 공간이 있었나…"
        );
    }
}