using UnityEngine;
using UnityEngine.UI;

public class Classroom02OtherGirlsPhotoEvent : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("사진 UI")]
    [SerializeField] private GameObject photoPanel;
    [SerializeField] private Image photoImage;

    [Header("여학생 사진")]
    [SerializeField] private Sprite girlsPhoto;

    private bool hasCheckedPhoto = false;


    public void Interact()
    {
        if (hasCheckedPhoto)
        {
            dialogueManager.ShowNarrationSequence(
                "이 방에 있었던 학생들의 사진인 것 같다."
            );

            return;
        }


        hasCheckedPhoto = true;


        // 사진 두 장이 합쳐진 Sprite 확대
        photoImage.sprite = girlsPhoto;
        photoPanel.SetActive(true);


        dialogueManager.ShowNarrationSequence(
            "벽에 오래된 사진 두 장이 붙어 있다.",
            "둘 다 이 학교 학생인 것 같다.",
            "사진 아래에 이름이 적혀 있었던 흔적이 있지만…",
            "심하게 긁혀서 알아볼 수 없다.",
            "…이 애들도 이 방에 있었던 건가?"
        );
    }


    public bool HasCheckedPhoto()
    {
        return hasCheckedPhoto;
    }
}