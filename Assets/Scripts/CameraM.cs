using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraM : MonoBehaviour
{
    public Transform target;

    [Header("Horizontal Bounds")]
    public float minX = -33.33f;
    public float maxX = 44.95f;

    [Header("Smoothing")]
    public float smoothSpeed = 8f;

    private Camera cam;
    private float fixedY;

    void Awake()
    {
        cam = GetComponent<Camera>();
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float halfWidth = cam.orthographicSize * cam.aspect;

        float clampedX = Mathf.Clamp(
            target.position.x,
            minX + halfWidth,
            maxX - halfWidth
        );

        Vector3 desiredPosition = new Vector3(clampedX, fixedY, -10f);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
