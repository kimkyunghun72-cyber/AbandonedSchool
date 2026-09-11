using UnityEngine;

public class Classroom13Door : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    public void Interact()
    {
        if (dialogueManager == null)
            return;

        dialogueManager.ShowNarrationSequence(
            "문이 잠겨 있다."    
        );
    }
}