using UnityEngine;
using UnityEngine.UI;

public class Classroom02GroupPhotoEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("사진 UI")]
    [SerializeField] private GameObject photoPanel;
    [SerializeField] private Image photoImage;

    [Header("단체사진")]
    [SerializeField] private Sprite groupPhoto;

    private bool hasCheckedPhoto = false;


    public void Interact()
    {
        // =========================
        // 이미 조사한 경우
        // =========================
        if (hasCheckedPhoto)
        {
            dialogueManager.ShowNarrationSequence(
                "오래된 단체사진이다.",
                "몇몇 얼굴은 알아볼 수 없게 긁혀 있다."
            );

            return;
        }


        // =========================
        // 첫 조사
        // =========================
        hasCheckedPhoto = true;


        // 단체사진 확대
        photoImage.sprite = groupPhoto;
        photoPanel.SetActive(true);


        dialogueManager.ShowNarrationSequence(
            "낡은 단체사진이다.",
            "이 학교 학생들과 교사가 함께 찍혀 있다.",
            "…잠깐.",
            "사진 속 몇몇 얼굴이 심하게 긁혀 있다.",
            "일부러 지운 것처럼 보이는데…"
        );
    }


    public bool HasCheckedPhoto()
    {
        return hasCheckedPhoto;
    }
}