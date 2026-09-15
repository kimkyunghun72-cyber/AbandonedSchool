using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Classroom02SecretSwitch : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private PlayerController playerController;

    [Header("교실 조명")]
    [SerializeField] private Light2D globalLight;

    [Header("비밀방 입구 조명")]
    [SerializeField] private Light2D secretEntranceLight;

    [Header("비밀방 입구")]
    [SerializeField] private Classroom02SecretRoomEntrance secretRoomEntrance;

    [Header("효과음")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip switchSound;

    [Header("조명 설정")]
    [SerializeField] private float darkIntensity = 0.05f;
    [SerializeField] private float entranceLightIntensity = 1.5f;
    [SerializeField] private float fadeDuration = 0.3f;

    private bool hasPressed = false;
    private bool isRunning = false;


    private void Start()
    {
        // 처음에는 비밀방 입구 조명 OFF
        if (secretEntranceLight != null)
        {
            secretEntranceLight.intensity = 0f;
        }
    }


    public void Interact()
    {
        if (hasPressed || isRunning)
        {
            return;
        }

        StartCoroutine(SwitchRoutine());
    }


    private IEnumerator SwitchRoutine()
    {
        isRunning = true;
        hasPressed = true;


        // =========================
        // 플레이어 이동 잠금
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        // =========================
        // 철컥 효과음
        // =========================
        if (audioSource != null && switchSound != null)
        {
            audioSource.PlayOneShot(switchSound);
        }


        yield return new WaitForSeconds(0.15f);


        // =========================
        // 조명 전환
        // =========================
        float startGlobalIntensity =
            globalLight != null ? globalLight.intensity : 0f;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / fadeDuration;


            // 교실 전체 어둡게
            if (globalLight != null)
            {
                globalLight.intensity =
                    Mathf.Lerp(
                        startGlobalIntensity,
                        darkIntensity,
                        t
                    );
            }


            // 비밀방 입구만 밝게
            if (secretEntranceLight != null)
            {
                secretEntranceLight.intensity =
                    Mathf.Lerp(
                        0f,
                        entranceLightIntensity,
                        t
                    );
            }


            yield return null;
        }


        // 정확한 최종 값
        if (globalLight != null)
        {
            globalLight.intensity = darkIntensity;
        }

        if (secretEntranceLight != null)
        {
            secretEntranceLight.intensity = entranceLightIntensity;
        }


        // =========================
        // 비밀방 입구 활성화
        // =========================
        if (secretRoomEntrance != null)
        {
            secretRoomEntrance.UnlockEntrance();
        }


        yield return new WaitForSeconds(0.5f);


        // =========================
        // 플레이어 이동 해제
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }


        isRunning = false;
    }
}