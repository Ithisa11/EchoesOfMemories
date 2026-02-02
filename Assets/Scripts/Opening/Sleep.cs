using System.Collections;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;

    [Header("References")]
    public Animator animator;
    public MonoBehaviour movementScript;   // Drag CharacterM here
    public Rigidbody2D rb;

    [Header("Time")]
    public GameTimeManager timeManager;    // Drag your GameTimeManager here
    public int wakeUpHour = 9;
    public int wakeUpMinute = 0;

    [Header("Snap")]
    public float snapSpeed = 20f;

    [Header("Animator Triggers")]
    public string sitTrigger = "Sit";
    public string sleepTrigger = "Sleep";
    public string exitTrigger = "ExitAction";

    [Header("Animator State Names")]
    public string idleState = "IdleAnimation";
    public string sitIdleState = "SitIdle";
    public string sleepIdleState = "SleepIdle";

    [Header("Cutscene (Sleep -> Bedroom)")]
    public CutsceneEndLoadScene cutsceneFlow;   // Drag CutscenePlayer here
    public float cutsceneStartDelay = 0.05f;    // tiny delay to let pose settle

    private InteractableSpot currentSpot;

    private bool isInAction;
    private bool isTransitioning;
    private bool isSleepingAndCutsceneStarted;
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

        // While sleeping we don't allow "exit" anymore (cutscene will take over)
        if (isSleepingAndCutsceneStarted) return;

        if (!isInAction)
            TryStartAction();
        else
            StartExit(); // only meaningful for Sit now
    }

    private void TryStartAction()
    {
        if (currentSpot == null || currentSpot.actionPoint == null) return;
        if (animator == null) return;

        // Cancel any pending routines (safety)
        if (waitPoseRoutine != null) { StopCoroutine(waitPoseRoutine); waitPoseRoutine = null; }
        if (exitRoutine != null) { StopCoroutine(exitRoutine); exitRoutine = null; }

        isTransitioning = true;
        isInAction = true;
        currentActionType = currentSpot.type;

        // Disable movement
        if (movementScript != null) movementScript.enabled = false;

        // Freeze physics
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Snap to chair/bed point
        StartCoroutine(SnapTo(currentSpot.actionPoint.position));

        // Fire enter trigger ONCE
        animator.ResetTrigger(exitTrigger);
        animator.ResetTrigger(sitTrigger);
        animator.ResetTrigger(sleepTrigger);

        if (currentActionType == SpotType.Sit)
            animator.SetTrigger(sitTrigger);
        else
            animator.SetTrigger(sleepTrigger);

        // Wait until we arrive at the idle pose state (SitIdle/SleepIdle)
        string targetIdlePose = (currentActionType == SpotType.Sit) ? sitIdleState : sleepIdleState;

        waitPoseRoutine = StartCoroutine(WaitUntilState(targetIdlePose, () =>
        {
            isTransitioning = false;

            // If we just entered SleepIdle, start cutscene immediately
            if (currentActionType == SpotType.Sleep)
            {
                StartCoroutine(StartCutsceneAfterSleepPose());
            }
        }));
    }

    private IEnumerator StartCutsceneAfterSleepPose()
    {
        if (isSleepingAndCutsceneStarted) yield break;
        isSleepingAndCutsceneStarted = true;

        // Optional tiny delay so animation fully settles / avoids a 1-frame pop
        if (cutsceneStartDelay > 0f)
            yield return new WaitForSeconds(cutsceneStartDelay);

        // Set wake-up time now (since we won't "exit" sleep anymore)
        if (timeManager != null)
            timeManager.SetTime(wakeUpHour, wakeUpMinute);

        // Play cutscene (it will load Bedroom when finished)
        if (cutsceneFlow != null)
        {
            cutsceneFlow.Play();
        }
        else
        {
            Debug.LogError("Sleep: cutsceneFlow is not assigned.");
            // Fallback: restore control so you don't get stuck
            RestoreControl();
            isSleepingAndCutsceneStarted = false;
        }
    }

    // Sit exit still works normally
    private void StartExit()
    {
        if (!isInAction) return;
        if (animator == null) return;

        // If current action is sleep, ignore exit (we don't want wake-up triggered by key)
        if (currentActionType == SpotType.Sleep) return;

        if (waitPoseRoutine != null) { StopCoroutine(waitPoseRoutine); waitPoseRoutine = null; }

        isTransitioning = true;

        animator.ResetTrigger(exitTrigger);
        animator.SetTrigger(exitTrigger);

        exitRoutine = StartCoroutine(WaitUntilState(idleState, () =>
        {
            RestoreControl();
        }));
    }
    private void RestoreControl()
    {
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
        if (movementScript != null) movementScript.enabled = true;

        isInAction = false;
        isTransitioning = false;
        currentActionType = SpotType.Sit; // doesn't matter, just resets
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