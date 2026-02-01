using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public bool loadsScene;
    public string sceneToLoad;

    public bool isLocked;
    public string lockedMessage = "Door is locked";

    [Header("Prompt")]
    [SerializeField] private DoorPrompt prompt;

    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void Interact()
    {
        if (isLocked)
        {
            if (LockedDoorUI.I != null)
                LockedDoorUI.I.Show();

            return;
        }


        if (loadsScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            if (TransitionManager.I != null)
                TransitionManager.I.LoadSceneWithFade(sceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (prompt != null) prompt.ShowPrompt(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (prompt != null) prompt.ShowPrompt(false);
    }
}
