using System.Collections;
using UnityEngine;

public class PlayerSitSleepController : MonoBehaviour
{
    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;
    public Animator animator;
    public MonoBehaviour movementScript;  
    public Rigidbody2D rb;

    [Header("Time")]
    public GameTimeManager timeManager;  
    public int wakeUpHour = 9;
    public int wakeUpMinute = 0;

    [Header("Snap")]
    public float snapSpeed = 20f;

    [Header("Animator")]
    public string sitTrigger = "Sit";
    public string sleepTrigger = "Sleep";
    public string exitTrigger = "ExitAction";
    public string idleState = "IdleAnimation";
    public string sitIdleState = "SitIdle";
    public string sleepIdleState = "SleepIdle";

    [Header("UI")]
    public SleepBlockedUI sleepBlockedUI;


    private InteractableSpot currentSpot;

    private bool isInAction;
    private bool isTransitioning;
    private SpotType currentActionType;

    private Coroutine waitPoseRoutine;
    private Coroutine exitRoutine;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(interactKey)) return;
        if (isTransitioning) return;

        if (!isInAction)
            TryStartAction();
        else
            StartExit();
    }

    private void TryStartAction()
    {
        if (currentSpot == null || currentSpot.actionPoint == null) return;
        if (animator == null) return;

        // Block sleep unless night
        if (currentSpot.type == SpotType.Sleep)
        {
            if (timeManager == null)
            {
                return;
            }

            if (!timeManager.IsNightForSleeping())
            {
                if (sleepBlockedUI != null)
                    sleepBlockedUI.Show();
                return;
            }
        }

        // Cancel routines 
        if (waitPoseRoutine != null) { StopCoroutine(waitPoseRoutine); waitPoseRoutine = null; }
        if (exitRoutine != null) { StopCoroutine(exitRoutine); exitRoutine = null; }

        isTransitioning = true;
        isInAction = true;
        currentActionType = currentSpot.type;

        // Disable movement
        if (movementScript != null) movementScript.enabled = false;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Snap 
        StartCoroutine(SnapTo(currentSpot.actionPoint.position));

        animator.ResetTrigger(exitTrigger);
        animator.ResetTrigger(sitTrigger);
        animator.ResetTrigger(sleepTrigger);

        if (currentActionType == SpotType.Sit)
            animator.SetTrigger(sitTrigger);
        else
            animator.SetTrigger(sleepTrigger);

        // Wait until idle 
        string targetIdlePose = (currentActionType == SpotType.Sit) ? sitIdleState : sleepIdleState;
        waitPoseRoutine = StartCoroutine(WaitUntilState(targetIdlePose, () =>
        {
            isTransitioning = false;
        }));
    }

    private void StartExit()
    {
        if (!isInAction) return;
        if (animator == null) return;
        if (waitPoseRoutine != null) { StopCoroutine(waitPoseRoutine); waitPoseRoutine = null; }

        isTransitioning = true;
        animator.ResetTrigger(exitTrigger);
        animator.SetTrigger(exitTrigger);

        // Wait until idle
        exitRoutine = StartCoroutine(WaitUntilState(idleState, () =>
        {
            // set time to 09:00
            if (currentActionType == SpotType.Sleep && timeManager != null)
            {
                timeManager.SetTime(wakeUpHour, wakeUpMinute);
            }

            // Restore movement
            if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
            if (movementScript != null) movementScript.enabled = true;

            isInAction = false;
            isTransitioning = false;
        }));
    }

    private IEnumerator WaitUntilState(string stateName, System.Action onReached)
    {
        yield return null;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        onReached?.Invoke();
    }

    private IEnumerator SnapTo(Vector2 target)
    {
        for (int i = 0; i < 8; i++)
        {
            transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * snapSpeed);
            yield return null;
        }
        transform.position = target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var spot = other.GetComponent<InteractableSpot>();
        if (spot != null) currentSpot = spot;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var spot = other.GetComponent<InteractableSpot>();
        if (spot != null && spot == currentSpot) currentSpot = null;
    }
}
