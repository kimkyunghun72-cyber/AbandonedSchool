using UnityEngine;

public class PuzzleDesk : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private LightSwitch lightSwitch;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private ClassroomDoorEvent classroomDoor;


    [Header("불이 켜져 있을 때")]
    [TextArea(2, 5)]
    [SerializeField]
    private string lightOnMessage = "오래된 책상이다.\n특별한 것은 없어 보인다.";


    [Header("불이 꺼져 있을 때")]
    [TextArea(2, 5)]
    [SerializeField]
    private string findMessage = "책상 안쪽에 무언가 끼어 있다.";

    [TextArea(2, 5)]
    [SerializeField]
    private string getKeyMessage = "낡은 열쇠를 얻었다.";


    [Header("열쇠 획득 후")]
    [TextArea(2, 5)]
    [SerializeField]
    private string emptyMessage = "책상 안은 비어 있다.";


    [Header("사진")]
    [SerializeField] private Sprite studentPhoto;


    // 열쇠를 가져갔는지
    private bool hasTakenKey = false;


    public void Interact()
    {
        if (lightSwitch == null || dialogueManager == null)
        {
            return;
        }


        // =========================
        // 이미 열쇠를 가져감
        // =========================
        if (hasTakenKey)
        {
            dialogueManager.ShowDialogue(emptyMessage);
            return;
        }


        // =========================
        // 불이 켜져 있음
        // =========================
        if (lightSwitch.IsLightOn())
        {
            dialogueManager.ShowDialogue(lightOnMessage);
            return;
        }


        // =========================
        // 불이 꺼져 있음
        // 열쇠 발견
        // =========================
        hasTakenKey = true;


        // =========================
        // 문에 열쇠 획득 전달
        // =========================
        if (classroomDoor != null)
        {
            classroomDoor.GetKey();
        }


        // 사진을 대화 종료 후 보여주도록 예약
        dialogueManager.SetNextPhoto(
            studentPhoto,
            "렌",
            "이 여자애는..."
        );


        dialogueManager.ShowNarrationSequence(
            findMessage,
            getKeyMessage,
            "사진 한 장이 함께 끼어 있다."
        );
    }


    public bool HasKey()
    {
        return hasTakenKey;
    }
}