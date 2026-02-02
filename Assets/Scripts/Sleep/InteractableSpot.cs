using UnityEngine;

public enum SpotType { Sit, Sleep }

[RequireComponent(typeof(Collider2D))]
public class InteractableSpot : MonoBehaviour
{
    public SpotType type = SpotType.Sit;

    [Tooltip("Where the player will snap to when interacting.")]
    public Transform actionPoint;

    [Header("Interaction UI")]
    [SerializeField] private InteractionS interactionUI;
    [SerializeField] private string playerTag = "Player";

    private int playerInsideCount = 0;

    private void Reset()
    {
        // Ensure trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Auto-assign action point
        if (actionPoint == null)
        {
            var child = transform.Find(type == SpotType.Sit ? "SitPoint" : "SleepPoint");
            if (child != null) actionPoint = child;
        }

        // Try auto-find UI
        if (interactionUI == null)
            interactionUI = GetComponentInChildren<InteractionS>();
    }

    private void Awake()
    {
        // Safety: hide UI at start
        if (interactionUI != null)
            interactionUI.ShowPrompt(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInsideCount++;
        if (playerInsideCount == 1)
            interactionUI?.ShowPrompt(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInsideCount = Mathf.Max(playerInsideCount - 1, 0);
        if (playerInsideCount == 0)
            interactionUI?.ShowPrompt(false);
    }
}