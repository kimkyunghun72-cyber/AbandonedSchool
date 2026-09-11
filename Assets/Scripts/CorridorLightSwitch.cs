using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CorridorLightSwitch : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("게시판 단서")]
    [SerializeField] private SecondBulletinBoard secondBulletinBoard;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Global Light")]
    [SerializeField] private Light2D globalLight;

    [Header("암전")]
    [SerializeField] private float darkIntensity = 0.02f;
    [SerializeField] private float darknessTime = 1f;

    [Header("붉은 조명")]
    [SerializeField] private float redIntensity = 0.55f;

    [SerializeField]
    private Color redColor = new Color(
        0.42f,
        0.06f,
        0.06f,
        1f
    );

    [Header("새로운 발자국")]
    [SerializeField] private GameObject newFootprints;

    [Header("연출")]
    [SerializeField] private float redLightWaitTime = 0.5f;

    [Header("붉은 복도 공포 오브젝트")]
    [SerializeField] private GameObject redHorrorObjects;

    private bool eventPlayed = false;
    private bool isEventPlaying = false;
    public bool EventFinished { get; private set; } = false;


    private void Start()
    {
        // 새 발자국은 처음에는 숨김
        if (newFootprints != null)
        {
            newFootprints.SetActive(false);
        }
        // 붉은 복도 실루엣/손자국 처음에는 숨김
        if (redHorrorObjects != null)
        {
            redHorrorObjects.SetActive(false);
        }
    }


    public void Interact()
    {
        if (isEventPlaying)
            return;


        // =========================
        // 게시판 단서를 아직 못 봄
        // =========================
        if (secondBulletinBoard == null ||
            !secondBulletinBoard.LightOffClueFound)
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    "낡은 전등 스위치다."
                );
            }

            return;
        }


        // =========================
        // 이벤트 이미 완료
        // =========================
        if (eventPlayed)
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    "스위치는 더 이상 반응하지 않는다."
                );
            }

            return;
        }


        StartCoroutine(LightHorrorEvent());
    }


    private IEnumerator LightHorrorEvent()
    {
        isEventPlaying = true;
        eventPlayed = true;


        // =========================
        // 플레이어 정지
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }


        // =========================
        // 완전 암전
        // =========================
        if (globalLight != null)
        {
            globalLight.intensity = darkIntensity;
        }


        // 아무 일도 없는 정적
        yield return new WaitForSeconds(darknessTime);


        // =========================
        // 붉은 조명으로 변경
        // =========================
        if (globalLight != null)
        {
            globalLight.color = redColor;
            globalLight.intensity = redIntensity;
        }
        // =========================
        // 창문 실루엣 / 손자국 등장
        // =========================
        if (redHorrorObjects != null)
        {
            redHorrorObjects.SetActive(true);
        }


        // =========================
        // 새로운 발자국 등장
        // =========================
        if (newFootprints != null)
        {
            newFootprints.SetActive(true);
        }


        // 붉어진 복도를 잠깐 보여줌
        yield return new WaitForSeconds(redLightWaitTime);

        
        // 복도 공포 이벤트 완료
        EventFinished = true;

        // =========================
        // 다시 이동 가능
        // =========================
        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }


        isEventPlaying = false;
    }
}