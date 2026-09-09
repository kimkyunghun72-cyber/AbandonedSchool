using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private TMP_Text horrorSpeakerNameText;
    [Header("사진 UI")]
    [SerializeField] private GameObject photoPanel;
    [SerializeField] private Image photoImage;

    [Header("플레이어")]
    [SerializeField] private PlayerController playerController;

    [Header("텍스트 출력 속도")]
    [SerializeField] private float typingSpeed = 0.04f;


    private bool isDialogueOpen = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;
    private TMP_Text currentText;
    private bool isPhotoOpen = false;

    private Sprite pendingPhoto;
    private string photoFollowUpSpeaker;
    private string photoFollowUpMessage;

    private bool showPhotoAcquiredNext = false;


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
        photoPanel.SetActive(false);

        speakerNameText.gameObject.SetActive(false);
        // 공포 이름표 처음에는 숨김
        horrorSpeakerNameText.gameObject.SetActive(false);
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
    // 이름표 없는 여러 문장
    // =========================
    public void ShowNarrationSequence(params string[] messages)
    {
        dialogueMessages = messages;
        currentMessageIndex = 0;

        dialoguePanel.SetActive(true);
        horrorPanel.SetActive(false);

        // 조사/시스템 메시지는 이름표 숨김
        speakerNameText.gameObject.SetActive(false);

        currentText = dialogueText;

        StartDialogue( dialogueMessages[currentMessageIndex] );
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

        // 이름표 없는 공포 문장
        horrorSpeakerNameText.gameObject.SetActive(false);

        currentText = horrorText;

        StartDialogue(message);
    }
    // =========================
    // 공포 대화 - 이름표 있음
    // =========================
    public void ShowHorrorDialogue(string speaker, string message)
    {
        dialogueMessages = null;

        dialoguePanel.SetActive(false);
        horrorPanel.SetActive(true);

        horrorSpeakerNameText.text = speaker;
        horrorSpeakerNameText.gameObject.SetActive(true);

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
        // =========================
        // 사진을 보고 있는 상태
        // =========================
        if (isPhotoOpen)
        {
            photoPanel.SetActive(false);
            isPhotoOpen = false;

            string speaker = photoFollowUpSpeaker;
            string message = photoFollowUpMessage;

            photoFollowUpSpeaker = null;
            photoFollowUpMessage = null;

            showPhotoAcquiredNext = true;
            ShowDialogueSequence( speaker, message );

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
        // =========================
        // 예약된 사진이 있으면 사진 표시
        // =========================
        if (pendingPhoto != null)
        {
            ShowPendingPhoto();
            return;
        }
        if (showPhotoAcquiredNext)
        {
            showPhotoAcquiredNext = false;

            ShowDialogue("사진을 얻었다.");

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
        photoPanel.SetActive(false);

        // 이름표들 숨김
        speakerNameText.gameObject.SetActive(false);
        horrorSpeakerNameText.gameObject.SetActive(false);


        isDialogueOpen = false;
        isTyping = false;

        dialogueMessages = null;
        currentMessageIndex = 0;

        if (playerController != null)
        {
            playerController.SetMovementLocked(false);
        }
    }
    // =========================
    // 다음에 사진 보여주기 예약
    // =========================
    public void SetNextPhoto( Sprite photo, string followUpSpeaker, string followUpMessage)
    {
        pendingPhoto = photo;

        photoFollowUpSpeaker = followUpSpeaker;
        photoFollowUpMessage = followUpMessage;
    }


    // =========================
    // 사진 표시
    // =========================
    private void ShowPendingPhoto()
    {
        if (pendingPhoto == null)
        {
            return;
        }

        dialoguePanel.SetActive(false);
        horrorPanel.SetActive(false);

        photoImage.sprite = pendingPhoto;
        photoPanel.SetActive(true);

        isPhotoOpen = true;
        isDialogueOpen = true;

        // 사진을 띄웠으므로 예약된 사진은 제거
        pendingPhoto = null;
    }


    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}