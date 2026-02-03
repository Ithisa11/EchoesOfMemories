using System.Collections;
using UnityEngine;

public class WakeOnSceneEnter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour movementScript; 
    [SerializeField] private Rigidbody2D rb;

    [Header("Animator")]
    [SerializeField] private string sleepIdleState = "SleepIdle";
    [SerializeField] private string idleState = "IdleAnimation";
    [SerializeField] private string exitTrigger = "ExitAction";

    [Header("Delay")]
    [SerializeField] private float startDelay = 0.05f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (!SpawnRouter.playWakeOnNextScene) return;

        SpawnRouter.playWakeOnNextScene = false;
        StartCoroutine(PlayWakeRoutine());
    }

    private IEnumerator PlayWakeRoutine()
    {
        if (animator == null) yield break;

        // lock control (wake)
        if (movementScript != null) movementScript.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);

        // SleepIdle first
        animator.Play(sleepIdleState, 0, 0f);
        yield return null;

        // trigger wake 
        animator.ResetTrigger(exitTrigger);
        animator.SetTrigger(exitTrigger);

        // animation set
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(idleState))
            yield return null;

        // restore control
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
        if (movementScript != null) movementScript.enabled = true;
    }
}