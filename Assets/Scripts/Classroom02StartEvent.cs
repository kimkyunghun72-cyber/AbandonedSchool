using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Classroom02StartEvent : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Dialogue Manager")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("시작 대기 시간")]
    [SerializeField] private float startDelay = 0.5f;

    private bool hasStarted = false;


    void Start()
    {
        // Classroom02에서만 실행
        if (SceneManager.GetActiveScene().name != "Classroom02")
        {
            return;
        }

        StartCoroutine(StartEvent());
    }


    private IEnumerator StartEvent()
    {
        if (hasStarted)
            yield break;

        hasStarted = true;


        // 입장 직후 이동 잠금
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        yield return new WaitForSeconds(startDelay);


        // 입장 독백
        dialogueManager.ShowDialogueSequence(
            "렌",
            "1-3반...",
            "이상하네.",
            "처음 들어오는 곳인데... 왜 이렇게 익숙하지?"
        );
    }
}