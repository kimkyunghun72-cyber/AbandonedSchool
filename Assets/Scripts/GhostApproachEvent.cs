using System.Collections;
using UnityEngine;

public class GhostApproachEvent : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Ghost")]
    [SerializeField] private GameObject ghostGirl;
    [SerializeField] private SpriteRenderer ghostRenderer;

    [Header("Ghost 사라지는 시간")]
    [SerializeField] private float fadeDuration = 1.5f;

    private bool eventStarted = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (eventStarted)
            return;

        if (!collision.CompareTag("Player"))
            return;

        eventStarted = true;

        StartCoroutine(GhostSequence());
    }


    private IEnumerator GhostSequence()
    {
        // =========================
        // 플레이어 즉시 정지
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        // 약간 정적
        yield return new WaitForSeconds(0.5f);


        // =========================
        // GhostGirl 대사
        // =========================
        if (dialogueManager != null)
        {
            dialogueManager.ShowHorrorText(
                "<color=#B91515>…보고 싶었어.</color>"
            );
        }


        // =========================
        // 플레이어가 E를 눌러
        // 대사를 닫을 때까지 기다림
        // =========================
        yield return new WaitUntil( () => !dialogueManager.IsDialogueOpen()
        );


        // DialogueManager의 HideDialogue에서
        // 잠깐 이동 잠금이 풀리므로 다시 잠금
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        // =========================
        // GhostGirl 서서히 사라짐
        // =========================
        yield return StartCoroutine(FadeOutGhost());


        // GhostGirl 완전히 제거
        if (ghostGirl != null)
        {
            ghostGirl.SetActive(false);
        }


        // =========================
        // 다시 이동 가능
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }


        // Trigger도 더 이상 필요 없음
        gameObject.SetActive(false);
    }


    private IEnumerator FadeOutGhost()
    {
        if (ghostRenderer == null)
            yield break;

        Color startColor = ghostRenderer.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp( 1f, 0f, time / fadeDuration );

            Color newColor = startColor;
            newColor.a = alpha;

            ghostRenderer.color = newColor;

            yield return null;
        }

        Color finalColor = startColor;
        finalColor.a = 0f;

        ghostRenderer.color = finalColor;
    }
}