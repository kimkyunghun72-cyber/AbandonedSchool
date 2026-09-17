using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class DemoEnding : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Light2D globalLight;

    [Header("Ghost")]
    [SerializeField] private GameObject ghostGirl;

    [Header("암전")]
    [SerializeField] private CanvasGroup blackoutPanel;

    [Header("DEMO END")]
    [SerializeField] private GameObject demoEndPanel;
    [SerializeField] private TMP_Text demoEndText;

    [Header("조명")]
    [SerializeField] private float normalIntensity = 1f;
    [SerializeField] private float darkIntensity = 0.05f;
    [SerializeField] private float flickerInterval = 0.12f;
    [SerializeField] private int flickerCount = 6;

    private bool endingStarted = false;


    private void Start()
    {
        if (ghostGirl != null)
            ghostGirl.SetActive(false);

        if (blackoutPanel != null)
        {
            blackoutPanel.alpha = 0f;
            blackoutPanel.blocksRaycasts = false;
        }

        if (demoEndPanel != null)
            demoEndPanel.SetActive(false);
    }


    // 마지막 메모를 다 보고 닫았을 때 호출
    public void StartDemoEnding()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        StartCoroutine(EndingSequence());
    }


    private IEnumerator EndingSequence()
    {
        // =========================
        // 이동 정지
        // =========================

        if (playerController != null)
            playerController.SetMovementLocked(true);


        // 잠깐 정적
        yield return new WaitForSeconds(0.8f);


        // =========================
        // 조명 깜빡임
        // =========================
        yield return StartCoroutine(FlickerLight());


        // =========================
        // 깜빡임 끝난 뒤 어두운 상태로 고정
        // =========================
        if (globalLight != null)
        {
            globalLight.intensity = darkIntensity;
        }

        yield return new WaitForSeconds(0.7f);


        // =========================
        // 어두운 상태에서 GhostGirl 등장
        // =========================
        if (ghostGirl != null)
        {
            ghostGirl.SetActive(true);
        }

        yield return new WaitForSeconds(1f);


        // =========================
        // 마지막 공포 대사
        // =========================
        if (dialogueManager != null)
        {
            dialogueManager.ShowHorrorDialogue(
                "???",
                "이제 조금 기억났어?"
            );

            yield return new WaitForSeconds(2.5f);
        }


        // =========================
        // 암전
        // =========================

        yield return StartCoroutine(FadeToBlack());


        // 귀신 숨기기
        if (ghostGirl != null)
            ghostGirl.SetActive(false);


        yield return new WaitForSeconds(0.7f);


        // =========================
        // 마지막 문장
        // =========================

        if (demoEndPanel != null)
            demoEndPanel.SetActive(true);

        if (demoEndText != null)
        {
            demoEndText.text =
                
                "DEMO END" ;
        }
    }


    private IEnumerator FlickerLight()
    {
        if (globalLight == null)
            yield break;

        for (int i = 0; i < flickerCount; i++)
        {
            globalLight.intensity = darkIntensity;
            yield return new WaitForSeconds(flickerInterval);

            globalLight.intensity = normalIntensity;
            yield return new WaitForSeconds(flickerInterval);
        }
    }


    private IEnumerator FadeToBlack()
    {
        if (blackoutPanel == null)
            yield break;

        blackoutPanel.blocksRaycasts = true;

        float time = 0f;
        float duration = 1f;

        while (time < duration)
        {
            time += Time.deltaTime;

            blackoutPanel.alpha =
                Mathf.Lerp(0f, 1f, time / duration);

            yield return null;
        }

        blackoutPanel.alpha = 1f;
    }
}