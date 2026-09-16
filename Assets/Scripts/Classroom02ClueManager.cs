using UnityEngine;

public class Classroom02ClueManager : MonoBehaviour
{
    [Header("비밀방 단서")]
    [SerializeField] private Classroom02PlayerPhotoEvent playerPhotoEvent;
    [SerializeField] private Classroom02OtherGirlsPhotoEvent otherGirlsPhotoEvent;
    [SerializeField] private Classroom02TeacherRecordEvent teacherRecordEvent;
    [SerializeField] private Classroom02GroupPhotoEvent groupPhotoEvent;

    [Header("마지막 메모")]
    [SerializeField] private GameObject finalGirlMemo;

    private bool hasRevealedFinalMemo = false;


    private void Start()
    {
        if (finalGirlMemo != null)
        {
            finalGirlMemo.SetActive(false);
        }
    }


    private void Update()
    {
        // 이미 등장했다면 더 이상 검사하지 않음
        if (hasRevealedFinalMemo)
        {
            return;
        }


        // 연결이 안 된 것이 있으면 검사하지 않음
        if (playerPhotoEvent == null ||
            otherGirlsPhotoEvent == null ||
            teacherRecordEvent == null ||
             groupPhotoEvent == null)
        {
            return;
        }


        // =========================
        // 세 단서를 모두 조사했는지 확인
        // =========================
        if (playerPhotoEvent.HasCheckedPhoto() &&
            otherGirlsPhotoEvent.HasCheckedPhoto() &&
            teacherRecordEvent.HasReadRecord() &&
            groupPhotoEvent.HasCheckedPhoto())
        {
            RevealFinalMemo();
        }
    }


    private void RevealFinalMemo()
    {
        hasRevealedFinalMemo = true;

        if (finalGirlMemo != null)
        {
            finalGirlMemo.SetActive(true);
        }
    }
}