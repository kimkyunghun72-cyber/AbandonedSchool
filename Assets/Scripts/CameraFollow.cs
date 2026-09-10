using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("추적 대상")]
    [SerializeField] private Transform target;

    [Header("카메라 이동")]
    [SerializeField] private float smoothSpeed = 5f;

    [Header("카메라 왼쪽 제한")]
    [SerializeField] private float minX = 0f;

    private float fixedY;
    private float fixedZ;


    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }


    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 플레이어 X를 따라감
        float followX = Mathf.Lerp( transform.position.x, target.position.x, smoothSpeed * Time.deltaTime );

        // 카메라 자체가 minX보다 왼쪽으로 못 가게 강제 제한
        followX = Mathf.Max(followX, minX);

        transform.position = new Vector3(followX, fixedY, fixedZ );
    }
}