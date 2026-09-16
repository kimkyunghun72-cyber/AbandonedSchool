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

    private bool hasPressed = false;

    private float originalLightIntensity;


    private void Start()
    {
        // 처음 교실 밝기 저장
        if (globalLight != null)
        {
            originalLightIntensity = globalLight.intensity;
        }

        // 비밀방 입구 조명은 처음엔 꺼둠
        if (secretEntranceLight != null)
        {
            secretEntranceLight.intensity = 0f;
        }
    }


    public void Interact()
    {
        // 한 번만 작동
        if (hasPressed)
        {
            return;
        }

        hasPressed = true;

        StartCoroutine(SwitchRoutine());
    }


    private IEnumerator SwitchRoutine()
    {
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
        // 교실 전체 어둡게
        // =========================
        if (globalLight != null)
        {
            globalLight.intensity = darkIntensity;
        }


        // =========================
        // 비밀방 입구만 밝게
        // =========================
        if (secretEntranceLight != null)
        {
            secretEntranceLight.intensity =
                entranceLightIntensity;
        }


        // =========================
        // 비밀방 입구 사용 가능
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
    }


    // =========================
    // 비밀방 진입 후 밝기 복구
    // =========================
    public void RestoreLight()
    {
        if (globalLight != null)
        {
            globalLight.intensity = originalLightIntensity;
        }

        if (secretEntranceLight != null)
        {
            secretEntranceLight.intensity = 0f;
        }
    }
}