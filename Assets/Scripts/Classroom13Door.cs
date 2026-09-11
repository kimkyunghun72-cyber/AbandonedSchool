using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Classroom13Door : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("1-3반 열쇠")]
    [SerializeField] private FireExtinguisherEvent fireExtinguisherEvent;

    [Header("이동할 씬")]
    [SerializeField] private string classroom13SceneName = "Classroom03";

    private bool isOpening = false;


    public void Interact()
    {
        if (dialogueManager == null)
            return;

        if (isOpening)
            return;


        // =========================
        // 열쇠가 없는 경우
        // =========================
        if (fireExtinguisherEvent == null ||
            !fireExtinguisherEvent.HasClassroom13Key)
        {
            dialogueManager.ShowNarrationSequence(
                "문이 잠겨 있다."
            );

            return;
        }


        // =========================
        // 열쇠가 있는 경우
        // =========================
        StartCoroutine(OpenDoor());
    }


    private IEnumerator OpenDoor()
    {
        isOpening = true;

        dialogueManager.ShowNarrationSequence(
            "철컥.",
            "문이 열렸다."
        );


        // 마지막 대사를 닫을 때까지 기다림
        yield return new WaitUntil(
            () => !dialogueManager.IsDialogueOpen()
        );


        // =========================
        // 1-3반 씬 이동
        // =========================
        SceneManager.LoadScene(classroom13SceneName);
    }
}