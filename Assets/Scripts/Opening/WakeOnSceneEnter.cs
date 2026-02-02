using System.Collections;
using UnityEngine;

public class WakeOnSceneEnter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour movementScript; // your CharacterM (optional)
    [SerializeField] private Rigidbody2D rb;

    [Header("Animator")]
    [SerializeField] private string sleepIdleState = "SleepIdle";
    [SerializeField] private string idleState = "IdleAnimation";
    [SerializeField] private string exitTrigger = "ExitAction";

    [Header("Timing")]
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

        // optional: lock control during wake
        if (movementScript != null) movementScript.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // let the scene settle 1 frame
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);

        // force the animator into SleepIdle first (so the wake makes sense)
        animator.Play(sleepIdleState, 0, 0f);
        yield return null;

        // trigger wake (your existing transition)
        animator.ResetTrigger(exitTrigger);
        animator.SetTrigger(exitTrigger);

        // wait until we're back to IdleAnimation
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(idleState))
            yield return null;

        // restore control
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
        if (movementScript != null) movementScript.enabled = true;
    }
}