using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode pickupKey = KeyCode.E;


    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange) return;
        if (!Input.GetKeyDown(pickupKey)) return;

        if (InventoryManager.I != null)
            InventoryManager.I.AddBathroomKey();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
    }
}