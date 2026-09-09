using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    [Header("교실 조명")]
    [SerializeField] private Light2D globalLight;

    [Header("밝기")]
    [SerializeField] private float lightOnIntensity = 1f;
    [SerializeField] private float lightOffIntensity = 0.1f;


    [Header("칠판 힌트")]
    [SerializeField] private GameObject blackboardHint;


    [Header("첫 공포 연출")]
    [SerializeField] private GameObject ghostGirl;
    [SerializeField] private PlayerController playerController;


    [Header("조명 깜빡임")]
    [SerializeField] private float flickerInterval = 0.12f;


    [Header("귀신 페이드")]
    [SerializeField] private float ghostMaxAlpha = 0.4f;
    [SerializeField] private float ghostFadeInTime = 0.25f;
    [SerializeField] private float ghostStayTime = 0.7f;
    [SerializeField] private float ghostFadeOutTime = 0.35f;


    // 귀신 SpriteRenderer
    private SpriteRenderer ghostRenderer;


    // 현재 조명이 켜져 있는지
    private bool isLightOn = true;

    // 첫 공포 연출은 한 번만 실행
    private bool firstHorrorEventPlayed = false;

    // 연출 중 스위치 재입력 방지
    private bool isEventPlaying = false;



    void Start()
    {
        // =========================
        // 칠판 힌트 초기화
        // =========================
        if (blackboardHint != null)
        {
            blackboardHint.SetActive(false);
        }


        // =========================
        // 귀신 초기화
        // =========================
        if (ghostGirl != null)
        {
            // 비활성화된 자식도 포함해서 SpriteRenderer 찾기
            ghostRenderer = ghostGirl.GetComponentInChildren<SpriteRenderer>(true);

            // 처음에는 완전히 투명
            SetGhostAlpha(0f);

            ghostGirl.SetActive(false);
        }
    }



    // =========================
    // 스위치 상호작용
    // =========================
    public void Interact()
    {
        // 공포 연출 중에는 스위치 입력 무시
        if (isEventPlaying)
        {
            return;
        }


        // =========================
        // 불 끄기
        // =========================
        if (isLightOn)
        {
            isLightOn = false;


            // 처음 불을 끄는 경우
            if (!firstHorrorEventPlayed)
            {
                StartCoroutine(FirstHorrorEvent());
            }

            // 두 번째부터는 그냥 불만 끔
            else
            {
                TurnLightOff();
            }
        }


        // =========================
        // 불 켜기
        // =========================
        else
        {
            isLightOn = true;

            TurnLightOn();
        }
    }



    // =========================
    // 불 켜기
    // =========================
    private void TurnLightOn()
    {
        if (globalLight != null)
        {
            globalLight.intensity = lightOnIntensity;
        }


        // 불을 켜면 칠판 글씨 숨김
        if (blackboardHint != null)
        {
            blackboardHint.SetActive(false);
        }
    }



    // =========================
    // 불 끄기
    // =========================
    private void TurnLightOff()
    {
        if (globalLight != null)
        {
            globalLight.intensity = lightOffIntensity;
        }


        // 불을 끄면 칠판 글씨 표시
        if (blackboardHint != null)
        {
            blackboardHint.SetActive(true);
        }
    }



    // =========================
    // 첫 번째 공포 이벤트
    // =========================
    private IEnumerator FirstHorrorEvent()
    {
        isEventPlaying = true;
        firstHorrorEventPlayed = true;


        // =========================
        // 플레이어 이동 막기
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        // =========================
        // 처음에는 어둡게
        // =========================
        if (globalLight != null)
        {
            globalLight.intensity = lightOffIntensity;
        }


        if (blackboardHint != null)
        {
            blackboardHint.SetActive(false);
        }


        if (ghostGirl != null)
        {
            SetGhostAlpha(0f);
            ghostGirl.SetActive(false);
        }


        // 잠깐 정적
        yield return new WaitForSeconds(0.25f);



        // =========================
        // 조명 깜빡임 1
        // =========================
        if (globalLight != null)
        {
            globalLight.intensity = lightOnIntensity;
        }

        yield return new WaitForSeconds(flickerInterval);


        if (globalLight != null)
        {
            globalLight.intensity = lightOffIntensity;
        }

        yield return new WaitForSeconds(flickerInterval);



        // =========================
        // 조명 깜빡임 2
        // =========================
        if (globalLight != null)
        {
            globalLight.intensity = lightOnIntensity;
        }

        yield return new WaitForSeconds(flickerInterval);


        if (globalLight != null)
        {
            globalLight.intensity = lightOffIntensity;
        }



        // =========================
        // 칠판 힌트 등장
        // =========================
        if (blackboardHint != null)
        {
            blackboardHint.SetActive(true);
        }



        // =========================
        // 귀신 페이드 등장
        // =========================
        if (ghostGirl != null)
        {
            ghostGirl.SetActive(true);

            SetGhostAlpha(0f);


            // 서서히 등장
            if (ghostRenderer != null)
            {
                yield return StartCoroutine(
                    FadeGhost( 0f, ghostMaxAlpha, ghostFadeInTime )
                );


                // 잠깐 유지
                yield return new WaitForSeconds( ghostStayTime );


                // 서서히 사라짐
                yield return StartCoroutine(
                    FadeGhost( ghostMaxAlpha, 0f, ghostFadeOutTime ) );
            }
            else
            {
                // SpriteRenderer를 못 찾았을 경우
                // 최소한 일정 시간 후 사라지게 처리
                yield return new WaitForSeconds( ghostStayTime );
            }


            ghostGirl.SetActive(false);
        }



        // =========================
        // 플레이어 이동 다시 허용
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }


        isEventPlaying = false;
    }



    // =========================
    // 귀신 페이드
    // =========================
    private IEnumerator FadeGhost( float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;


        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            float alpha = Mathf.Lerp( startAlpha, endAlpha, t );


            SetGhostAlpha(alpha);

            yield return null;
        }


        // 마지막 값 정확하게 적용
        SetGhostAlpha(endAlpha);
    }



    // =========================
    // 귀신 Alpha 조절
    // =========================
    private void SetGhostAlpha(float alpha)
    {
        if (ghostRenderer == null)
        {
            return;
        }


        Color color = ghostRenderer.color;

        color.a = alpha;

        ghostRenderer.color = color;
    }
    // =========================
    // 문 이벤트용 강제 소등
    // =========================
    public void ForceLightOffForDoorEvent()
    {
        isLightOn = false;

        if (globalLight != null)
        {
            globalLight.intensity = lightOffIntensity;
        }

        // 기존 칠판 힌트는 숨김
        if (blackboardHint != null)
        {
            blackboardHint.SetActive(false);
        }
    }
    public bool IsLightOn()
    {
        return isLightOn;
    }
}