using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassroomDoorEvent : MonoBehaviour
{
    [Header("필수 상태")]
    public bool hasKey = false;   // 테스트용. 나중엔 인벤토리 값이랑 연결
    private bool firstEventPlayed = false;
    private bool doorReadyToOpen = false;
    private bool isEventRunning = false;

    private SpriteRenderer ghostRenderer;
    public void GetKey()
    {
        hasKey = true;

        Debug.Log("교실 열쇠 획득");
    }
    void Start()
    {
        if (ghostGirl != null)
        {
            ghostRenderer = ghostGirl.GetComponentInChildren<SpriteRenderer>(true);
        }
    }


    [Header("연출 오브젝트")]
    [SerializeField] private GameObject blackoutPanel;
    [SerializeField] private GameObject blackboardWarning;
    [SerializeField] private GameObject ghostGirl;

    [Header("플레이어")]
    [SerializeField] private MonoBehaviour playerController;
    // 네 플레이어 조작 스크립트 넣기
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("씬 이동")]
    [SerializeField] private string corridorSceneName = "CorridorScene";
    [Header("교실 조명")]
    [SerializeField] private LightSwitch lightSwitch;

    private bool lockedDialoguePlayed = false;

    public void Interact()
    {
        if (isEventRunning)
            return;

        // 열쇠가 없으면
        if (!hasKey)
        {
            if (!lockedDialoguePlayed)
            {
                lockedDialoguePlayed = true;

                dialogueManager.SetNextDialogue(
                    "렌",
                    "일단 여길 나갈 방법을 찾아야겠어..."
                );
            }
            dialogueManager.ShowHorrorText("문이 잠겨 있다.");
            return;
        }

        // 첫 번째 이벤트 아직 안 봤으면
        if (!firstEventPlayed)
        {
            StartCoroutine(PlayDoorEvent());
            return;
        }

        // 이벤트 끝났고 문 열 수 있으면
        if (doorReadyToOpen)
        {
            StartCoroutine(OpenDoorAndMoveScene());
        }
    }

    IEnumerator PlayDoorEvent()
    {
        isEventRunning = true;
        LockPlayer(true);

        yield return StartCoroutine(ShowOnlyMessage("열쇠를 꽂았다."));

        yield return new WaitForSeconds(0.2f);

        // 정전 / 점멸 연출
        yield return StartCoroutine(BlackoutFlicker());

        

        // 깜빡임이 끝난 뒤 교실을 불 꺼진 밝기로 고정
        if (lightSwitch != null)
        {
            lightSwitch.ForceLightOffForDoorEvent();
        }

        // 칠판 경고문 표시
        if (blackboardWarning != null)
            blackboardWarning.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        // 여자아이 등장
        if (ghostGirl != null)
        {
            ghostGirl.SetActive(true);
            yield return StartCoroutine(FadeGhost(0f, 0.8f, 0.2f));
        }

        yield return StartCoroutine(
            ShowGhostMessage("또 날 버리는 거야?")
        );

        if (ghostGirl != null)
        {
            yield return StartCoroutine(FadeGhost(0.8f, 0f, 0.2f));
            ghostGirl.SetActive(false);
        }

        firstEventPlayed = true;
        doorReadyToOpen = true;
        isEventRunning = false;

        LockPlayer(false);
    }

    IEnumerator OpenDoorAndMoveScene()
    {
        isEventRunning = true;
        LockPlayer(true);

        yield return StartCoroutine(ShowOnlyMessage("문이 열렸다."));

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(corridorSceneName);
    }

    IEnumerator BlackoutFlicker()
    {
        if (blackoutPanel == null)
            yield break;

        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(0.25f);

        blackoutPanel.SetActive(false);
        yield return new WaitForSeconds(0.15f);

        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(0.12f);

        blackoutPanel.SetActive(false);
        yield return new WaitForSeconds(0.18f);

        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(0.08f);

        blackoutPanel.SetActive(false);
    }

    IEnumerator ShowOnlyMessage(string message)
    {
        if (dialogueManager == null)
            yield break;

        dialogueManager.ShowDialogue(message);

        // 플레이어가 E를 눌러 대화창을 닫을 때까지 기다림
        yield return new WaitUntil(() => !dialogueManager.IsDialogueOpen());
    }
    IEnumerator ShowGhostMessage(string message)
    {
        if (dialogueManager == null)
            yield break;

        dialogueManager.ShowHorrorDialogue( "???", message );

        yield return new WaitUntil(() => !dialogueManager.IsDialogueOpen() );
    }

    void LockPlayer(bool value)
    {
        if (playerController != null)
            playerController.enabled = !value;
    }
    IEnumerator FadeGhost(float startAlpha, float endAlpha, float duration)
    {
        if (ghostRenderer == null)
            yield break;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            Color color = ghostRenderer.color;
            color.a = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            ghostRenderer.color = color;

            yield return null;
        }

        Color finalColor = ghostRenderer.color;
        finalColor.a = endAlpha;
        ghostRenderer.color = finalColor;
    }

}