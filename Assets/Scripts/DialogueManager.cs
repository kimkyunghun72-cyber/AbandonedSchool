using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("일반 대화 UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;

    [Header("공포 텍스트")]
    [SerializeField] private GameObject horrorPanel;
    [SerializeField] private TMP_Text horrorText;

    [Header("플레이어")]
    [SerializeField] private PlayerController playerController;

    [Header("텍스트 출력 속도")]
    [SerializeField] private float typingSpeed = 0.04f;


    private bool isDialogueOpen = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;
    private TMP_Text currentText;


    // =========================
    // 여러 문장 대화
    // =========================
    private string[] dialogueMessages;
    private int currentMessageIndex = 0;

    // 다음에 이어서 보여줄 독백
    private bool hasNextDialogue = false;
    private string nextSpeaker;
    private string nextMessage;


    void Start()
    {
        dialoguePanel.SetActive(false);
        horrorPanel.SetActive(false);

        speakerNameText.gameObject.SetActive(false);
    }


    // =========================
    // 일반 조사 - 한 문장
    // =========================
    public void ShowDialogue(string message)
    {
        dialogueMessages = new string[] { message };
        currentMessageIndex = 0;

        dialoguePanel.SetActive(true);
        horrorPanel.SetActive(false);

        // 조사할 때는 이름표 숨김
        speakerNameText.gameObject.SetActive(false);

        currentText = dialogueText;

        StartDialogue(message);
    }


    // =========================
    // 여러 문장 대화
    // =========================
    public void ShowDialogueSequence( string speakerName, params string[] messages)
    {
        dialogueMessages = messages;
        currentMessageIndex = 0;

        dialoguePanel.SetActive(true);
        horrorPanel.SetActive(false);

        // 이름 표시
        speakerNameText.text = speakerName;
        speakerNameText.gameObject.SetActive(true);

        currentText = dialogueText;

        StartDialogue( dialogueMessages[currentMessageIndex] );
    }


    // =========================
    // 공포 텍스트
    // =========================
    public void ShowHorrorText(string message)
    {
        dialogueMessages = null;

        dialoguePanel.SetActive(false);
        horrorPanel.SetActive(true);

        currentText = horrorText;

        StartDialogue(message);
    }


    // =========================
    // 대화 시작
    // =========================
    private void StartDialogue(string message)
    {
        isDialogueOpen = true;

        if (playerController != null)
        {
            playerController.SetMovementLocked(true);
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(message));
    }


    // =========================
    // 글자 하나씩 출력
    // =========================
    private IEnumerator TypeText(string message)
    {
        isTyping = true;

        currentText.text = message;
        currentText.maxVisibleCharacters = 0;

        currentText.ForceMeshUpdate();

        int totalCharacters = currentText.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            currentText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }
    public void SetNextDialogue(string speaker, string message)
    {
        hasNextDialogue = true;
        nextSpeaker = speaker;
        nextMessage = message;
    }


    // =========================
    // E키
    // =========================
    public void ContinueDialogue()
    {
        // 글자가 아직 출력 중이면 전부 표시
        if (isTyping)
        {
            CompleteTyping();
            return;
        }


        // 예약된 다음 독백이 있으면 출력
        if (hasNextDialogue)
        {
            hasNextDialogue = false;

            string speaker = nextSpeaker;
            string message = nextMessage;

            nextSpeaker = null;
            nextMessage = null;

            ShowDialogueSequence( speaker, message );

            return;
        }


        // 다음 문장이 있으면 다음 문장 출력
        if (dialogueMessages != null &&
            currentMessageIndex < dialogueMessages.Length - 1)
        {
            currentMessageIndex++;

            StartDialogue( dialogueMessages[currentMessageIndex] );

            return;
        }


        HideDialogue();
    }


    // =========================
    // 타이핑 즉시 완료
    // =========================
    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (currentText != null)
        {
            currentText.maxVisibleCharacters = int.MaxValue;
        }

        isTyping = false;
    }


    // =========================
    // 대화 종료
    // =========================
    public void HideDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialoguePanel.SetActive(false);
        horrorPanel.SetActive(false);

        isDialogueOpen = false;
        isTyping = false;

        dialogueMessages = null;
        currentMessageIndex = 0;

        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }
    }


    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}