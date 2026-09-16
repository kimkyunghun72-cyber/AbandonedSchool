using UnityEngine;
using UnityEngine.UI;

public class Classroom02PlayerPhotoEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("사진 UI")]
    [SerializeField] private GameObject photoPanel;
    [SerializeField] private Image photoImage;

    [Header("주인공 사진")]
    [SerializeField] private Sprite playerPhoto;

    private bool hasCheckedPhoto = false;


    public void Interact()
    {
        // =========================
        // 이미 조사한 경우
        // =========================
        if (hasCheckedPhoto)
        {
            dialogueManager.ShowNarrationSequence(
                "내 사진이다.",
                "왜 이런 곳에 내 사진이 있는 거지…"
            );

            return;
        }


        // =========================
        // 첫 조사
        // =========================
        hasCheckedPhoto = true;


        // 주인공 사진 확대
        photoImage.sprite = playerPhoto;
        photoPanel.SetActive(true);


        // 사진을 띄운 상태로 대사 진행
        dialogueManager.ShowNarrationSequence(
            "……",
            "이거… 나잖아.",
            "사진 옆에 글씨가 적혀 있다.",
            "「<color=#C00000>계속 함께야...?</color>」",
            "사진 가장자리가 심하게 닳아 있다.",
            "「<color=#C00000>누군가 계속 만져온 것처럼..</color>」"
        );
    }


    public bool HasCheckedPhoto()
    {
        return hasCheckedPhoto;
    }
}