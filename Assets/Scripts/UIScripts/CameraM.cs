using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class CameraM : MonoBehaviour
{
    public Transform target;

    [Header("Bounderies")]
    public float minX = -33.33f;
    public float maxX = 44.95f;
    public float smoothSpeed = 8f;

    private Camera cam;
    private float fixedY;

    void Awake()
    {
        cam = GetComponent<Camera>();
        fixedY = transform.position.y;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        AssignTargetIfMissing();
        SnapToTarget();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignTargetIfMissing();
        SnapToTarget();
    }

    private void AssignTargetIfMissing()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player != null ? player.transform : null;
    }

    private void SnapToTarget()
    {
        if (target == null) return;

        float halfWidth = cam.orthographicSize * cam.aspect;
        float clampedX = Mathf.Clamp(target.position.x, minX + halfWidth, maxX - halfWidth);

        transform.position = new Vector3(clampedX, fixedY, -10f);
    }

    void LateUpdate()
    {
        if (target == null)
        {
            AssignTargetIfMissing();
            if (target == null) return;
        }

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
