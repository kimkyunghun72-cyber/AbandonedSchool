using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("일반 대화 UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

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

    // 현재 출력 중인 텍스트
    private TMP_Text currentText;


    void Start()
    {
        dialoguePanel.SetActive(false);
        horrorPanel.SetActive(false);
    }


    // =========================
    // 일반 조사 대화
    // =========================
    public void ShowDialogue(string message)
    {
        dialoguePanel.SetActive(true);
        horrorPanel.SetActive(false);

        currentText = dialogueText;

        StartDialogue(message);
    }


    // =========================
    // 공포 텍스트
    // =========================
    public void ShowHorrorText(string message)
    {
        dialoguePanel.SetActive(false);
        horrorPanel.SetActive(true);

        currentText = horrorText;

        StartDialogue(message);
    }


    // =========================
    // 대화 시작 공통 처리
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

        typingCoroutine = StartCoroutine(
            TypeText(message)
        );
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


    // =========================
    // E키
    // =========================
    public void ContinueDialogue()
    {
        // 아직 출력 중
        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        // 출력 완료 후 E → 닫기
        HideDialogue();
    }


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