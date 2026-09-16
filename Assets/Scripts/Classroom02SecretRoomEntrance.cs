using System.Collections;
using UnityEngine;

public class Classroom02SecretRoomEntrance : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("비밀방 내부 위치")]
    [SerializeField] private Transform secretRoomSpawnPoint;

    [Header("카메라")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform secretRoomCameraPoint;
    [SerializeField] private float cameraMoveDuration = 0.8f;
    [Header("비밀방 스위치")]
    [SerializeField] private Classroom02SecretSwitch secretSwitch;


    private bool isUnlocked = false;
    private bool hasEntered = false;


    public void UnlockEntrance()
    {
        isUnlocked = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUnlocked)
        {
            return;
        }

        if (hasEntered)
        {
            return;
        }


        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            return;
        }


        hasEntered = true;

        StartCoroutine(
            EnterSecretRoomRoutine(player)
        );
    }


    private IEnumerator EnterSecretRoomRoutine(
        PlayerController player)
    {
        // =========================
        // 플레이어 이동 잠금
        // =========================
        player.SetMovementLocked(true);


        // =========================
        // 플레이어를 비밀방 내부로 이동
        // =========================
        if (secretRoomSpawnPoint != null)
        {
            Rigidbody2D rb =  player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.position = secretRoomSpawnPoint.position;
            }
            else
            {
                player.transform.position = secretRoomSpawnPoint.position;
            }
        }
        // =========================
        // 비밀방 진입 후 밝기 복구
        // =========================
        if (secretSwitch != null)
        {
            secretSwitch.RestoreLight();
        }



        // =========================
        // 카메라 Y축만 아래로 이동
        // =========================
        if (mainCamera != null &&
            secretRoomCameraPoint != null)
        {
            Vector3 startPosition =
                mainCamera.position;

            Vector3 targetPosition =
                new Vector3(
                    startPosition.x,
                    secretRoomCameraPoint.position.y,
                    startPosition.z
                );


            float elapsedTime = 0f;


            while (elapsedTime < cameraMoveDuration)
            {
                elapsedTime += Time.deltaTime;

                float t =
                    elapsedTime / cameraMoveDuration;

                t = Mathf.SmoothStep(
                    0f,
                    1f,
                    t
                );


                mainCamera.position =
                    Vector3.Lerp(
                        startPosition,
                        targetPosition,
                        t
                    );

                yield return null;
            }


            mainCamera.position =
                targetPosition;
        }


        // 살짝 정적
        yield return new WaitForSeconds(0.3f);


        // =========================
        // 비밀방 내부 대사
        // =========================
        dialogueManager.ShowDialogueSequence(
            "렌",
            "…여긴 뭐지?",
            "이런 공간이 있었나…"
        );


        // 대화 종료 대기
        while (dialogueManager.IsDialogueOpen())
        {
            yield return null;
        }


        // =========================
        // 플레이어 이동 해제
        // =========================
        player.SetMovementLocked(false);
    }
}