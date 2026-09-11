using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CorridorGhostEvent : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Ghost")]
    [SerializeField] private GameObject ghostGirl;

    [Header("Global Light")]
    [SerializeField] private Light2D globalLight;

    [Header("밝기")]
    [SerializeField] private float normalIntensity = 1f;
    [SerializeField] private float darkIntensity = 0.1f;

    [Header("깜빡임")]
    [SerializeField] private float flickerTime = 0.12f;

    private bool eventStarted = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (eventStarted)
            return;

        if (!collision.CompareTag("Player"))
            return;

        eventStarted = true;

        // 이벤트 시작과 동시에 이동 금지
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }

        StartCoroutine(GhostEvent());
    }


    private IEnumerator GhostEvent()
    {
        // 잠깐 정적
        yield return new WaitForSeconds(0.5f);


        // ========================
        // 첫 번째 깜빡임
        // ========================

        globalLight.intensity = darkIntensity;

        yield return new WaitForSeconds(flickerTime);

        globalLight.intensity = normalIntensity;

        yield return new WaitForSeconds(0.15f);


        // ========================
        // 두 번째 깜빡임
        // ========================

        globalLight.intensity = darkIntensity;

        yield return new WaitForSeconds(flickerTime);


        // 어두운 순간 GhostGirl 등장
        if (ghostGirl != null)
        {
            ghostGirl.SetActive(true);
        }


        // 불 다시 켜기
        globalLight.intensity = normalIntensity;


        // ========================
        // 귀신을 1초 동안 보여줌
        // ========================

        yield return new WaitForSeconds(1f);


        // 플레이어 다시 이동 가능
        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }
    }
}