using System.Collections;
using UnityEngine;

public class StartFade : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;


    private IEnumerator Start()
    {
        // =========================
        // 시작 시 플레이어 이동 금지
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }

        // 화면 완전 검정
        fadePanel.alpha = 1f;
        fadePanel.gameObject.SetActive(true);

        // 한 프레임 기다리기
        yield return null;


        // =========================
        // Fade In
        // =========================
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float t = time / fadeDuration;

            // 1 → 0
            fadePanel.alpha = 1f - t;

            yield return null;
        }


        // 완전히 투명하게
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);


        // Fade가 끝난 뒤 잠깐 정적
        yield return new WaitForSeconds(0.4f);


        // =========================
        // 시작 독백
        // =========================
        if (dialogueManager != null)
        {
            dialogueManager.ShowDialogueSequence(
                "렌",
                "어떻게 된 거지...?",
                "난 분명..."
            );
        }
    }
}